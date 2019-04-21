namespace Gim.Revit.Documentation.Model
{
    using System;
    using System.Collections.Generic;

    public class Family
    {
        public virtual Guid UniqueId { get; set; }

        public virtual string Name { get; set; }

        public virtual Category Category { get; set; }

        public virtual bool IsWorkPlaneBased { get; set; }

        public virtual string Host { get; set; }

        public virtual bool RoomCalculationPoint { get; set; }

        public virtual bool SharedFamily { get; set; }

        public virtual bool AlwaysVertical { get; set; }

        public virtual string FittingType { get; set; }

        public virtual string DimensionRound { get; set; }

        public virtual IList<FamilyParameter> Parameters { get; set; }

        public virtual IList<FamilyType> FamilyTypes { get; set; }
    }
}
