using System.Collections.Generic;
using System.IO;
using System.Net;
using Autodesk.Revit.DB;
using Gim.Domain.Helpers;
using Gim.Domain.Writer;
using Gim.Revit.Addin.Journal;
using Gim.Revit.Documentation.Json;
using Gim.Revit.Documentation.Model;
using Gim.Revit.Documentation.Wrapper;
using Gim.Revit.Helper;
using Newtonsoft.Json;

namespace Gim.Revit.Documentation
{
    public class DocumentationManager
    {
        private readonly GimJsonWriter writer = new GimJsonWriter();

        private ICollection<JsonConverter> GetConverters(DocumentationSetting setting)
        {
            return setting.WrapGimObjects
                ? new List<JsonConverter> {
                    new ParameterJsonConverter(),
                    new FamilyTypeJsonConverter()
                }
                : null;
        }

        public void CreateJsonFamilyDoc(Document document, DocumentationSetting setting)
        {
            object jsonObject = new FamilyAdapter(document.OwnerFamily, setting.LibraryRoot);
            var converters = GetConverters(setting);
            if (setting.WrapGimObjects)
            {
                jsonObject = new FamilyWarpper(jsonObject as FamilyAdapter);
            }

            var filePath = setting.CreateOutputFilePath(document.PathName, "json");
            writer.Formating = setting.FormatJson;
            writer.WrtieFile(filePath, jsonObject, converters);
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
                var converters = GetConverters(setting);
                //TODO: Change if a web request depends on the format...
                writer.Formating = false;
                var json = writer.CreateJson(document, converters);

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

            var views = Get3dViewSet(document);
            var option = GetOptions();

            var filePath = GetFbxFileName(document, setting);
            FileHelper.DeleteFile(filePath);
            var folderPath = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileName(filePath);
            document.Export(folderPath, fileName, views, option);
        }

        private string GetFbxFileName(Document document, DocumentationSetting setting)
        {
            var filePath = setting.CreateOutputFilePath(document.PathName, "fbx");
            return filePath;
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
