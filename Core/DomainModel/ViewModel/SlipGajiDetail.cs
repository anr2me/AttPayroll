using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class SlipGajiDetail
    {
        public int Id { get; set; }
        public DateTime MONTH { get; set; } // Tahun dan bulan yg di proses
        public int EmployeeId { get; set; }
        public Nullable<int> SlipGajiDetail1Id { get; set; }
        public Nullable<int> SlipGajiDetail2AId { get; set; }

        public string NoBadge { get; set; }
        public string Name { get; set; }
        public string Jabatan { get; set; }
        public DateTime TanggalPenerimaan { get; set; }
        public DateTime PeriodeAwal { get; set; }
        public DateTime PeriodeAkhir { get; set; }
        public decimal GajiBasis { get; set; }
        public string StatusMarriage { get; set; }
        public decimal NoSlip { get; set; }
        public decimal Rate { get; set; }
        public decimal Lembur15 { get; set; }
        public decimal Lembur20 { get; set; }
        public decimal Lembur30 { get; set; }
        public decimal Lembur40 { get; set; }
        public string Disiapkan_oleh { get; set; }
        public string Disetujui_oleh { get; set; }
        public string Dikoreksi_oleh { get; set; }
        public string company_code { get; set; }

        public virtual SlipGajiDetail1 SlipGajiDetail1 { get; set; }
        public virtual SlipGajiDetail2A SlipGajiDetail2A { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }

    
}
