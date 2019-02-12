using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ORest {
    public class ORestPropListConverter : JsonConverter {

        public string Path { get;}
        
        public ORestPropListConverter(string path) {
            Path = path;
        }
        

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            // throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {

            var jo = JObject.Load(reader);
            //var targetObj = Activator.CreateInstance(objectType);
            //var isList = objectType.DeclaringType == typeof(IEnumerable<>);
            //var jsonPath = !string.IsNullOrWhiteSpace(Path) ? Path : prop.Name;

            var token = jo.SelectToken(Path);

            if (token != null && token.Type != JTokenType.Null) {
                var value = token.ToObject(objectType, serializer);
                return value;
            }

            return existingValue;
        }

        public override bool CanConvert(Type objectType) {
            // CanConvert is not called when [JsonConverter] attribute is used
            return false;
        }

        public override bool CanWrite => false;
    }
}