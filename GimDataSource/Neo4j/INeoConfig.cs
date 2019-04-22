namespace GraphOnSharp.NeoForJ
{
    using Neo4jClient.Extension.Cypher;
    using System;

    public interface INeoConfig
    {
        string Short { get; }

        string Label { get; }

        string Full { get; }

        Type ModelType { get; }

        void Config(CypherExtensionContext config);

        void SetupConstraint();

    }
}
