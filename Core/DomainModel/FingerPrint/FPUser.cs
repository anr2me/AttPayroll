using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class FPUser
    {
        public int Id { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        //public int UserID { get; set; } //EnrollNumber

        public int PIN { get; set; } // ushort //UserID? //Internal number of a user (Enroll Number)
        public byte Privilege { get; set; } // byte //0=user,1=enroller,2=admin,3=supervisor
        [StringLength(5)]
        public string Password { get; set; } // byte[5]
        [StringLength(8)]
        public string Name { get; set; } // byte[8]
        [StringLength(5)]
        public string Card { get; set; } // byte[5] //RF Card?
        public byte Group { get; set; } // byte //1 to 5
        public string TimeZones { get; set; } //int //ushort //"1:2:3" //user (or group) time slots (each user have 3)
        public Int64 PIN2 { get; set; } // uint // User ID used by FP Template
        public int VerifyMode { get; set; } // B&W: Single=1-15,Group=129-134 //Only for devices supporting multiple verification modes 
        //[MaxLength(4)]
        public string Reserved { get; set; } //byte[]

        public string Remark { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsInSync { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<FPTemplate> FPTemplates { get; set; }
        public virtual ICollection<FPAttLog> FPAttLogs { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }
}
