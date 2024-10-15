using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainDDD.Entities.Detector
{
    public class Detector
    {
        public DetectorId Id { get; private set; }
        public string MacAddress { get; private set; } = string.Empty;
        public StationId StationId { get; private set; }
        public DetectorState State { get; private set; }
    }
}
