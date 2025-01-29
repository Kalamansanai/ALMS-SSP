using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainDDD.Entities.SubProduct
{
    public class SubProduct
    {
        public Guid SubProductId { get; private set; }
        public Guid AssemblyProcessId { get; private set; }
        public SubProductState State { get; private set; }
    }
}
