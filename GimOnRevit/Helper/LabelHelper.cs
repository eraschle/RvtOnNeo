using System;
using Autodesk.Revit.DB;

namespace Gim.Revit.Helper
{
    internal class LabelHelper
    {
        public static string Get(ParameterType parameterType)
        {
            try { return LabelUtils.GetLabelFor(parameterType); }
            catch(Exception ex)
            {
                return parameterType.ToString();
            }
        }

        public static string Get(DisplayUnitType displayUnitType)
        {
            try { return LabelUtils.GetLabelFor(displayUnitType); }
            catch (Exception ex)
            {
                return displayUnitType.ToString();
            }
        }
    }
}
