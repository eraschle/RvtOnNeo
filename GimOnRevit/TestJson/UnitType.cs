using System.Collections.Generic;

namespace Gim.Revit.TestJson
{
    public class UnitType
    {
        public int RevitId { get; set; }
        public string RevitName { get; set; }
        public string Name { get; set; }
        public string UnitGroup { get; set; }
        public IList<string> DisplayUnits { get; set; }
        public string TypeCatalog { get; set; }
    }
}
