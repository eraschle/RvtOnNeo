using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Gim.Domain.Model.Element;

namespace Gim.Revit.Gim.Element
{

    public class LevelRepo
    {
        private readonly Document document;
        public LevelRepo(Document document)
        {
            this.document = document;
        }

        public IList<GimLevel> AllLevel()
        {
            var levels = new FilteredElementCollector(document)
                .OfCategory(BuiltInCategory.OST_Levels)
                .WhereElementIsNotElementType()
                .OfType<Level>();
            return levels.Select(level => new GimLevelWrapper(level))
                .OfType<GimLevel>().ToList();
        }
    }
}
