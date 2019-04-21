namespace Gim.Domain.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class EnvVar
    {
        public static string ByName(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }

        public static List<string> ByNames(List<string> names)
        {
            var variables = new List<string>();
            foreach (var name in names)
            {
                variables.Add(ByName(name));
            }

            return variables;
        }

        public static IDictionary GetAll()
        {
            return Environment.GetEnvironmentVariables();
        }
    }
}