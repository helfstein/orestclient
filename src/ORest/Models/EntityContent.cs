﻿using Newtonsoft.Json;

namespace ORest.Models {
    //=============================================================================================
    public class EntityContent<T> where T : class {

        #region Properties
        //-----------------------------------------------------------------------------------------
        [JsonProperty("results")]
        public T Results { get; set; }
        //-----------------------------------------------------------------------------------------
        #endregion

    }
    //=============================================================================================
}