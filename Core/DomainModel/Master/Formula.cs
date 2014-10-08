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
        public int SalaryItemId { get; set; } // Parent Id
        //public string ItemSign { get; set; } // "-", "+"
        public Nullable<int> FirstSalaryItemId { get; set; } // 1st Operand
        public string FormulaOp { get; set; } // "+", "-", "/", "*"
        public bool IsValue { get; set; } // 2nd operand is a value (not an item)
        public Nullable<int> SecondSalaryItemId { get; set; } // 2nd Operand
        public decimal Value { get; set; }
        public string ValueSign { get; set; } // "-", "+"

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual SalaryItem FirstSalaryItem { get; set; }
        public virtual SalaryItem SecondSalaryItem { get; set; }
        public virtual SalaryItem SalaryItem { get; set; } // Parent
    }
}
