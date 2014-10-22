using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class SalaryItem
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsLegacy { get; set; }
        public bool IsMainSalary { get; set; }
        public bool IsDetailSalary { get; set; }
        public decimal DefaultValue { get; set; }
        public decimal CurrentValue { get; set; } // temporary used during processing
        public int SalaryItemType { get; set; } // salary, attendance, salaryslip, otherincome, otherexpense, ptkp, pph21, thr
        public int SalarySign { get; set; } // -1 = expense, 1 = income
        public int SalaryItemStatus { get; set; } // evently, daily, weekly, monthly, yearly
        //public bool IsFormula { get; set; }
        //public Nullable<int> FormulaId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        //public virtual Formula Formula { get; set; }
    }
}
