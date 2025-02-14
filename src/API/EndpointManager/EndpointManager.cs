using API.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.AspNetCore.Builder;

namespace API.EndpointManager {
    public static class EndpointManager {
        private static EndpointBase[] _endpointMap = {
                    new EndpointTest()
                };

        /// <summary>
        /// Adds all endpoints described in API/EndpointManager.cs to the app
        /// all endpoints must be children of EndpointBase, with the Path and
        /// a function to execute upon calls being needed
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static Result AddEndpoints(this WebApplication app) {
            return Result.Try(() => {
                foreach (var endpoint in _endpointMap) {
                    // static type analysis doesn't really work,
                    // so runtime reflection needs to be used here
                    // since this only runs once, the performance impact is neglegible
                    app.MapPost(
                        (string)endpoint.GetType().GetProperty("Path").GetValue(endpoint)!,
                        (Delegate)endpoint.GetType().GetProperty("PathFunction").GetValue(endpoint)!
                    );
                }
            });
        }
    }
}
