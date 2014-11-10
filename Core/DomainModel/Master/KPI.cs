using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class KPI
    {
        public int Id { get; set; }
        public int KRAId { get; set; }

        public string KeyPerformanceIndicator { get; set; }

        public Dictionary<string, string> Errors { get; set; }
    }
}
