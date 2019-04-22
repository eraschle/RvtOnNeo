using System;
using Autodesk.Revit.DB;

namespace Gim.Revit.Helper
{
    internal class LabelHelper
    {
        public static string Get(ParameterType parameterType,
            ParameterType onError = ParameterType.Invalid)
        {
            try { return LabelUtils.GetLabelFor(parameterType); }
            catch (Exception)
            {
                try { return LabelUtils.GetLabelFor(onError); }
                catch (Exception) { return onError.ToString(); }
            }
        }

        public static string Get(BuiltInParameter builtInParameter,
            BuiltInParameter onError = BuiltInParameter.INVALID)
        {
            try { return LabelUtils.GetLabelFor(builtInParameter); }
            catch (Exception)
            {
                try { return LabelUtils.GetLabelFor(onError); }
                catch (Exception) { return onError.ToString(); }
            }
        }

        public static string Get(BuiltInParameterGroup builtInParameterGroup,
            BuiltInParameterGroup onError = BuiltInParameterGroup.INVALID)
        {
            try { return LabelUtils.GetLabelFor(builtInParameterGroup); }
            catch (Exception)
            {
                try { return LabelUtils.GetLabelFor(onError); }
                catch (Exception) { return onError.ToString(); }
            }
        }

        public static string Get(DisplayUnitType displayUnitType,
            DisplayUnitType onError = DisplayUnitType.DUT_UNDEFINED)
        {
            try { return LabelUtils.GetLabelFor(displayUnitType); }
            catch (Exception)
            {
                try { return LabelUtils.GetLabelFor(onError); }
                catch (Exception) { return onError.ToString(); }
            }
        }


        public static string Get(UnitType unitType,
            UnitType onError = UnitType.UT_Undefined)
        {
            try { return LabelUtils.GetLabelFor(unitType); }
            catch (Exception)
            {
                try { return LabelUtils.GetLabelFor(onError); }
                catch (Exception) { return onError.ToString(); }
            }
        }


        public static string Get(UnitSymbolType unitSymbolType,
            UnitSymbolType onError = UnitSymbolType.UST_NONE)
        {
            try { return LabelUtils.GetLabelFor(unitSymbolType); }
            catch (Exception)
            {
                try { return LabelUtils.GetLabelFor(onError); }
                catch (Exception) { return onError.ToString(); }
            }
        }
    }
}
