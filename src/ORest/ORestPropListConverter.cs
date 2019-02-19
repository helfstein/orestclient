using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ORest {
    public class ORestPropListConverter : JsonConverter {

        public string Path { get;}
        public bool RegisterErrors { get;}
        
        public ORestPropListConverter(string path) {
            Path = path;
        }

        public ORestPropListConverter(string path, bool registerErrors) : this(path) {
            RegisterErrors = registerErrors;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            // throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            try {
                var jo = JObject.Load(reader);
                //var targetObj = Activator.CreateInstance(objectType);
                //var isList = objectType.DeclaringType == typeof(IEnumerable<>);
                //var jsonPath = !string.IsNullOrWhiteSpace(Path) ? Path : prop.Name;
                var token = jo.SelectToken(Path);
                if (token != null && token.Type != JTokenType.Null) {
                    var value = token.ToObject(objectType, serializer);
                    return value;
                }
            }
            catch (Exception e) {
                if (RegisterErrors) {
                    Console.WriteLine(e);
                }
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