using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainDDD.Entities.SubProduct
{
    public class SubProduct
    {
        public SubProductId SubProductId { get; private set; }
        public AssemblyProcessId AssemblyProcessId { get; private set; }
        public SubProductState State { get; private set; }
    }
}
