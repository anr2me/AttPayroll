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

        public string Code { get; set; }
        public string Name { get; set; }
        //public string ItemSign { get; set; } // "-", "+"
        public int FirstItemId { get; set; }
        public string FormulaOp { get; set; } // "+", "-", "/", "*"
        public bool IsVariable { get; set; }
        public Nullable<int> SecondItemId { get; set; }
        public decimal VariableValue { get; set; }
        public string VariableSign { get; set; } // "-", "+"

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual ICollection<SalaryItem> SalaryItems { get; set; }
    }
}
