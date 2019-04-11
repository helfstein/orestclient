using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ORest.Interfaces;
using ORest.Models;

namespace ORest {
    public class ClientRequestBase {

        protected readonly HttpClient _client;
        
        protected readonly IORestClientSettings _settings;

        public ClientRequestBase(HttpClient client, IORestClientSettings settings) {
            _client = client;
            _settings = settings;
        }

        protected R DeserializeObject<R>(string data) where R : class {
            R ret = null;
            switch (_settings.ImplementationType) {
                case ODataImplementation.ODataV4:
                    var resV4 = JsonConvert.DeserializeObject<R>(data);
                    ret = resV4;
                    break;
                case ODataImplementation.SapGateway:
                    var resSap = JsonConvert.DeserializeObject<Entity<R>>(data);
                    ret = resSap.Content;
                    break;
            }

            return ret;
        }

        //-----------------------------------------------------------------------------------------
        protected IEnumerable<R> DeserializeList<R>(string data) where R : class {
            IEnumerable<R> ret = null;
            var serializeSettings = new JsonSerializerSettings();
            serializeSettings.DateParseHandling = _settings.DateParseHandling ?? serializeSettings.DateParseHandling;
            switch (_settings.ImplementationType) {
                case ODataImplementation.ODataV4:
                    var resV4 = JsonConvert.DeserializeObject<V4ListEntity<R>>(data, serializeSettings);
                    ret = resV4.Value;
                    break;
                case ODataImplementation.SapGateway:
                    
                    var resSap = JsonConvert.DeserializeObject<ListEntity<IEnumerable<R>>>(data, serializeSettings);
                    ret = resSap.Content.Results;
                    break;
            }

            return ret;
        }

        //-----------------------------------------------------------------------------------------
        protected string MakeUrl(string source) {
            return source.Replace(" ", "%20").Replace("'", "%27");
        }

        protected async Task<HttpRequestMessage> SetHeaders(HttpMethod method, string path) {
            var url = $"{_client.BaseAddress.AbsoluteUri}{path}";
            var request = new HttpRequestMessage(method, path);

            if (_settings.UseBasicAuth && request.Headers.Authorization == null) {
                var byteArray = Encoding.ASCII.GetBytes($"{_settings.Username}:{_settings.Password}");
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }

            switch (method.Method.ToUpper()) {
                case "DELETE":
                case "PUT":
                case "PATCH":
                case "POST":
                if (_settings.UseXsrf) {
                    request = await SetXCSRFToken(request);
                }

                break;
                default:
                if (!url.Contains("$format")) {
                    url = url.Contains("?")
                        ? $"{url}&$format=json"
                        : $"{url}?$format=json";
                }

                request.RequestUri = new Uri(url);
                break;
            }

            return request;

        }

        //-----------------------------------------------------------------------------------------
        protected async Task<HttpRequestMessage> SetXCSRFToken(HttpRequestMessage request) {
            try {
                if (!string.IsNullOrWhiteSpace(_settings.XsrfToken)) {
                    request.Headers.Add("X-CSRF-Token", _settings.XsrfToken);
                    return request;
                }
                var metaUrl = $"{_settings.BaseUrl}?$format=json";
                var req = new HttpRequestMessage(HttpMethod.Get, metaUrl);
                req.Headers.Authorization = request.Headers.Authorization;
                req.Headers.Add("X-CSRF-Token", "Fetch");

                if (_settings.BeforeRequestAsync != null) {
                    request = await _settings.BeforeRequestAsync.Invoke(request);
                }

                var response = await _client.SendAsync(req);
                _settings.TraceRequest?.Invoke(req);
                _settings.AfterResponse?.Invoke(response);
                if (response.IsSuccessStatusCode) {
                    response.Headers.TryGetValues("X-CSRF-Token", out var token);
                    request.Headers.Add("X-CSRF-Token", token.FirstOrDefault());
                    _settings.XsrfToken = token.FirstOrDefault();
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw new ORestRequestException(e.Message, e);
            }

            return request;
        }


    }
}