using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainDDD.Entities.Site
{
    public class Site
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public Guid OPUId { get; private set; }
    }
}
