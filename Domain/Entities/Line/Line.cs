using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainDDD.Entities.Line
{
    public class Line
    {
        public LineId Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public OPUId OPUId { get; private set; }
        public StationId StationId { get; private set; }
        public ProductId ProductId { get; private set; }
    }
}
