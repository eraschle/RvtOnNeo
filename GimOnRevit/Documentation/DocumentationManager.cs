using System.Collections.Generic;
using System.IO;
using System.Net;
using Autodesk.Revit.DB;
using Gim.Domain.Helpers;
using Gim.Domain.Writer;
using Gim.Revit.Addin.Journal;
using Gim.Revit.Documentation.Json;
using Gim.Revit.Documentation.Model;
using Gim.Revit.Helper;
using Newtonsoft.Json;

namespace Gim.Revit.Documentation
{
    public class DocumentationManager
    {
        private GimJsonWriter writer = new GimJsonWriter();

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

        public void CreateFbx(Document document, DocumentationSetting setting)
        {
            if (setting.ExportFbx == false) { return; }

            var folder = GetFolderPath(document);
            var name = GetFbxFileName(document);
            var views = Get3dViewSet(document);
            var option = GetOptions();
            document.Export(folder, name, views, option);
        }

        private string GetFbxFileName(Document document)
        {
            var fileName = FileHelper.ChangeExtension(document.PathName, "fbx");
            FileHelper.DeleteFile(fileName);
            return Path.GetFileName(fileName);
        }

        private string GetFolderPath(Document document)
        {
            var fileName = document.PathName;
            return Path.GetDirectoryName(fileName);
        }

        private ViewSet Get3dViewSet(Document document)
        {
            var views = new ViewSet();
            views.Insert(ViewHelper.Default3D(document));
            return views;
        }

        private FBXExportOptions GetOptions()
        {
            return new FBXExportOptions
            {
                WithoutBoundaryEdges = true,
                LevelsOfDetailValue = 15, // range 1..15 
                UseLevelsOfDetail = true,
                StopOnError = false
            };
        }
    }
}
