using System;
using Autodesk.Revit.DB;
using Gim.Domain.Model.Element;

namespace Gim.Revit.Gim.Element
{
    class GimLevelWrapper : GimLevel
    {
        private readonly Level level;
        public GimLevelWrapper(Level level)
        {
            this.level = level;
        }

        public override Guid GimId
        {
            get { return new Guid(level.UniqueId); }
            protected set { base.GimId = value; }
        }

        public override string Name
        {
            get { return level.Name; }
            protected set { level.Name = value; }
        }
    }
}
