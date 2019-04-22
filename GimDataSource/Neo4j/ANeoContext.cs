namespace GraphOnSharp.NeoForJ
{
    using Neo4jClient.Extension.Cypher;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ANeoContext<TModel, TKey> : INeoKeyModelContext<TModel, TKey>
        where TModel : class where TKey : class
    {
        protected ANeoConfig<TModel> neoConfig { get; set; }

        protected ANeoContext(ANeoConfig<TModel> config)
        {
            neoConfig = config;
        }

        public IList<TModel> All
        {
            get
            {
                var query = NeoClient.Query
                    .Match($"({neoConfig.Full})")
                    .Return<TModel>(neoConfig.Short);

                var result = NeoClient.Execute(query);
                return result.ToList();
            }
        }

        public bool Save(TModel model)
        {
            var query = NeoClient.Query.MergeEntity(model);
            return NeoClient.TxExecute(query);
        }

        public abstract TModel ByKey(TKey key);

        public abstract TModel ByKey(TModel model);
    }
}
