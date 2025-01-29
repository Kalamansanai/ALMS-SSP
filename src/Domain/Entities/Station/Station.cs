using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainDDD.Entities.Station
{
    public class Station
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public Guid LineId { get; private set; }
        public Guid DetectorId { get; private set; }
        public Guid SubProductId { get; private set; }
        public Guid BufferId { get; private set; }
    }
}
