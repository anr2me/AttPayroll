using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class SlipGajiMini
    {
        public int Id { get; set; }
        public DateTime MONTHyear { get; set; } // Tahun dan bulan yg di proses
        public int EmployeeId { get; set; }

        public string month	{ get; set; }
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public string title_code { get; set; }
        public string title_name { get; set; }
        public DateTime start_working { get; set; }
        public string no_rekening { get; set; }

        public decimal salary_basic { get; set; }
        public decimal rate_hour { get; set; }
        public decimal allowance_rate { get; set; }
        public decimal uang_makan { get; set; }
        public decimal jml_jam_lembur { get; set; }
        public decimal jml_lembur { get; set; }
        public decimal jml_hari_absen { get; set; }
        public decimal tunj_lap { get; set; }
        public decimal insentive_hadir { get; set; }
        public decimal krg_bln_lalu { get; set; }
        public decimal thr { get; set; }
        public decimal pot_absensi { get; set; }
        public decimal pot_others { get; set; }
        public decimal gaji_kotor { get; set; }
        public decimal pot_pinjaman { get; set; }
        public decimal pot_jamsostek { get; set; }
        public decimal pjk_jkk_jkm_204 { get; set; }
        public decimal tot_dpt_kotor { get; set; }
        public decimal pjk_tunj_jabatan { get; set; }
        public decimal pjk_ptkp { get; set; }
        public decimal tot_pengurang_pajak { get; set; }
        public decimal tot_dpt_kena_pajak { get; set; }
        public decimal tot_dpt_kena_pajak_tahun { get; set; }
        public decimal pph_5_persen { get; set; }
        public decimal pph_15_persen { get; set; }
        public decimal pph_25_persen { get; set; }
        public decimal pph_30_persen { get; set; }
        public decimal pph21 { get; set; }
        public decimal round { get; set; }
        public decimal gaji_bersih { get; set; }

        public string company_code { get; set; }
        public string Disiapkan_oleh { get; set; }
        public string Disetujui_oleh { get; set; }
        public string Dikoreksi_oleh { get; set; }

        public Dictionary<string, string> Errors { get; set; }
    }
}
