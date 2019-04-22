namespace GraphOnSharp.NeoForJ
{
    using Neo4jClient.Extension.Cypher;
    using System;

    public abstract class ANeoConfig<TModel> : INeoConfig
    {
        public virtual string Full { get { return string.Format("{0}:{1}", Short, FirstLabel); } }

        public virtual string Short { get; protected set; }

        public virtual string Label { get; protected set; }

        public Type ModelType { get { return typeof(TModel); } }

        protected ANeoConfig(string label, string shortName)
        {
            Short = shortName;
            Label = label;
        }

        private string FirstLabel
        {
            get
            {
                var rootLabel = Label;
                var splitted = NeoHelper.GetLabels(Label);
                if (splitted != null && splitted.Count > 0)
                {
                    rootLabel = splitted[0];
                }
                return rootLabel;
            }
        }

        public abstract void Config(CypherExtensionContext config);

        protected abstract ConfigWith<TModel> Config(ConfigWith<TModel> config);

        public abstract void SetupConstraint();
    }
}
