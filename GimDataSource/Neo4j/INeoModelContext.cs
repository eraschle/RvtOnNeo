namespace GraphOnSharp.NeoForJ
{
    using System.Collections.Generic;

    public interface INeoModelContext<TModel>
    {
        IList<TModel> All { get; }
    }
}
