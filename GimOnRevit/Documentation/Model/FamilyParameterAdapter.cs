using System;
using Autodesk.Revit.DB;
using Rvt = Autodesk.Revit.DB;

namespace Gim.Revit.Documentation.Model
{
    class FamilyParameterAdapter : FamilyParameter
    {
        private readonly Rvt.FamilyParameter familyParameter;
        private Definition definition { get { return familyParameter.Definition; } }

        public FamilyParameterAdapter(Rvt.FamilyParameter parameter)
        {
            familyParameter = parameter;
        }

        public override string Name
        {
            get { return definition.Name; }
        }

        private string defaultParameterKind
        {
            get
            {
                var defaultType = ParameterType.Invalid;
                try
                {
                    return LabelUtils.GetLabelFor(defaultType);
                }
                catch (Exception)
                {
                    return defaultType.ToString();
                }
            }
        }


        public override string ParameterKind
        {
            get
            {
                try
                {
                    var parameterType = definition.ParameterType;
                    return LabelUtils.GetLabelFor(parameterType);
                }
                catch (Exception)
                {
                    return defaultParameterKind;
                }
            }
        }

        private string defaultUnitType
        {
            get
            {
                var defaultType = DisplayUnitType.DUT_UNDEFINED;
                try
                {
                    return LabelUtils.GetLabelFor(defaultType);
                }
                catch (Exception)
                {
                    return defaultType.ToString();
                }
            }
        }

        public override bool IsInstanceParameter
        {
            get
            {
                return familyParameter.IsInstance;
            }
        }


        public override string Unit
        {
            get
            {
                try
                {
                    var displayUnit = familyParameter.DisplayUnitType;
                    return LabelUtils.GetLabelFor(displayUnit);
                }
                catch (Exception)
                {
                    return defaultUnitType;
                }
            }
        }

        public override Guid SharedGuid
        {
            get
            {
                try { return familyParameter.GUID; }
                catch { return Guid.Empty; }
            }
        }

        private BuiltInParameter defaultRevitBip
        {
            get { return BuiltInParameter.INVALID; }
        }

        public override string RevitParameter
        {
            get
            {
                var rvtBip = defaultRevitBip;
                if (definition is InternalDefinition internalDefinition)
                {
                    rvtBip = internalDefinition.BuiltInParameter;
                }
                return rvtBip.ToString(); ;
            }
        }

        public override string Formula
        {
            get { return familyParameter.Formula; }
        }

        public override bool IsRevitParameter
        {
            get
            {
                var defaultString = defaultRevitBip.ToString();
                return RevitParameter.Equals(defaultString) == false;
            }
        }
    }
}
