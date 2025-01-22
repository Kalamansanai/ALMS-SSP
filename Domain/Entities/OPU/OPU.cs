using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainDDD.Entities.OPU
{
    public class OPU
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public Guid SiteId { get; private set; }
        public Guid LineId { get; private set; }
    }
}
