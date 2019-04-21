using System;
using Gim.Revit.Helper;
using Rvt = Autodesk.Revit.DB;

namespace Gim.Revit.Wrapper
{

    public class ParameterValueWrapper : IParameterValueWrapper
    {
        private readonly Rvt.Parameter parameter;
        public ParameterValueWrapper(Rvt.Parameter rvtRarameter)
        {
            parameter = rvtRarameter;
        }

        private Rvt.Definition definition { get { return parameter.Definition; } }

        public string Name { get { return definition.Name; } }
        public string StringValue
        {
            get
            {
                switch (parameter.StorageType)
                {
                    case Rvt.StorageType.String:
                        return parameter.AsString();
                    default:
                        return parameter.AsValueString();
                }
            }
        }

        public object Value
        {
            get
            {
                if (IsString(out var stringValue)) { return stringValue; }
                else if (IsBool(out var boolValue)) { return boolValue; }
                else if (IsInt(out var intValue)) { return intValue; }
                else if (IsDouble(out var doubleValue)) { return doubleValue; }
                else if (IsElementId(out var elementIdValue)) { return elementIdValue; }
                else { return null; }
            }
        }

        public bool IsInt(out int? value)
        {
            value = null;
            if (parameter.StorageType == Rvt.StorageType.Integer)
            {
                value = parameter.AsInteger();
            }
            return value != null;
        }

        public bool IsDouble(out double? value, bool converted = true)
        {
            value = null;
            if (parameter.StorageType == Rvt.StorageType.Double)
            {
                var doubleValue = parameter.AsDouble();
                if (converted)
                {
                    var dut = parameter.DisplayUnitType;
                    doubleValue = ConvertHelper.ConvertFrom(doubleValue, dut);
                }
                value = doubleValue;
            }
            return value != null;
        }

        public bool IsString(out string value)
        {
            value = null;
            if (parameter.StorageType == Rvt.StorageType.String)
            {
                value = parameter.AsString();
            }
            return value != null;
        }

        public bool IsBool(out bool? value)
        {
            value = null;
            if (IsInt(out var intValue)
                && definition.ParameterType == Rvt.ParameterType.YesNo)
            {
                value = Convert.ToBoolean(intValue);
            }
            return value != null;
        }

        public bool IsElementId(out Rvt.ElementId value)
        {
            value = null;
            if (parameter.StorageType == Rvt.StorageType.ElementId)
            {
                value = parameter.AsElementId();
            }
            return value != null;
        }
    }
}
