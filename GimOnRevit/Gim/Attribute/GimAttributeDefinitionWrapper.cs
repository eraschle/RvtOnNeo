using Autodesk.Revit.DB;
using Gim.Domain.Model.Attribute;
using Gim.Revit.Helper;

namespace Gim.Revit.Gim.Attribute
{
    class GimAttributeDefinitionWrapper : GimAttributeDefinition
    {
        public GimAttributeDefinitionWrapper(Parameter parameter)
        {
            var parameterType = parameter.Definition.ParameterType;
            DefinitionType = LabelHelper.Get(parameterType);
            ValueType = ValueTypeHelper.Get(parameter);
        }
    }
}
