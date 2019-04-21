using System;
using System.Collections.Generic;
using Gim.Domain.Model.Attribute;

namespace Gim.Domain.Model.Component
{
    public class GimProduct
    {
        public virtual Guid GimId { get; protected set; }

        public virtual string Name { get; set; }

        public virtual ISet<GimAttributeValue> Attributes { get; set; }
    }
}
