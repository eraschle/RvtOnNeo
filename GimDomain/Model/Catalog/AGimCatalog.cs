namespace Gim.Domain.Model.Catalog
{
    public abstract class AGimCatalog
    {
        public virtual string Name { get; set; }
        public virtual AGimCatalog Parent { get; set; }
    }
}
