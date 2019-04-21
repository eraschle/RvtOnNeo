namespace Gim.Revit.Documentation.Model
{
    public class Category
    {
        public virtual string Name { get; set; }

        public virtual Category Parent { get; set; }
    }
}
