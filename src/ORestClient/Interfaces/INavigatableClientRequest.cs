using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ORestClient.Interfaces {
    public interface INavigatableClientRequest<T> : IClientRequest<T> where T : class {

        Task<T> FindEntryAsync();

        Task<T> UpdateEntryAsync(T entry);

        Task<T> UpdateEntryAsync(object entry);

        Task<T> DeleteEntryAsync();

        IClientRequest<U> Navigate<U>(string path) where U : class;

        IClientRequest<U> Navigate<U>(Expression<Func<T, object>> exp) where U : class;

        IUnboundClientRequest<U> Unbound<U>() where U : class;

        IUnboundClientRequest Unbound();

    }
}