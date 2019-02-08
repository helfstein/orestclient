using System;
using System.Threading;
using System.Threading.Tasks;
using ORestClient.Interfaces;

namespace ORestClient {
    class Session : ISession {
        //private IODataAdapter _adapter;
        //private HttpConnection _httpConnection;
        //private EdmMetadataCache _metadataCache;

        private Session(string baseUri) : this(new ORestClientSettings {
            BaseUrl = baseUri
        }) {
        }

        private Session(IORestClientSettings settings) {
            if (settings.BaseUrl == null || string.IsNullOrEmpty(settings.BaseUrl)) {
                throw new InvalidOperationException("Unable to create client session with no URI specified.");
            }

            Settings = settings;

            //if (!string.IsNullOrEmpty(Settings.MetadataDocument)) {
            //    // Create as early as possible as most unit tests require this and also makes it simpler when assigning a static document
            //    _metadataCache = InitializeStaticMetadata(Settings.MetadataDocument);
            //}
        }

     

        public IORestClientSettings Settings { get; }

        public ITypeCache TypeCache => TypeCaches.TypeCache(Settings.BaseUrl);
        public Uri BaseUri => new Uri(Settings.BaseUrl);

        public void Dispose() {
            lock (this) {
                //if (_httpConnection != null) {
                //    _httpConnection.Dispose();
                //    _httpConnection = null;
                //}
            }
        }

        public async Task Initialize(CancellationToken cancellationToken) {
            await Task.Delay(1);
        }
        
        internal static Session FromSettings(IORestClientSettings settings) {
            return new Session(settings);
        }

        


    }
}
