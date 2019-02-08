﻿using System.Collections.Concurrent;
using ORestClient.Extensions;
using ORestClient.Interfaces;

namespace ORestClient {
    internal static class TypeCaches {
        private static ConcurrentDictionary<string, ITypeCache> _typeCaches;

        static TypeCaches() {
            // TODO: Have a global switch whether we use the dictionary or not
            _typeCaches = new ConcurrentDictionary<string, ITypeCache>();
        }

        internal static ITypeCache Global => TypeCache("global");

        internal static ITypeCache TypeCache(string uri) {
            return _typeCaches.GetOrAdd(uri, new TypeCache(CustomConverters.Converter(uri)));
        }
    }
}