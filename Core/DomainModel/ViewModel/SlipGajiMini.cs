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
        public DateTime MONTH { get; set; } // Tahun dan bulan yg di proses
        public int EmployeeId { get; set; }

        public string company_name { get; set; }
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public string department_name { get; set; }
        public string division_name { get; set; }
        public string title_name { get; set; }
        public DateTime start_working { get; set; }
        public string STATUS { get; set; }
        public decimal number_wd { get; set; }

        public decimal basic_salary { get; set; }
        public decimal transport_d { get; set; }
        public decimal transport_m { get; set; }
        public decimal fix_charge { get; set; }
        public decimal var_charge { get; set; }
        public decimal other { get; set; }
        public decimal bonus_thr { get; set; }
        public decimal company_astek_p { get; set; }
        public decimal gross_paid { get; set; }

        public decimal employee_astek { get; set; }
        public decimal company_astek_m { get; set; }
        public decimal deduction_koperasi { get; set; }
        public decimal deduction_union { get; set; }
        public decimal deduction_all { get; set; }

        public decimal t_jabatan { get; set; }
        public decimal nett_paid { get; set; }
        public decimal nett_paid_tax { get; set; }
        public decimal pph_in_year { get; set; }
        public decimal ptkp { get; set; }

        public decimal pkp { get; set; }
        public decimal pkp_5 { get; set; }
        public decimal pkp_15 { get; set; }
        public decimal pkp_25 { get; set; }
        public decimal pkp_30 { get; set; }

        public decimal tax_month { get; set; }
        public decimal thp { get; set; }
        public decimal balance_tax { get; set; }
        public decimal total_thp { get; set; }

        public Dictionary<string, string> Errors { get; set; }
    }
}
