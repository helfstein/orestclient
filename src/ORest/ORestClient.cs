using System;
using System.Net.Http;
using ORest.Interfaces;

namespace ORest {
    //=============================================================================================
    public class ORestClient : IORestClient {

        #region Variables
        //-----------------------------------------------------------------------------------------
        protected readonly HttpClient _client;
        //-----------------------------------------------------------------------------------------
        protected readonly IORestClientSettings _settings;
        //-----------------------------------------------------------------------------------------
        #endregion

        #region Constructor
        //-----------------------------------------------------------------------------------------
        public ORestClient(IORestClientSettings settings) {
            var url = settings.BaseUrl;
            _client = new HttpClient {
                BaseAddress = new Uri(url)
            };
            if (settings.Timeout.TotalMilliseconds > 0) {
                _client.Timeout = settings.Timeout;
            }
            
            _settings = settings;
        }
        //-----------------------------------------------------------------------------------------
        #endregion

        #region Interface Implementation
        //-----------------------------------------------------------------------------------------
        public IORestClientSettings Settings => _settings;
        //-----------------------------------------------------------------------------------------
        public virtual IClientRequest<T> For<T>(string entity = null) where T : class {
            entity = entity ?? typeof(T).Name;
            return new ClientRequest<T>(_client, _settings, entity);
        }
        //-----------------------------------------------------------------------------------------
        public IUnboundClientRequest<T> Unbound<T>() where T : class {
            return new UnboundClientRequest<T>(_client, _settings);
        }
        //-----------------------------------------------------------------------------------------
        public IUnboundClientRequest Unbound() {
            return new UnboundClientRequest(_client, _settings);
        }
        //-----------------------------------------------------------------------------------------
        #endregion
        
    }
    //=============================================================================================
}