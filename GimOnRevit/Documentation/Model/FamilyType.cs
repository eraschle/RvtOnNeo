using System.Collections.Generic;

namespace Gim.Revit.Documentation.Model
{
    public class FamilyType
    {
        public virtual string Name { get; set; }

        public virtual IDictionary<string, object> ParameterValues { get; set; }
    }
}
