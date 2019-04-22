namespace GraphOnSharp.NeoForJ
{
    using Neo4jClient.Extension.Cypher;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ConfigFactory
    {
        private static ConfigFactory instance = null;

        public static ConfigFactory Instance
        {
            get
            {
                if (instance is null)
                {
                    instance = new ConfigFactory();
                }
                return instance;
            }
        }


        private IDictionary<Type, INeoConfigModel> configModelMap = new Dictionary<Type, INeoConfigModel>();
        private readonly IDictionary<Type, INeoConfigRelation> configRelationMap = new Dictionary<Type, INeoConfigRelation>();

        private ConfigFactory()
        {
            configModelMap = CreateModelMap();
            configRelationMap = CreateRelationMap();
        }

        public ANeoConfig<TModel> GetConfig<TModel>(Type type) where TModel : class
        {
            var typeName = type.FullName;
            if (configModelMap.ContainsKey(type))
            {
                return configModelMap[type] as ANeoConfig<TModel>;
            }
            return null;
        }

        private IEnumerable<Type> GetConfigs(Type config)
        {
            var domain = AppDomain.CurrentDomain;
            var loadedAssemblies = domain.GetAssemblies();
            var configs = new List<Type>();
            foreach (var assembly in loadedAssemblies)
            {
                var allTypes = assembly.GetTypes();
                var configTypes = allTypes.Where(dll => config.IsAssignableFrom(dll) && !dll.IsInterface && !dll.IsAbstract);
                configs.AddRange(configTypes);
            }
            return configs;
        }

        public void Configure(CypherExtensionContext context)
        {
            Configure(context, configModelMap);
            Configure(context, configRelationMap);
        }

        private void Configure<TNeoConfig>(CypherExtensionContext context, IDictionary<Type, TNeoConfig> map)
            where TNeoConfig : INeoConfig
        {
            foreach (var type in map.Keys)
            {
                var config = map[type];
                config.Config(context);
                config.SetupConstraint();
            }
        }

        private IDictionary<Type, INeoConfigModel> CreateModelMap()
        {
            var configs = GetConfigs(typeof(INeoConfigModel));
            return CreateMap(configs, configModelMap);
        }

        private IDictionary<Type, INeoConfigRelation> CreateRelationMap()
        {
            var configs = GetConfigs(typeof(INeoConfigRelation));
            return CreateMap(configs, configRelationMap);
        }

        private IDictionary<Type, TNeoConfig> CreateMap<TNeoConfig>(
            IEnumerable<Type> types,
            IDictionary<Type, TNeoConfig> configMap)
            where TNeoConfig : class, INeoConfig
        {
            foreach (var type in types)
            {
                var obj = Activator.CreateInstance(type);
                var config = obj as TNeoConfig;
                configMap.Add(config.ModelType, config);
            }
            return configMap;
        }
    }
}
