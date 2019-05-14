using Gim.Revit.Documentation.Model;

namespace Gim.Revit.Documentation.Wrapper
{
    public class FamilyWarpper
    {
        public Family Family { get; set; }
        public FamilyWarpper(FamilyAdapter familyAdapter)
        {
            Family = familyAdapter;
        }
    }
}
