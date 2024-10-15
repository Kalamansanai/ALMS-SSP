using DomainDDD.Entities.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainDDD.Entities.AssemblyProcess
{
    public class AssemblyProcess
    {
        public AssemblyProcessId Id { get; private set; }
        public List<Item.Item> items { get; private set; }
        public int MaxOrderNumber { get; private set; }

    }
}
