namespace Gim.Domain.Model.Attribute
{
    using System;
    using System.Collections.Generic;

    public class GimAttributeDefinition
    {
        public virtual string DefinitionType { get; set; }

        public virtual Type ValueType { get; protected set; }

        public override bool Equals(object obj)
        {
            return obj is GimAttributeDefinition definition &&
                   DefinitionType == definition.DefinitionType;
        }

        public override int GetHashCode()
        {
            return 1276114056 + EqualityComparer<string>.Default.GetHashCode(DefinitionType);
        }
    }
}
