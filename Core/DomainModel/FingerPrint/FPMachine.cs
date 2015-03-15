using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class FPMachine
    {
        public int Id { get; set; }
        public int CompanyInfoId { get; set; }

        public string MachineName { get; set; } // DeviceName
        public int MachineNumber { get; set; } // RS232/RS485:101-132, USB:Outside 101-132, Ethernet:Unused/Any?
        public int CommType { get; set; } // Serial/Ethernet/USB
        public string EthernetIP { get; set; } // default = 192.168.1.201
        public int EthernetPort { get; set; } // default = 4370
        public string EthernetMAC { get; set; }
        public string SerialPort { get; set; }
        public int SerialBaudRate { get; set; } // need to be greater than 0 (9600,19200,38400,57600,115200)
        //[StringLength(5)]
        public string Password { get; set; } // byte[5]
        public string TimeZone { get; set; } // should use IANA standard instead of Windows standard to maintain compatibility with javascript??
        public decimal TimeZoneOffset { get; set; } // additional minutes offset for the timezone selected

        //public string ProductName { get; set; }
        public string Platform { get; set; }
        public string FirmwareVer { get; set; }
        public string ArithmeticVer { get; set; }
        public string SerialNumber { get; set; }
        public Int64 UserCount { get; set; }
        public Int64 AdminCount { get; set; }
        public Int64 FPCount { get; set; }
        public Int64 FCCount { get; set; }
        public Int64 PasswordCount { get; set; }
        public Int64 AttLogCount { get; set; }

        public bool IsAutoConnect { get; set; }
        public bool IsConnected { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsClearLogAfterDownload { get; set; }
        public bool IsInSync { get; set; }
        public Nullable<DateTime> LastSync { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual CompanyInfo CompanyInfo { get; set; }
        //public virtual ICollection<FPAttLog> FPAttLogs { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        //public FPDevice.ZKEvents fpDevice { get; set; }
    }
}
