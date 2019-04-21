namespace Gim.Revit.Documentation.Json
{
    public class FamilyJsonWarpper
    {
        public Family Family { get; set; }
        public FamilyJsonWarpper(FamilyAdapter familyAdapter)
        {
            Family = familyAdapter;
        }
    }
}
