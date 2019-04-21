namespace Gim.Revit.Documentation.Json
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public abstract class AGenericJsonConverter : JsonConverter
    {
        public override bool CanRead { get { return false; } }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (CanConvert(value.GetType()))
            {
                var token = JToken.FromObject(value);
                var wrapper = new JObject
                {
                   { PropertyName, token }
                };
                wrapper.WriteTo(writer);
            }
            else
            {
                WriteJson(writer, value, serializer);
            }
        }

        protected abstract string PropertyName { get; }
    }
}
