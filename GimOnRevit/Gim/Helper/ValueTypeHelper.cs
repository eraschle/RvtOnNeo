using System;
using Autodesk.Revit.DB;

namespace Gim.Revit.Gim.Helper
{
    class ValueTypeHelper
    {
        internal static Type Get(StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Integer:
                    return typeof(int);
                case StorageType.Double:
                    return typeof(double);
                case StorageType.ElementId:
                    return typeof(int);
                case StorageType.String:
                default:
                    return typeof(string);
            }
        }

        internal static Type Get(Parameter parameter)
        {
            var parameterType = parameter.Definition.ParameterType;
            return parameterType != ParameterType.YesNo
                ? typeof(bool) : Get(parameter.StorageType);
        }
    }
}
