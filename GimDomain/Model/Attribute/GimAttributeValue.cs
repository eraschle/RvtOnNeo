using System.Collections.Generic;

namespace Gim.Domain.Model.Attribute
{
    public class GimAttributeValue
    {
        public virtual GimAttribute Attribute { get; set; }

        public virtual object Value { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GimAttributeValue value &&
                   EqualityComparer<GimAttribute>.Default.Equals(Attribute, value.Attribute) &&
                   EqualityComparer<object>.Default.Equals(Value, value.Value);
        }

        public override int GetHashCode()
        {
            var hashCode = -1299389615;
            hashCode = hashCode * -1521134295 + EqualityComparer<GimAttribute>.Default.GetHashCode(Attribute);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Value);
            return hashCode;
        }
    }
}
