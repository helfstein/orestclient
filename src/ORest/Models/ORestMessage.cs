﻿using Newtonsoft.Json;

namespace ORest.Models {
    //=============================================================================================
    public class ORestMessage {

        #region Properties
        //-----------------------------------------------------------------------------------------
        [JsonProperty("lang")]
        public string Lang { get; set; }
        //-----------------------------------------------------------------------------------------
        [JsonProperty("value")]
        public string Value { get; set; }
        //-----------------------------------------------------------------------------------------
        #endregion

    }
    //=============================================================================================
}