using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class OtherIncomeDetail
    {
        public int Id { get; set; }
        public int OtherIncomeId { get; set; }
        public int EmployeeId { get; set; }
        public int OtherIncomeType { get; set; } // legacy income type
        public int SalaryStatus { get; set; } // evently, daily, weekly, monthly, yearly
        public decimal Amount { get; set; }
        public Nullable<DateTime> EffectiveDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public int Recurring { get; set; } // repeat times
        public bool IsApproved { get; set; }
        public string Remark { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual OtherIncome OtherIncome { get; set; }
    }
}
