namespace Gim.Revit.Documentation.Json
{
    using System;

    public class ParameterJsonConverter : AGenericJsonConverter
    {
        private readonly Type parameterType = typeof(Parameter);
        public override bool CanConvert(Type objectType)
        {
            var canConvert = objectType.IsSubclassOf(parameterType);
            return canConvert;
        }

        protected override string PropertyName
        {
            get { return parameterType.Name; }
        }
    }
}
