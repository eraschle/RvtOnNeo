namespace GraphOnSharp.NeoForJ
{
    public interface INeoKeyModelContext<TModel, TKey> : INeoModelContext<TModel>
        where TModel : class where TKey : class
    {
        TModel ByKey(TKey key);

        TModel ByKey(TModel model);

        bool Save(TModel model);
    }
}
