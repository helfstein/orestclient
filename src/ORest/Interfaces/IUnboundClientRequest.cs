using System.Collections.Generic;
using System.Threading.Tasks;

namespace ORest.Interfaces {
    public interface IUnboundClientRequest<T> where T : class {

        IExecutableClientRequest<T> Function(string path);

        IExecutableClientRequest<T> Function(string functionName, Dictionary<string, string> parameters);

        IExecutableClientRequest<T> Action(string actionName);

        IExecutableClientRequest<T> Action(string actionName, object data);

        

    }
    public interface IUnboundClientRequest {

        IExecutableClientRequest Function(string path);

        IExecutableClientRequest Function(string functionName, Dictionary<string, string> parameters);

        IExecutableClientRequest Action(string actionName);

        IExecutableClientRequest Action(string actionName, object data);

        

    }

    public interface IExecutableClientRequest<T> where T : class {

        Task<T> ExecuteAsSingleAsync();
        
        Task<IEnumerable<T>> ExecuteAsEnumerableAsync();

        Task ExecuteAsync();
        IExecutableClientRequest<T> UseMethod(Method method);
        

    }

    public interface IExecutableClientRequest {
        
        IExecutableClientRequest UseMethod(Method method);

        Task ExecuteAsync();
    }

    public enum Method {
        Get,
        Post
    }

}