using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class PPH21ebiz
    {
        public int Id { get; set; }
        public DateTime YEAR { get; set; } // Tahun yg di proses
        public int EmployeeId { get; set; }

        public string year { get; set; }
        public string company_npwp { get; set; }
        public string company_city { get; set; }
        public string company_name { get; set; }
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public string title_name { get; set; }
        public string employee_npwp { get; set; }
        public string address { get; set; }
        public string sex { get; set; }
        public string marital_status { get; set; }
        public string number_child { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }

        public decimal basic_salary1 { get; set; }
        public decimal tunj_pph2 { get; set; }
        public decimal lembur3 { get; set; }
        public decimal honor4 { get; set; }
        public decimal premi5 { get; set; }
        public decimal uangmakan6 { get; set; }
        public decimal thr7 { get; set; }
        public decimal jht12 { get; set; }
        public decimal jml_ptkp17 { get; set; }
        public decimal pph21 { get; set; }
        public decimal neto15 { get; set; }
        public decimal jml_bln_kerja { get; set; }
        public decimal tunj_lap { get; set; }
        public decimal insentive_hadir { get; set; }
        public decimal jkk_jkm_204 { get; set; }
        public decimal tunj_lain { get; set; }
        public decimal kurang_bulan_lalu { get; set; }
        public decimal potongan_absen { get; set; }
        public decimal potongan_lain { get; set; }
        public decimal pot_tunj_jabatan { get; set; }
        public decimal nol { get; set; }

        public Dictionary<string, string> Errors { get; set; }
    }
}
