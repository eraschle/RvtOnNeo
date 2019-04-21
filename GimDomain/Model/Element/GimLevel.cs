using System;

namespace Gim.Domain.Model.Element
{
    public class GimLevel
    {
        public virtual Guid GimId { get; protected set; }

        public virtual string Name { get; protected set; }

        public int Elevation { get; set; }
    }
}
