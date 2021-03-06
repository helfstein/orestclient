﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ORest.Interfaces {
    public interface IClientRequest<T> where T : class {
        /// <summary>
        /// Retunrs a Collenction of EntitySets names
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> Authenticate();

        Task<IEnumerable<T>> FindEntriesAsync();
        
        Task<T> InsertEntryAsync(T entry);

        IClientRequest<T> Skip(int number);

        IClientRequest<T> Top(int number);

        IClientRequest<T> Select(Expression<Func<T, object>> fields);

        IClientRequest<T> Select(string[] fields);

        IClientRequest<T> OrderBy(Dictionary<string, OrderByDirection> fields);

        IClientRequest<T> Expand(string[] fields);

        IClientRequest<T> Expand(Expression<Func<T, object>> fields);

        IClientRequest<T> Filter(Expression<Func<T, bool>> exp);

        IClientRequest<T> Filter(string exp);

        INavigatableClientRequest<T> Key(object key);
        
        INavigatableClientRequest<T> Key(Dictionary<string, string> key);

    }
}