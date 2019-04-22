namespace GraphOnSharp.NeoForJ
{
    using Neo4jClient;
    using Neo4jClient.Cypher;
    using Neo4jClient.Transactions;
    using System;
    using System.Collections.Generic;
    using System.Net;

    public interface IGraphDbClient
    {
        void Connect(Uri host, NetworkCredential credential);

        IGraphClient Client { get; }

        ITransactionalGraphClient TxClient { get; }

        ICypherFluentQuery Query { get; }

        bool TxExecute(ICypherFluentQuery query);

        IEnumerable<TModel> Execute<TModel>(ICypherFluentQuery<TModel> query);

        void CleanUp();
    }
}
