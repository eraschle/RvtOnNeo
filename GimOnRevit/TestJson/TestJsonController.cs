using Rvt = Autodesk.Revit.DB;
using Gim.Revit.Documentation.Model;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System;
using Gim.Revit.TestJson;
using System.Text;
using static Gim.Revit.Helper.FileHelper;
using System.Globalization;

namespace Gim.Revit
{
    public class TestJsonController
    {
        private readonly string outputPath = @"C:\OneDrive\Workspace\vm_share\ubuntu\gim\json\common\categories";
        private static Encoding utf8Encoding = new UTF8Encoding(false);
        private Formatting jsonFormat = Formatting.Indented;
        private const string nullValue = "\"NULL\"";

        private string CreateJson(object serialzeObject)
        {
            var setting = new JsonSerializerSettings
            {
                Formatting = jsonFormat,
                NullValueHandling = NullValueHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.Default,
                Culture = CultureInfo.CurrentCulture
            };
            return JsonConvert.SerializeObject(serialzeObject, setting);
        }

        private string CreateJsonString(object jsonObject)
        {
            var jsonString = CreateJson(jsonObject);

            return string.IsNullOrEmpty(jsonString) ? jsonString
                : jsonString.Replace("null", nullValue);
        }

        private string GetFilePath(Rvt.Document document, string fileNamePrefix)
        {
            var fileName = $"{fileNamePrefix}_{jsonFormat.ToString()}.json";
            fileName = RepairInvalidFileName(fileName);
            var directory = RepairInvalidPathName(outputPath);
            var filePath = Path.Combine(directory, fileName);
            return filePath;
        }


        private void WrtieFiles<T>(Rvt.Document document, IList<string> fileNamePrefixes, IList<T> jsonObjects) where T : class
        {
            for (var idx = 0; idx < jsonObjects.Count; idx++)
            {
                WrtieFile(document, fileNamePrefixes[idx], jsonObjects[idx]);
            }
        }

        private void WrtieFile(Rvt.Document document, string fileNamePrefix, object jsonObject)
        {
            var fromLineEnd = NewLineSymbols[NewLine.Windows];
            var toLineEnd = NewLineSymbols[NewLine.Linux];


            var jsonString = CreateJsonString(jsonObject);
            var splits = new List<string> { fromLineEnd }.ToArray();
            var allLines = jsonString.Split(splits, StringSplitOptions.None);

            var filePath = GetFilePath(document, fileNamePrefix);
            try
            {
                using (var writer = new StreamWriter(filePath, append: false, utf8Encoding))
                {
                    //writer.Write(jsonString);
                    writer.NewLine = toLineEnd;
                    //foreach (var line in allLines)
                    //{
                    //    writer.WriteLine(line);s
                    //}
                    for (var idx = 0; idx < allLines.Length; idx++)
                    {
                        var line = allLines[idx];
                        if (idx < allLines.Length - 1)
                        {
                            writer.WriteLine(line);
                        }
                        else
                        {
                            writer.Write(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"File {filePath}", ex);
            }
        }

        public void CreateCategories(Rvt.Document document)
        {
            var jsonObjects = CategoryJsonObjectList(document);
            var objectNames = CategoryNames(jsonObjects.Categories);
            WrtieFiles(document, objectNames, jsonObjects.Categories);
        }
        private IList<string> CategoryNames(IList<CategoryJsonWrapper> adapters)
        {
            var names = new List<string>();
            foreach (var adapter in adapters)
            {
                var category = adapter.Category;
                var categoryName = RepairInvalidFileName(category.Name);
                var fileName = $"{Math.Abs(category.RevitId)}_{categoryName}";
                names.Add(fileName);
            }
            return names;
        }

        private CategoriesWrapper CategoryJsonObjectList(Rvt.Document document)
        {
            var rvtCategories = Enum.GetValues(typeof(Rvt.BuiltInCategory));
            var adapters = new List<CategoryJsonWrapper>();
            foreach (Rvt.BuiltInCategory bic in rvtCategories)
            {
                Rvt.Category category = null;
                try { category = Rvt.Category.GetCategory(document, bic); }
                catch (Exception) { }

                if (category is null) { continue; }

                var adapter = new CategoryAdapter(category);
                var wrapper = new CategoryJsonWrapper { Category = adapter };
                adapters.Add(wrapper);
            }
            return new CategoriesWrapper(adapters);
        }

        public void CreateBips(Rvt.Document document)
        {
            var jsonObject = BipJsonObject();
            WrtieFile(document, "BuiltInParameter", jsonObject);
        }

        private IList<BipParameter> BipJsonObject()
        {
            var bipParameters = Enum.GetValues(typeof(Rvt.BuiltInParameter));
            var adapters = new List<BipParameter>();
            foreach (Rvt.BuiltInParameter bip in bipParameters)
            {
                try
                {
                    var adapter = new BipParameter
                    {
                        RevitId = bip.GetHashCode(),
                        RevitName = bip.ToString(),
                        Name = Rvt.LabelUtils.GetLabelFor(bip)
                    };
                    adapters.Add(adapter);
                }
                catch { }
            }
            return adapters;
        }

        public void CreateUnits(Rvt.Document document)
        {
            var jsonObject = UnitJsonObject();
            WrtieFile(document, "UnityTypes", jsonObject);
        }

        private IList<UnitType> UnitJsonObject()
        {
            var unitTypes = Rvt.UnitUtils.GetValidUnitTypes();
            var adapters = new List<UnitType>();
            foreach (var unitType in unitTypes)
            {
                var unitGroup = Rvt.UnitUtils.GetUnitGroup(unitType);
                var displayUnits = Rvt.UnitUtils.GetValidDisplayUnits(unitType);
                var typeCatalog = Rvt.UnitUtils.GetTypeCatalogString(unitType);
                try
                {
                    var adapter = new UnitType
                    {
                        RevitId = unitType.GetHashCode(),
                        RevitName = unitType.ToString(),
                        UnitGroup = unitGroup.ToString(),
                        DisplayUnits = GetDisplayUnitTypes(displayUnits),
                        TypeCatalog = typeCatalog
                    };
                    adapters.Add(adapter);
                }
                catch { }
            }
            return adapters;
        }

        private IList<string> GetDisplayUnitTypes(IList<Rvt.DisplayUnitType> rvtUnitTypes)
        {
            var unitTypes = new List<string>();
            foreach (var rvtUnitType in rvtUnitTypes)
            {
                var unitType = Rvt.LabelUtils.GetLabelFor(rvtUnitType);
                unitTypes.Add(unitType);
            }
            return unitTypes;
        }
    }

    public class CategoriesWrapper
    {
        public IList<CategoryJsonWrapper> Categories { get; set; }

        public CategoriesWrapper(IList<CategoryJsonWrapper> categories)
        {
            Categories = categories;
        }
    }
}
