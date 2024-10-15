using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainDDD.Entities.OPU
{
    public class OPU
    {
        public OPUId Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public SiteId SiteId { get; private set; }
        public LineId LineId { get; private set; }
    }
}
