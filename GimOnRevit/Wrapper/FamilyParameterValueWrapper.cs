using System;
using Gim.Revit.Helper;
using Rvt = Autodesk.Revit.DB;

namespace Gim.Revit.Wrapper
{
    public class FamilyParameterValueWrapper : IParameterValueWrapper
    {
        private readonly Rvt.FamilyParameter parameter;

        private readonly Rvt.FamilyType currentType;

        public FamilyParameterValueWrapper(Rvt.FamilyManager manager, Rvt.FamilyParameter rvtParameter)
            : this(manager.CurrentType, rvtParameter) { }

        public FamilyParameterValueWrapper(Rvt.FamilyType current, Rvt.FamilyParameter rvtRarameter)
        {
            parameter = rvtRarameter;
            currentType = current;
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
                        return currentType.AsString(parameter);
                    default:
                        return currentType.AsValueString(parameter);
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
                value = currentType.AsInteger(parameter);
            }
            return value != null;
        }

        public bool IsDouble(out double? value, bool converted = true)
        {
            value = null;
            if (parameter.StorageType == Rvt.StorageType.Double)
            {
                value = currentType.AsDouble(parameter);
                if (converted && value != null)
                {
                    var dut = parameter.DisplayUnitType;
                    value = ConvertHelper.ConvertFrom((double)value, dut);
                }
            }
            return value != null;
        }

        public bool IsString(out string value)
        {
            value = null;
            if (parameter.StorageType == Rvt.StorageType.String)
            {
                value = currentType.AsString(parameter);
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
                value = currentType.AsElementId(parameter);
            }
            return value != null;
        }
    }
}
