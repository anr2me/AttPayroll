using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class SlipGajiDetail1
    {
        public int Id { get; set; }
        public int SlipGajiDetailId { get; set; }

        public string NoBadge { get; set; }
        public DateTime Tanggal { get; set; }
        public string Shift { get; set; }
        public string Status { get; set; }
        public decimal jamkerjaActual { get; set; }
        public decimal jamReg { get; set; }
        public decimal Lembur15 { get; set; }
        public decimal Lembur20 { get; set; }
        public decimal Lembur30 { get; set; }
        public decimal Lembur40 { get; set; }
        public string nmhari { get; set; }

        public virtual SlipGajiDetail SlipGajiDetail { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }

    
}
