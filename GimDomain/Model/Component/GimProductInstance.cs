using System;
using System.Collections.Generic;
using Gim.Domain.Model.Attribute;
using Gim.Domain.Model.Element;

namespace Gim.Domain.Model.Component
{
    public class GimProductInstance
    {
        public virtual Guid GimId { get; protected set; }

        public virtual string Name { get; set; }

        public virtual ISet<GimAttributeValue> Attributes { get; set; }

        public virtual GimProduct Product { get; set; }

        public virtual GimLevel Level { get; set; }
    }
}
