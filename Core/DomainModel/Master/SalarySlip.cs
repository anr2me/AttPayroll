using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class SalarySlip
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsMainSalary { get; set; }
        public bool IsDetailSalary { get; set; }
        public int SalarySign { get; set; } // 0 = expense, 1 = income
        public int SalaryItemId { get; set; }
        public decimal TotalAmount { get; set; } // 
        public bool IsPTKP { get; set; }
        //public Nullable<int> PTKPId { get; set; }
        public bool IsPPH21 { get; set; }
        //public Nullable<int> PPH21SPTId { get; set; }
        public bool IsVisible { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual SalaryItem SalaryItem { get; set; }
        //public virtual PTKP PTKP { get; set; }
        //public virtual PPH21SPT PPH21SPT { get; set; }
        public virtual ICollection<SalarySlipDetail> SalarySlipDetails { get; set; }

    }
}
