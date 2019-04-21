using Rvt = Autodesk.Revit.DB;

namespace Gim.Revit.Wrapper
{
    public interface IParameterValueWrapper
    {
        string StringValue { get; }

        object Value { get; }

        bool IsInt(out int? value);

        bool IsDouble(out double? value, bool converted = true);

        bool IsString(out string value);

        bool IsBool(out bool? value);

        bool IsElementId(out Rvt.ElementId value);

    }
}
