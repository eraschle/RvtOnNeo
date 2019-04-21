using System.Collections.Generic;
using System.IO;
using System.Net;
using Autodesk.Revit.DB;
using Gim.Revit.Addin.Journal;
using Gim.Revit.Documentation.Json;
using Gim.Revit.Documentation.Model;
using Gim.Revit.Helper;
using Newtonsoft.Json;

namespace Gim.Revit.Documentation
{
    public class DocumentationManager
    {
        public string CreateJson(Document document, DocumentationSetting setting)
        {
            var famDoc = new FamilyAdapter(document.OwnerFamily);
            var format = setting.FormatJson ? Formatting.Indented : Formatting.None;
            if (setting.WrapGimObjects)
            {
                var converters = new List<JsonConverter> {
                    new ParameterJsonConverter(),
                    new FamilyTypeJsonConverter()
                };
                var wrapper = new FamilyJsonWarpper(famDoc);
                return JsonConvert.SerializeObject(wrapper, format, converters.ToArray());
            }
            else
            {
                return JsonConvert.SerializeObject(famDoc, format);
            }
        }

        public void CreateJsonFamilyDoc(Document document, DocumentationSetting setting)
        {
            var jsonString = CreateJson(document, setting);
            var filePath = FileHelper.ChangeExtension(document.PathName, "json");
            File.WriteAllText(filePath, jsonString);
        }

        public void CreateWebFamilyDoc(Document document, DocumentationSetting setting)
        {
            //"curl -v -H \"Content-Type: application/json\" -X POST -d \"{}\" -H \"X-Redmine-API-Key: {}\" -H \"Connection: close\" http://{}:{}/{}.json".format(Data, ApiKey, Host, Port, Url)
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(setting.WebUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add($"X-Redmine-API-Key: {setting.ApiKey}");
            httpWebRequest.Headers.Add("Connection: close");

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var json = CreateJson(document, setting);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
    }
}
