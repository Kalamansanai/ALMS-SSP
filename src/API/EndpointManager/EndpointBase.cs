using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.EndpointManager {
    /// <summary>
    /// Base for all endpoint classes
    /// </summary>
    /// <remarks>
    /// after a child is defined, it must be added to the array in
    /// EndpointManager.cs to be usable
    /// </remarks>
    public interface EndpointBase {
        /// <summary>
        /// The local path that can call this endpoint
        /// </summary>
        static abstract string Path { get; }

        /// <summary>
        /// The function to execute upon the endpoint being called
        /// </summary>
        /// <remarks>
        /// The easiest way to use this is to create a Func<...> from a lambda
        /// (it can use/return any type of parameters)
        /// </remarks>
        static abstract Delegate PathFunction { get; }
    }
}
