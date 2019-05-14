using Rvt = Autodesk.Revit.DB;
using Gim.Revit.Documentation.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using Gim.Revit.TestJson;
using Gim.Domain.Helpers;
using Gim.Domain.Writer;

namespace Gim.Revit
{
    public class TestJsonController
    {
        private readonly GimJsonWriter writer = new GimJsonWriter();
        private readonly string outputPath = @"C:\OneDrive\Workspace\rascer\rest-api\family";
        private readonly Formatting jsonFormat = Formatting.None;

        private string GetFilePath(string outputPath, string fileNamePrefix)
        {
            var fileName = $"{fileNamePrefix}_{jsonFormat.ToString()}.json";
            return writer.GetFilePath(outputPath, fileName);
        }

        public void CreateCategories(Rvt.Document document)
        {
            var jsonObjects = CategoryJsonObjectList(document);
            var objectNames = CategoryNames(jsonObjects.Categories);
            writer.WrtieFiles(objectNames, jsonObjects.Categories);
        }
        private IList<string> CategoryNames(IList<CategoryJsonWrapper> adapters)
        {
            var names = new List<string>();
            foreach (var adapter in adapters)
            {
                var category = adapter.Category;
                var categoryName = FileHelper.RepairInvalidFileName(category.Name);
                var fileName = $"{Math.Abs(category.RevitId)}_{categoryName}";
                fileName = GetFilePath(outputPath, fileName);
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
            var fileName = GetFilePath(outputPath, "BuiltInParameter");
            writer.WrtieFile(fileName, jsonObject);
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
            var fileName = GetFilePath(outputPath, "UnityTypes");
            writer.WrtieFile(fileName, jsonObject);
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
