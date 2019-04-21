using System.Collections.Generic;
using Rvt = Autodesk.Revit.DB;

namespace Gim.Revit.Documentation.Model
{
    class FamilyTypeAdapter : FamilyType
    {
        private readonly Rvt.FamilyType familyType;

        public FamilyTypeAdapter(Rvt.FamilyType rvtFamilyType)
        {
            familyType = rvtFamilyType;
        }

        public override string Name
        {
            get { return familyType.Name; }
        }

        private readonly IDictionary<string, object> parameterValues = new Dictionary<string, object>();
        public override IDictionary<string, object> ParameterValues
        {
            get { return parameterValues; }
        }

        internal void AddParameter(Rvt.FamilyParameter parameter, object value)
        {
            parameterValues.Add(parameter.Definition.Name, value);
        }
    }
}
