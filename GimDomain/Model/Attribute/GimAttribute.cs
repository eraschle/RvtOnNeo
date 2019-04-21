using System.Collections.Generic;

namespace Gim.Domain.Model.Attribute
{
    public class GimAttribute
    {
        public virtual string Name { get; set; }

        public virtual GimAttributeDefinition Definition { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GimAttribute attribute &&
                   Name == attribute.Name &&
                   EqualityComparer<GimAttributeDefinition>.Default.Equals(Definition, attribute.Definition);
        }

        public override int GetHashCode()
        {
            var hashCode = 762058934;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<GimAttributeDefinition>.Default.GetHashCode(Definition);
            return hashCode;
        }
    }
}
