﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace ORestClient.Models {
    public class V4ListEntity<T> where T : class {
        [JsonProperty("value")]
        public IEnumerable<T> Value { get; set; }
    }
}