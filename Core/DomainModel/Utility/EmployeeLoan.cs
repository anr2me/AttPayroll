using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class EmployeeLoan
    {
        public int Id { get; set; }

        public DateTime LoanDate { get; set; }
        public int EmployeeId { get; set; }
        public string Description { get; set; }
        public int SalaryItemId { get; set; } // ExpenseType:Kasbon
        public decimal Interest { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
        public decimal InstallmentValue { get; set; }
        public int InstallmentTimes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual SalaryItem SalaryItem { get; set; }
        public virtual ICollection<EmployeeLoanDetail> EmployeeLoanDetails { get; set; }
    }
}
