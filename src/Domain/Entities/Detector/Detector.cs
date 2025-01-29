using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainDDD.Entities.Detector
{
    public class Detector
    {
        public Guid Id { get; private set; }
        public string MacAddress { get; private set; } = string.Empty;
        public Guid StationId { get; private set; }
        public DetectorState State { get; private set; }
    }
}
