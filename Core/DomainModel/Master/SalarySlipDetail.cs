using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class SalarySlipDetail
    {
        public int Id { get; set; }
        public int SalarySign { get; set; } // addition or deduction to parent's amount
        public int SalarySlipId { get; set; }
        public int Index { get; set; }
        public int FormulaId { get; set; } // SalaryItemId
        public decimal Value { get; set; } // Base value
        public bool HasMinValue { get; set; }
        public decimal MinValue { get; set; }
        public bool HasMaxValue { get; set; }
        public decimal MaxValue { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual SalarySlip SalarySlip { get; set; }
        public virtual Formula Formula { get; set; } // SalaryItem
    }
}
