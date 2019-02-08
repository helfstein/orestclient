using System;
using ORestClient.Extensions;

namespace ORestClient.Interfaces {
    /// <summary>
    /// Provide access to session-specific details.
    /// </summary>
    public interface ISession : IDisposable {
       

        /// <summary>
        /// Gets type information for this session.
        /// </summary>
        ITypeCache TypeCache { get; }

        Uri BaseUri { get; }
        // ODataAdapter Adapter { get; }


    }
}