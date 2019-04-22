namespace GraphOnSharp.NeoForJ
{
    using log4net;
    using Neo4jClient;
    using Neo4jClient.Cypher;
    using Neo4jClient.Extension.Cypher;
    using Neo4jClient.Transactions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class NeoClient
    {
        private static Uri Host { get; set; } = new Uri("http://localhost:7474/db/data");
        private static NetworkCredential Credential { get; set; } = new NetworkCredential("neo4j", "holdrio79");

        private static readonly ILog logger = LogManager.GetLogger(nameof(Neo4jClient));


        private static IGraphClient _Client;
        public static IGraphClient Client
        {
            get
            {
                if (_Client is null)
                {
                    _Client = new GraphClient(Host, Credential.UserName, Credential.Password);
                }
                if (_Client.IsConnected == false)
                {
                    _Client.Connect();
                }
                return _Client;
            }
        }

        public static ITransactionalGraphClient TxClient
        {
            get { return (ITransactionalGraphClient)Client; }
        }

        public static ICypherFluentQuery Query
        {
            get { return Client.Cypher; }
        }

        public static IEnumerable<TNode> Execute<TNode>(ICypherFluentQuery<TNode> query)
        {
            logger.Debug("Executed Query:");
            logger.Debug(query.GetFormattedDebugText());

            var result = query.Results.ToList();
            logger.Debug($"Result: Anzahl {result.Count()}");

            return result;
        }

        public static bool TxExecute(ICypherFluentQuery query)
        {
            var txClient = TxClient;
            var queryText = query.GetFormattedDebugText();

            using (var tx = txClient.BeginTransaction())
            {
                try
                {
                    logger.Debug("Executed Query under Transaction:");
                    logger.Debug(queryText);
                    query.ExecuteWithoutResults();
                    tx.Commit();

                    return true;
                }
                catch (Exception e)
                {
                    logger.Debug(
                        "Query:\n"
                        + $"{queryText}"
                        + "Stack:\n"
                        + $"{e.StackTrace}\n"
                        + "Message:\n"
                        + $"{e.Message}");

                    return false;
                }
            }
        }

        public void CleanUp()
        {
            var query = Query.Match("(n)").DetachDelete("n");
            TxExecute(query);
        }

        //public void test()
        //{
        //    var build = new ContainerBuilder();
        //    build.Register<IGraphClient>(context =>
        //    {
        //        var graphClient = new GraphClient(new Uri("http://localhost:7474/db/data"));
        //        graphClient.Connect();
        //        return graphClient;
        //    }).SingleInstance();
        //}

    }
}
