using System.Net.Http;
using System.Threading.Tasks;

namespace ORest {
    //=============================================================================================
    public delegate Task<HttpRequestMessage> BeforeRequestAsync(HttpRequestMessage request);
    //=============================================================================================
    public delegate void TraceRequest(HttpRequestMessage request);
    //=============================================================================================
    public delegate void AfterResponse(HttpResponseMessage response);
    //=============================================================================================
}