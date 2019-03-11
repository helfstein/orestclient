using Newtonsoft.Json;

namespace ORestClient.Samples.ODataModels {
    public class Estatistica {

        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("tipo")]
        public string Tipo { get; set; }
        [JsonProperty("titulo")]
        public string Titulo { get; set; }
        [JsonProperty("alias")]
        public string Alias { get; set; }
        [JsonProperty("sigla")]
        public string Sigla { get; set; }
        [JsonProperty("catId")]
        public long CatId { get; set; }
        [JsonProperty("catTitle")]
        public string CatTitle { get; set; }
        [JsonProperty("parentCatId")]
        public long ParentCatId { get; set; }
        [JsonProperty("parentCatTitle")]
        public string ParentCatTitle { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
       

    }
}