using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using ORestClient.Models;

namespace ORestClient {
    //=============================================================================================
    public class ORestRequestException : HttpRequestException {
        
        #region Constructor
        //-----------------------------------------------------------------------------------------
        public ORestRequestException(string message, HttpStatusCode statusCode, Exception innerException = null)
            : base(message, innerException) {
            Code = statusCode;
        }
        //-----------------------------------------------------------------------------------------
        public ORestRequestException(string message, Exception innerException = null)
            : base(message, innerException) {
        }
        //-----------------------------------------------------------------------------------------
        #endregion

        #region Properties
        //-----------------------------------------------------------------------------------------
        public HttpStatusCode? Code { get; set; }
        //-----------------------------------------------------------------------------------------
        [JsonProperty("error")]
        public ORestError Error { get; set; } 
        //-----------------------------------------------------------------------------------------
        #endregion

    }
    //=============================================================================================
}
