namespace GraphOnSharp.NeoForJ
{
    using GraphOnSharp.Helper;
    using System.Collections.Generic;
    using System.Text;

    public class NeoHelper
    {
        static readonly char separator = ':';

        public static string GetLabels(IList<string> labels)
        {
            var neoLabels = new StringBuilder();
            for (var idx = 0; idx < labels.Count; idx++)
            {
                neoLabels.Append(labels[idx]);
                if (idx < labels.Count - 1)
                {
                    neoLabels.Append(separator);
                }
            }
            return neoLabels.ToString();
        }

        public static IList<string> GetLabels(string labels)
        {
            return labels.Split(separator);
        }

        public static bool SetupUniqueConstraint(INeoConfig config, string uniqueName)
        {
            var property = $"{config.Short}.{uniqueName}";
            var query = NeoClient.Query.CreateUniqueConstraint(config.Full, property);
            return NeoClient.TxExecute(query);
        }

        public static TModel Synchronize<TModel>(TModel model, TModel existing)
        {
            var fields = ClassHelper.GetFields(model);
            foreach (var field in fields)
            {
                var modelValue = ClassHelper.GetValue(model, field);
                if (modelValue is null)
                {
                    modelValue = ClassHelper.GetValue(existing, field);
                }
                else if (modelValue is ISet<object> setValue)
                {
                    var existingValue = ClassHelper.GetValue<HashSet<object>>(existing, field);
                    setValue.UnionWith(existingValue);
                    modelValue = setValue;
                }
                ClassHelper.SetValue(model, field, modelValue);
            }
            return model;
        }
    }
}
