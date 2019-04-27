using System.Collections.Generic;

namespace Gim.Revit.Documentation.Model
{
    public class Category
    {
        public virtual int RevitId { get; set; }
        public virtual string Name { get; set; }

        public virtual Category Parent { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Category category &&
                   Name == category.Name &&
                   EqualityComparer<Category>.Default.Equals(Parent, category.Parent);
        }

        public override int GetHashCode()
        {
            var hashCode = -512206829;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<Category>.Default.GetHashCode(Parent);
            return hashCode;
        }
    }
}
