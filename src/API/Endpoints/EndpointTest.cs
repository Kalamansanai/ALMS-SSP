using API.EndpointManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Endpoints {
    public class EndpointTest : EndpointBase {
        public static string Path => "/lol";
        
        public static Delegate PathFunction => new Func<int, string>((int szam) => $"{szam} egy szep szam");
    }
}
