using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class EmployeeLoanDetail
    {
        public int Id { get; set; }
        public int EmployeeLoanId { get; set; }
        public DateTime InstallmentMonth { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public bool IsPaid { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual EmployeeLoan EmployeeLoan { get; set; }
    }
}
