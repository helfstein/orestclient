using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ORest.Interfaces;

namespace ORest {
    //=============================================================================================
    public class UnboundClientRequest<T> : ClientRequestBase, IUnboundClientRequest<T>,
        IExecutableClientRequest<T> where T : class {
        
        private readonly string _path;
        private string callPath;
        private HttpMethod _executionMethod = HttpMethod.Get;
        private object _data;


        public UnboundClientRequest(HttpClient client, IORestClientSettings settings) : base(client, settings) {
            
        }

        public UnboundClientRequest(HttpClient client, IORestClientSettings settings, string path) : base(client, settings) {
            _path = path;
        }



        public IExecutableClientRequest<T> Function(string path) {
            if (string.IsNullOrWhiteSpace(path)) {
                throw new ArgumentNullException($"path is required.");
            }
            callPath = path;
            return this;
        }

        public IExecutableClientRequest<T> Function(string functionName, Dictionary<string, string> parameters) {
            if (string.IsNullOrWhiteSpace(functionName)) {
                throw new ArgumentNullException($"functionName is required.");
            }
            if (parameters == null || parameters.Keys.Count == 0) {
                throw new ArgumentNullException($"parameters is required in this method.");
            }
            _executionMethod = HttpMethod.Get;
            var pars = new List<string>();

            foreach (var key in parameters.Keys) {
                pars.Add($"{key}={parameters[key]}");
            }

            var funcParams = string.Join("&", pars);
            callPath = _settings.ImplementationType == ODataImplementation.SapGateway 
                ? $"{functionName}?{funcParams}" 
                : $"{functionName}({funcParams})";
            
            return this;
        }



        public IExecutableClientRequest<T> Action(string actionName, object data) {
            _data = data;
            _executionMethod = HttpMethod.Post;
            return Action(actionName);
        }

        public IExecutableClientRequest<T> UseMethod(Method method) {
            _executionMethod = method == Method.Get ? HttpMethod.Get : HttpMethod.Post;
            return this;
        }

        public IExecutableClientRequest<T> Action(string actionName) {
            if (string.IsNullOrWhiteSpace(actionName)) {
                throw new ArgumentNullException($"actionName is required.");
            }
            _executionMethod = HttpMethod.Post;
            callPath = actionName;
            return this;
        }
        

        public async Task<T> ExecuteAsSingleAsync() {
            T thing;
            try {
                var requestPath = !string.IsNullOrWhiteSpace(_path)
                    ? $"{_path}{callPath}"
                    : callPath;
                requestPath = MakeUrl(requestPath);
                var request = await SetHeaders(_executionMethod, requestPath);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (_data != null) {
                    var tmp = JsonConvert.SerializeObject(_data, Formatting.None, new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Content = new StringContent(tmp, Encoding.UTF8, "application/json");
                }
                if (_settings.BeforeRequestAsync != null) {
                    request = await _settings.BeforeRequestAsync.Invoke(request);
                }

                var response = await _client.SendAsync(request);
                _settings.TraceRequest?.Invoke(request);
                _settings.AfterResponse?.Invoke(response);
                if (response.IsSuccessStatusCode) {
                    var content = await response.Content.ReadAsStringAsync();
                    thing = DeserializeObject<T>(content);
                }
                else {
                    try {
                        var content = await response.Content.ReadAsStringAsync();
                        var excp = JsonConvert.DeserializeObject<ORestRequestException>(content);
                        throw excp;
                    }
                    catch (Exception) {
                        //Console.WriteLine(e);
                        throw new ORestRequestException(await response.Content.ReadAsStringAsync(), response.StatusCode);
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw new ORestRequestException(e.Message, e);
            }
            return thing;
        }

        public async Task<IEnumerable<T>> ExecuteAsEnumerableAsync() {
            IEnumerable<T> thing;
            try {
                var requestPath = !string.IsNullOrWhiteSpace(_path)
                    ? $"{_path}{callPath}"
                    : callPath;
                requestPath = MakeUrl(requestPath);
                var request = await SetHeaders(_executionMethod, requestPath);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (_data != null) {
                    var tmp = JsonConvert.SerializeObject(_data, Formatting.None, new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Content = new StringContent(tmp, Encoding.UTF8, "application/json");
                }
                if (_settings.BeforeRequestAsync != null) {
                    request = await _settings.BeforeRequestAsync.Invoke(request);
                }

                var response = await _client.SendAsync(request);
                _settings.TraceRequest?.Invoke(request);
                _settings.AfterResponse?.Invoke(response);
                if (response.IsSuccessStatusCode) {
                    var content = await response.Content.ReadAsStringAsync();
                    thing = DeserializeList<T>(content);
                }
                else {
                    try {
                        var content = await response.Content.ReadAsStringAsync();
                        var excp = JsonConvert.DeserializeObject<ORestRequestException>(content);
                        throw excp;
                    }
                    catch (Exception) {
                        //Console.WriteLine(e);
                        throw new ORestRequestException(await response.Content.ReadAsStringAsync(), response.StatusCode);
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw new ORestRequestException(e.Message, e);
            }
            return thing;
        }

        public async Task ExecuteAsync() {
            try {
                var requestPath = !string.IsNullOrWhiteSpace(_path)
                    ? $"{_path}{callPath}"
                    : callPath;
                requestPath = MakeUrl(requestPath);
                var request = await SetHeaders(_executionMethod, requestPath);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (_data != null) {
                    var tmp = JsonConvert.SerializeObject(_data, Formatting.None, new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Content = new StringContent(tmp, Encoding.UTF8, "application/json");
                }
                if (_settings.BeforeRequestAsync != null) {
                    request = await _settings.BeforeRequestAsync.Invoke(request);
                }

                var response = await _client.SendAsync(request);
                _settings.TraceRequest?.Invoke(request);
                _settings.AfterResponse?.Invoke(response);
                if (!response.IsSuccessStatusCode) {
                    try {
                        var content = await response.Content.ReadAsStringAsync();
                        var excp = JsonConvert.DeserializeObject<ORestRequestException>(content);
                        throw excp;
                    }
                    catch (Exception) {
                        //Console.WriteLine(e);
                        throw new ORestRequestException(await response.Content.ReadAsStringAsync(), response.StatusCode);
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw new ORestRequestException(e.Message, e);
            }
        }

    }
    //=============================================================================================
    public class UnboundClientRequest : ClientRequestBase, IUnboundClientRequest, IExecutableClientRequest {

        private readonly string _path;
        private string callPath;
        private HttpMethod _executionMethod = HttpMethod.Get;
        private object _data;

        public UnboundClientRequest(HttpClient client, IORestClientSettings settings) : base(client, settings) {
            
        }

        public UnboundClientRequest(HttpClient client, IORestClientSettings settings, string path) : base(client, settings) {
            _path = path;
        }
        
        public IExecutableClientRequest Function(string path) {
            if (string.IsNullOrWhiteSpace(path)) {
                throw new ArgumentNullException($"path is required.");
            }
            callPath = path;
            _executionMethod = HttpMethod.Get;
            return this;
        }

        public IExecutableClientRequest Function(string functionName, Dictionary<string, string> parameters) {
            if (string.IsNullOrWhiteSpace(functionName)) {
                throw new ArgumentNullException($"functionName is required.");
            }
            if (parameters == null || parameters.Keys.Count == 0) {
                throw new ArgumentNullException($"parameters is required in this method.");
            }
            _executionMethod = HttpMethod.Get;
            var pars = new List<string>();

            foreach (var key in parameters.Keys) {
                pars.Add($"{key}={parameters[key]}");
            }

            var funcParams = string.Join("&", pars);

            callPath = _settings.ImplementationType == ODataImplementation.SapGateway
                ? $"{functionName}?{funcParams}"
                : $"{functionName}({funcParams})";
            return this;
        }

        public IExecutableClientRequest Action(string actionName) {
            if (string.IsNullOrWhiteSpace(actionName)) {
                throw new ArgumentNullException($"actionName is required.");
            }
            _executionMethod = HttpMethod.Post;
            callPath = actionName;
            return this;
        }

        public IExecutableClientRequest Action(string actionName, object data) {
            _data = data;
            return Action(actionName);
        }

        public IExecutableClientRequest UseMethod(Method method) {
            _executionMethod = method == Method.Get ? HttpMethod.Get : HttpMethod.Post;
            return this;
        }

        public async Task ExecuteAsync() {
            try {
                var requestPath = !string.IsNullOrWhiteSpace(_path)
                    ? $"{_path}{callPath}"
                    : callPath;
                requestPath = MakeUrl(requestPath);
                var request = await SetHeaders(_executionMethod, requestPath);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (_data != null) {
                    var tmp = JsonConvert.SerializeObject(_data, Formatting.None, new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Content = new StringContent(tmp, Encoding.UTF8, "application/json");
                }

                if (_settings.BeforeRequestAsync != null) {
                    request = await _settings.BeforeRequestAsync.Invoke(request);
                }

                var response = await _client.SendAsync(request);
                _settings.TraceRequest?.Invoke(request);
                _settings.AfterResponse?.Invoke(response);
                if (!response.IsSuccessStatusCode) {
                    try {
                        var content = await response.Content.ReadAsStringAsync();
                        var excp = JsonConvert.DeserializeObject<ORestRequestException>(content);
                        throw excp;
                    }
                    catch (Exception) {
                        //Console.WriteLine(e);
                        throw new ORestRequestException(await response.Content.ReadAsStringAsync(), response.StatusCode);
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw new ORestRequestException(e.Message, e);
            }
        }

    }    
    //=============================================================================================
}