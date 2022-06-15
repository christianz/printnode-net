using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PrintNodeNet
{
    internal sealed class PrintNodeChildAccountCreationResponseConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var childAccount = new PrintNodeChildAccount();

            serializer.Populate(jObject["Account"].CreateReader(), childAccount);

            return childAccount;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof (PrintNodeChildAccount));
        }
    }
}
