using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class Formula
    {
        public int Id { get; set; }
        //public string Code { get; set; }
        public Nullable<int> SalarySlipDetailId { get; set; } // SalaryItemId // Parent Id
        //public string ItemSign { get; set; } // "-", "+"
        public Nullable<int> FirstSalaryItemId { get; set; } // 1st Operand
        public string FormulaOp { get; set; } // "+", "-", "/", "*"
        public bool IsSecondValue { get; set; } // 2nd operand is a value (not an item)
        public Nullable<int> SecondSalaryItemId { get; set; } // 2nd Operand
        public decimal SecondValue { get; set; }
        public int ValueSign { get; set; } // "-", "+" // -1, 1
        //public bool HasMinValue { get; set; }
        //public decimal MinValue { get; set; }
        //public bool HasMaxValue { get; set; }
        //public decimal MaxValue { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual SalaryItem FirstSalaryItem { get; set; }
        public virtual SalaryItem SecondSalaryItem { get; set; }
        public virtual SalarySlipDetail SalarySlipDetail { get; set; } // SalaryItem // Parent
    }
}
