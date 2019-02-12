using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ORest.Expressions;
using ORest.Extensions;
using ORest.Interfaces;
using ODataExpression = ORest.Expressions.ODataExpression;

namespace ORest {
    //=============================================================================================
    public class ClientRequest<T> : ClientRequestBase, INavigatableClientRequest<T> where T : class {

        #region Variables
        //-----------------------------------------------------------------------------------------
        protected string _path;
        //-----------------------------------------------------------------------------------------
        protected string _key;
        //-----------------------------------------------------------------------------------------
        protected readonly Dictionary<string, string> queryParams;
        //-----------------------------------------------------------------------------------------
        protected readonly ITypeCache _typeCache = new TypeCache(new TypeConverter());
        //-----------------------------------------------------------------------------------------
        #endregion

        #region Constructor
        //-----------------------------------------------------------------------------------------
        public ClientRequest(HttpClient client, IORestClientSettings settings, string entry) : base(client, settings) {
            _path = entry;
            queryParams = new Dictionary<string, string>();
        }
        //-----------------------------------------------------------------------------------------
        #endregion

        #region Interface Implementation
        //-----------------------------------------------------------------------------------------
        public virtual async Task<IEnumerable<T>> FindEntriesAsync() {
            IEnumerable<T> lst;
            try {
                var query = BuildQuery();
                query = !string.IsNullOrWhiteSpace(query) ? $"?{query}" : string.Empty;
                _path = $"{_path}{query}";
                _path = MakeUrl(_path);
                var request = await SetHeaders(HttpMethod.Get, _path);
                if (_settings.BeforeRequestAsync != null) {
                    request = await _settings.BeforeRequestAsync.Invoke(request);
                }

                var response = await _client.SendAsync(request);

                _settings.TraceRequest?.Invoke(request);
                _settings.AfterResponse?.Invoke(response);

                if (response.IsSuccessStatusCode) {
                    var content = await response.Content.ReadAsStringAsync();
                    lst = DeserializeList<T>(content);
                }
                else {
                    throw new ORestRequestException(await response.Content.ReadAsStringAsync(), response.StatusCode);
                }

            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw new ORestRequestException(e.Message, e);
            }

            return lst;
        }
        //-----------------------------------------------------------------------------------------
        public virtual async Task<T> FindEntryAsync() {
            if (_key == null)
                throw new ArgumentNullException($"Key needed to be informed.");
            var query = BuildQuery();
            query = !string.IsNullOrWhiteSpace(query) ? $"?{query}" : string.Empty;
            _path = $"{_path}({_key})";
            _path = $"{_path}{query}";
            _path = MakeUrl(_path);
            T thing;
            try {
                var request = await SetHeaders(HttpMethod.Get, _path);
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
                    throw new ORestRequestException(await response.Content.ReadAsStringAsync(), response.StatusCode);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw new ORestRequestException(e.Message, e);
            }

            return thing;
        }
        //-----------------------------------------------------------------------------------------
        public virtual async Task<T> InsertEntryAsync(T entry) {
            T thing;
            try {
                var data = entry;

                var request = await SetHeaders(HttpMethod.Post, _path);
                if (data != null) {
                    var tmp = JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings {
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
                    throw new ORestRequestException(await response.Content.ReadAsStringAsync(), response.StatusCode);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw new ORestRequestException(e.Message, e);
            }

            return thing;
        }
        //-----------------------------------------------------------------------------------------
        public virtual async Task<T> UpdateEntryAsync(T entry) {
            if (_key == null)
                throw new ArgumentNullException($"Key needed to be informed.");
            _path = $"{_path}({_key})";
            T thing;
            try {
                var data = entry;
                var request = await SetHeaders(HttpMethod.Put, _path);
                //var request = await SetHeaders(_settings.ImplementationType == ODataImplementation.SapGateway
                //    ? HttpMethod.Put 
                //    : new HttpMethod("PATCH") , _path);
                if (data != null) {
                    var tmp = JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings {
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
                    throw new ORestRequestException(await response.Content.ReadAsStringAsync(), response.StatusCode);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw new ORestRequestException(e.Message, e);
            }
            return thing;
        }
        //-----------------------------------------------------------------------------------------
        public async Task<T> UpdateEntryAsync(object entry) {
            if (_key == null)
                throw new ArgumentNullException($"Key needed to be informed.");
            _path = $"{_path}({_key})";
            T thing;
            try {
                var data = entry;
                var request = await SetHeaders(HttpMethod.Put, _path);
                //var request = await SetHeaders(_settings.ImplementationType == ODataImplementation.SapGateway
                //    ? HttpMethod.Put 
                //    : new HttpMethod("PATCH") , _path);
                if (data != null) {
                    var tmp = JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings {
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
                    throw new ORestRequestException(await response.Content.ReadAsStringAsync(), response.StatusCode);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw new ORestRequestException(e.Message, e);
            }
            return thing;
        }
        //-----------------------------------------------------------------------------------------
        public virtual async Task<T> DeleteEntryAsync() {
            if (_key == null)
                throw new ArgumentNullException($"Key needed to be informed.");
            _path = $"{_path}({_key})";
            T thing;
            try {
                var request = await SetHeaders(HttpMethod.Delete, _path);


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
                    throw new ORestRequestException(await response.Content.ReadAsStringAsync(), response.StatusCode);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw new ORestRequestException(e.Message, e);
            }
            return thing;
        }
        //-----------------------------------------------------------------------------------------
        public IClientRequest<T> Skip(int number) {
            if (!queryParams.ContainsKey("$skip")) {
                queryParams.Add("$skip", number.ToString());
            }

            queryParams["$skip"] = number.ToString();
            return this;
        }

        //-----------------------------------------------------------------------------------------
        public IClientRequest<T> Select(string[] fields) {
            if (fields == null || fields.Length == 0) {
                if (queryParams.ContainsKey("$select")) {
                    queryParams.Remove("$select");
                }

                return this;
            }

            var selectedFields = string.Join(",", fields);
            if (!queryParams.ContainsKey("$select")) {
                queryParams.Add("$select", selectedFields);
            }

            queryParams["$select"] = selectedFields;
            return this;
        }

        //-----------------------------------------------------------------------------------------
        public IClientRequest<T> Select(Expression<Func<T, object>> fields) {

            if (fields == null) {
                if (queryParams.ContainsKey("$select")) {
                    queryParams.Remove("$select");
                }

                return this;
            }

            var props = fields.ExtractColumnNames(_typeCache);
            //props = Utils.GetCustomNamedProps<T>(props);
            var selectedFields = string.Join(",", props);
            if (!queryParams.ContainsKey("$select")) {
                queryParams.Add("$select", selectedFields);
            }

            queryParams["$select"] = selectedFields;
            return this;
        }

        //-----------------------------------------------------------------------------------------
        public IClientRequest<T> Expand(string[] fields) {
            if (fields == null || fields.Length == 0) {
                if (queryParams.ContainsKey("$expand")) {
                    queryParams.Remove("$expand");
                }

                return this;
            }

            var selectedFields = string.Join(",", fields);
            if (!queryParams.ContainsKey("$expand")) {
                queryParams.Add("$expand", selectedFields);
            }

            queryParams["$expand"] = selectedFields;
            return this;
        }
        //-----------------------------------------------------------------------------------------
        public IClientRequest<T> Expand(Expression<Func<T, object>> fields) {
            if (fields == null) {
                if (queryParams.ContainsKey("$expand")) {
                    queryParams.Remove("$expand");
                }

                return this;
            }

            var props = fields.ExtractColumnNames(_typeCache);
            //props = Utils.GetCustomNamedProps<T>(props);
            var selectedFields = string.Join(",", props);
            if (!queryParams.ContainsKey("$expand")) {
                queryParams.Add("$expand", selectedFields);
            }

            queryParams["$expand"] = selectedFields;
            return this;
        }
        //-----------------------------------------------------------------------------------------
        INavigatableClientRequest<T> INavigatableClientRequest<T>.Expand(Expression<Func<T, object>> fields) {
            return (INavigatableClientRequest<T>)Expand(fields);
        }
        //-----------------------------------------------------------------------------------------
        INavigatableClientRequest<T> INavigatableClientRequest<T>.Expand(string[] fields) {
            return (INavigatableClientRequest<T>)Expand(fields);
        }
        //-----------------------------------------------------------------------------------------
        public IClientRequest<T> Filter(Expression<Func<T, bool>> exp) {
            if (exp == null) {
                if (queryParams.ContainsKey("$filter")) {
                    queryParams.Remove("$filter");
                }

                return this;
            }

            var filterQuery = GetUrlExpression(exp);
            if (!queryParams.ContainsKey("$filter")) {
                queryParams.Add("$filter", filterQuery);
            }

            queryParams["$filter"] = filterQuery;
            return this;
        }

        //-----------------------------------------------------------------------------------------
        public IClientRequest<T> Filter(string exp) {
            if (string.IsNullOrWhiteSpace(exp)) {
                if (queryParams.ContainsKey("$filter")) {
                    queryParams.Remove("$filter");
                }

                return this;
            }

            var filterQuery = exp;
            if (!queryParams.ContainsKey("$filter")) {
                queryParams.Add("$filter", filterQuery);
            }

            queryParams["$filter"] = filterQuery;
            return this;
        }

        //-----------------------------------------------------------------------------------------
        public INavigatableClientRequest<T> Key(object key) {
            if (key == null) {
                throw new ArgumentNullException($"Key must be informed.");
            }

            if (key is string oKey) {
                _key = oKey.StartsWith("'") || oKey.EndsWith("'") 
                    ? oKey : 
                    $"'{oKey}'";
            }
            else {
                var t = key.GetType();
                var props = t.GetProperties();

                if (props.Length == 0) {
                    throw new ArgumentNullException($"key is a empty object.");
                }
                var pars = new List<string>();
                foreach (var prop in props) {
                    var par = $"{prop.Name}";
                    par = prop.PropertyType == typeof(string)
                        ? $"{par}='{prop.GetValue(key)}'" 
                        : $"{par}={prop.GetValue(key)}";
                    pars.Add(par);
                }

                _key = string.Join(",", pars);
            }
            return this;
        }
        //-----------------------------------------------------------------------------------------
        public INavigatableClientRequest<T> Key(Dictionary<string, string> key) {
            if (key == null || key.Keys.Count == 0) {
                throw new ArgumentNullException($"Key must be informed.");
            }

            var pars = new List<string>();

            foreach (var oKey in key.Keys) {
                pars.Add($"{oKey}={key[oKey]}");
            }

            var keyFields = string.Join(",", pars);
            _key = keyFields;
            return this;
        }
        //-----------------------------------------------------------------------------------------
        public IClientRequest<U> Navigate<U>(string path) where U : class {
            if (_key == null)
                throw new ArgumentNullException($"Key need to be informed.");
            return new ClientRequest<U>(_client, _settings, $"{_path}({_key})/{path}");
        }
        //-----------------------------------------------------------------------------------------
        public IClientRequest<U> Navigate<U>(Expression<Func<T, object>> exp) where U : class {
            if (exp == null) {
                throw new ArgumentNullException($"Parameter cannot be null.");
            }

            if (_key == null)
                throw new ArgumentNullException($"Key need to be informed.");

            var props = exp.ExtractColumnNames(_typeCache);
            //props = Utils.GetCustomNamedProps<T>(props);
            var selectedPath = props.FirstOrDefault();
            return new ClientRequest<U>(_client, _settings, $"{_path}({_key})/{selectedPath}");
        }
        //-----------------------------------------------------------------------------------------
        public IClientRequest<T> OrderBy(Dictionary<string, OrderByDirection> fields) {
            if (fields == null || fields.Keys.Count == 0) {
                if (queryParams.ContainsKey("$orderby")) {
                    queryParams.Remove("$orderby");
                }

                return this;
            }

            var pars = new List<string>();

            foreach (var key in fields.Keys) {
                pars.Add($"{key} {(fields[key] == OrderByDirection.Ascending ? "asc" : "desc")}");
            }

            var orderByFields = string.Join(",", pars);
            if (!queryParams.ContainsKey("$orderby")) {
                queryParams.Add("$orderby", orderByFields);
            }

            queryParams["$orderby"] = orderByFields;
            return this;
        }
        //-----------------------------------------------------------------------------------------
        public IClientRequest<T> Top(int number) {
            if (!queryParams.ContainsKey("$top")) {
                queryParams.Add("$top", number.ToString());
            }

            queryParams["$top"] = number.ToString();
            return this;
        }
        //-----------------------------------------------------------------------------------------
        public IUnboundClientRequest<U> Unbound<U>() where U : class {
            if (_key == null) {
                return new UnboundClientRequest<U>(_client, _settings, $"{_path}");
            }
            return new UnboundClientRequest<U>(_client, _settings, $"{_path}({_key})/");
        }
        //-----------------------------------------------------------------------------------------
        public IUnboundClientRequest Unbound() {
            if (_key == null) {
                return new UnboundClientRequest(_client, _settings, $"{_path}");
            }
            return new UnboundClientRequest(_client, _settings, $"{_path}({_key})/");
        }
        //-----------------------------------------------------------------------------------------
        #endregion

        #region Internal Methods
        //-----------------------------------------------------------------------------------------
        protected string BuildQuery() {
            var queryPars = queryParams.Select(x => $"{x.Key}={x.Value}").ToArray();
            var query = string.Join("&", queryPars);

            return query;
        }

        //-----------------------------------------------------------------------------------------
        protected string GetUrlExpression(Expression<Func<T, bool>> exp) {

            try {
                var t = new ODataExpression(exp).Format(new ExpressionContext(Session.FromSettings(_settings)));
                return t;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                var expBody = exp.Body.ToString();
                // Gives: ((x.Id > 5) AndAlso (x.Warranty != False))

                var paramName = exp.Parameters[0].Name;

                // You could easily add "OrElse" and others...
                expBody = expBody.Replace(paramName + ".", string.Empty)
                    .Replace("AndAlso", "and")
                    .Replace("OrElse", "or")
                    .Replace("==", "eq")
                    .Replace("!=", "ne")
                    .Replace("Not", "not")
                    .Replace("<=", "le")
                    .Replace(">=", "ge")
                    .Replace("<", "lt")
                    .Replace(">", "gt")

                    .Replace("+", "add")
                    .Replace("-", "sub")
                    .Replace("*", "mul")
                    .Replace("/", "div")

                    .Replace("%", "mod")

                    .Replace("\"", "'");
                return expBody;
            }



        }
        //-----------------------------------------------------------------------------------------
        #endregion

        //=============================================================================================
    }
}