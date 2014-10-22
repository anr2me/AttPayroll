using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class OtherIncome
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsMainSalary { get; set; }
        public bool IsDetailSalary { get; set; }
        //public int SalarySign { get; set; } // 0 = expense, 1 = income
        //public int SalaryStatus { get; set; } // evently, daily, weekly, monthly, yearly
        public Nullable<int> SalaryItemId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual SalaryItem SalaryItem { get; set; }
        public virtual ICollection<OtherIncomeDetail> OtherIncomeDetails { get; set; }
    }
}
