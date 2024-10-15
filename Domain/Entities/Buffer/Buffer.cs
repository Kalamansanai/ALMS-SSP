using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainDDD.Entities.Buffer
{
    public class Buffer
    {
        public BufferId Id { get; private set; }
        public static int Capacity { get; private set; }
        public int InBufferCount { get; private set; }
    }
}
