namespace Gim.Revit.Documentation.Model
{
    public class FamilyParameter : Parameter
    {
        public bool IsFamily { get { return IsRevitParameter == false && IsSharedGuid == false; } }

        public virtual bool IsInstanceParameter { get; set; }

        public virtual string Formula { get; set; }
    }
}
