using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class KRA
    {
        public int Id { get; set; }
        public int JobDescId { get; set; }
        public string JobDescName { get; set; }

        public string KeyResultArea { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public ICollection<KPI> KPIs { get; set; }
    }
}
