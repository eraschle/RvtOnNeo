using System;
using System.Collections.Generic;
using System.IO;
using Gim.Revit.Wrapper;
using Rvt = Autodesk.Revit.DB;

namespace Gim.Revit.Documentation.Model
{
    public class FamilyAdapter : Family
    {
        private readonly Rvt.Family family;
        public FamilyAdapter(Rvt.Family rvtFamily)
        {
            family = rvtFamily;
        }

        public override string Name
        {
            get
            {
                var path = family.Document.PathName;
                return Path.GetFileNameWithoutExtension(path);
            }
        }

        public override Guid UniqueId
        {
            get
            {
                try { return new Guid(family.UniqueId); }
                catch (ArgumentNullException) { return new Guid(); }
                catch (FormatException) { return Guid.Empty; }
            }
        }

        public override Category Category
        {
            get
            {
                var category = family.FamilyCategory;
                return new CategoryAdapter(category);
            }
        }

        public override bool IsWorkPlaneBased
        {
            get
            {
                var bip = Rvt.BuiltInParameter.FAMILY_WORK_PLANE_BASED;
                return ConvertToBool(bip);
            }
        }

        public override string Host
        {
            get
            {
                var placement = family.FamilyPlacementType;
                return placement.ToString();
            }
        }

        public override bool RoomCalculationPoint
        {
            get
            {
                var bip = Rvt.BuiltInParameter.ROOM_CALCULATION_POINT;
                return ConvertToBool(bip);
            }
        }

        public override bool SharedFamily
        {
            get
            {
                var bip = Rvt.BuiltInParameter.FAMILY_SHARED;
                return ConvertToBool(bip);
            }
        }

        public override bool AlwaysVertical
        {
            get
            {
                var bip = Rvt.BuiltInParameter.FAMILY_ALWAYS_VERTICAL;
                return ConvertToBool(bip);
            }
        }

        public override string DimensionRound
        {
            get
            {
                var bip = Rvt.BuiltInParameter.FAMILY_CONTENT_PART_TYPE;
                return family.get_Parameter(bip).AsValueString();
            }
        }

        public override string FittingType
        {
            get
            {
                var bip = Rvt.BuiltInParameter.FAMILY_CONTENT_PART_TYPE;
                return family.get_Parameter(bip).AsValueString();
            }
        }

        private bool ConvertToBool(Rvt.BuiltInParameter bip)
        {
            var parameter = family.get_Parameter(bip);
            return Convert.ToBoolean(parameter.AsInteger());
        }

        private IList<FamilyParameter> parameters = new List<FamilyParameter>();
        public override IList<FamilyParameter> Parameters
        {
            get
            {
                if (parameters is null || parameters.Count == 0)
                {
                    parameters = BuildParameters();
                }
                return parameters;
            }
        }

        private IList<FamilyParameter> BuildParameters()
        {
            var parameters = new List<FamilyParameter>();
            foreach (Rvt.FamilyParameter famParameter in GetRevitParameters())
            {
                var parameter = new FamilyParameterAdapter(famParameter);
                parameters.Add(parameter);
            }
            return parameters;
        }

        private Rvt.FamilyParameterSet GetRevitParameters()
        {
            return family.Document.FamilyManager.Parameters;
        }

        private IList<FamilyType> familyTypes = new List<FamilyType>();
        public override IList<FamilyType> FamilyTypes
        {
            get
            {
                if (familyTypes is null || familyTypes.Count == 0)
                {
                    familyTypes = BuildFamilyTypes();
                }
                return familyTypes;
            }
        }

        private IList<FamilyType> BuildFamilyTypes()
        {
            var familyTypes = new List<FamilyType>();
            var familyManager = family.Document.FamilyManager;
            var rvtFamilyTypes = familyManager.Types;
            foreach (Rvt.FamilyType familyType in rvtFamilyTypes)
            {
                familyManager.CurrentType = familyType;
                var familyTypeAdapter = new FamilyTypeAdapter(familyType);
                foreach (Rvt.FamilyParameter parameter in GetRevitParameters())
                {
                    var wrapper = new FamilyParameterValueWrapper(familyManager, parameter);
                    familyTypeAdapter.AddParameter(parameter, wrapper.StringValue);
                }
                familyTypes.Add(familyTypeAdapter);
            }
            return familyTypes;
        }
    }
}
