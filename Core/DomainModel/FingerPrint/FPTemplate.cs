using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class FPTemplate // Template 9.0 arithmetic
    {
        public int Id { get; set; }
        public int FPUserId { get; set; }

        public int Size { get; set; } // ushort
        public int PIN { get; set; } // ushort //internal user ID, which corresponds to PIN2 in user table
        public Int16 FingerID { get; set; } //byte //0 to 9, 15 = all at once // fingerprint backup index
        public byte Valid { get; set; } //byte
        //[MaxLength(700)] //[StringLength(700)]
        public string Template { get; set; }  //byte[] // byte[602];//the max value is 602 bytes //BYTE *Template; when using FingerID = 15

        public string AlgoVer { get; set; } // null/"" or "9" or "10"
        public bool IsInSync { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual FPUser FPUser { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }
}
