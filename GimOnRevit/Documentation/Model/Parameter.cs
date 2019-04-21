using System;

namespace Gim.Revit.Documentation.Model
{
    public class Parameter
    {
        public virtual string Name { get; set; }

        public virtual string ParameterKind { get; set; }

        public virtual string Unit { get; set; }

        public virtual Guid SharedGuid { get; set; } = Guid.Empty;

        public virtual bool IsSharedGuid { get { return Guid.Empty.Equals(SharedGuid) == false; } }

        public virtual string RevitParameter { get; set; }

        public virtual bool IsRevitParameter { get { return RevitParameter != null; } }
    }
}
