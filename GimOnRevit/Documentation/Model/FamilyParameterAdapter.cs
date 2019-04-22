using System;
using Autodesk.Revit.DB;
using Gim.Revit.Helper;
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

        public override string ParameterKind
        {
            get
            {
                var parameterType = definition.ParameterType;
                return LabelHelper.Get(parameterType);
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
                var displayUnit = familyParameter.DisplayUnitType;
                return LabelHelper.Get(displayUnit);
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
