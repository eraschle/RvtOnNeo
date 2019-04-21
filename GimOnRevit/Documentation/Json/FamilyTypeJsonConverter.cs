using System;
using Gim.Revit.Documentation.Model;

namespace Gim.Revit.Documentation.Json
{
    public class FamilyTypeJsonConverter : AGenericJsonConverter
    {
        private readonly Type familyType = typeof(FamilyType);
        public override bool CanConvert(Type objectType)
        {
            var canConvert = objectType.IsSubclassOf(familyType);
            return canConvert;
        }

        protected override string PropertyName
        {
            get { return familyType.Name; }
        }
    }
}
