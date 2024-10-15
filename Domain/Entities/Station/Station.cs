using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainDDD.Entities.Station
{
    public class Station
    {
        public StationId Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public LineId LineId { get; private set; }
        public DetectorId DetectorId { get; private set; }
        public SubProductId SubProductId { get; private set; }
        public BufferId BufferId { get; private set; }
    }
}
