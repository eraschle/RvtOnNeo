using Autodesk.Revit.DB;

namespace Gim.Revit.Helper
{
    public class ConvertHelper
    {
        public static double ConvertFrom(double value, DisplayUnitType displayUnitType)
        {
            try { return UnitUtils.ConvertFromInternalUnits(value, displayUnitType); }
            catch { return value; }
        }

        public static double ConvertTo(double value, DisplayUnitType displayUnitType)
        {
            try { return UnitUtils.ConvertToInternalUnits(value, displayUnitType); }
            catch { return value; }
        }
    }
}
