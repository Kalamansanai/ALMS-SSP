using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainDDD.Entities.Item
{
    public class Item
    {
        public Guid Id { get; private set; }
        public int OrderId { get; private set; }
        public ItemState State { get; private set; }
    }
}
