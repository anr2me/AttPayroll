using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class EmployeePersonal
    {
        public int Id { get; set; }

        //Base Info
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public DateTime StartWorkingDate { get; set; }

        // Current Working Status
        public string NIK { get; set; }
        public int JobType { get; set; } // part-time/full-time
        public DateTime JoinedDate { get; set; }
        public int TitleInfoId { get; set; }
        public string TitleInfoName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int SupervisorId { get; set; }
        public string SupervisorName { get; set; }
        public int SuperiorId { get; set; } // Boss
        public string SuperiorName { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public string Remark { get; set; }

        // Personal Info
        public int IDType { get; set; }
        public string IDNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string IDName { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime BirthDate { get; set; }
        public int Sex { get; set; }
        public string GolDarah { get; set; } // A,B,AB,O
        public string JenisGolDarah { get; set; } // positif/negatif
        public int MaritalStatus { get; set; } //Single(Widowed)/Married/Divroced
        public Nullable<DateTime> MarriedDate { get; set; }
        public string LastEducation { get; set; }
        public int Religion { get; set; }
        public string Nationality { get; set; }
        public int ChildNumber { get; set; } // anak ke ...
        public int OutOfBrothers { get; set; } // dari ... bersaudara

        public string EmergencyContactName { get; set; } 
        public string EmergencyPhoneNumber { get; set; } // nomer yg bisa dihubungi
        public string EmergencyContactRemark { get; set; } // hubungan dengan karyawan

        public string IDAddress { get; set; }
        public string IDCity { get; set; }
        public string IDPostalCode { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }

        public decimal TertanggungPPH21 { get; set; } // ?? 1320000
        public string NPWP { get; set; }
        public string NPWP_Name { get; set; }
        public string NPWP_Address { get; set; }
        public string NPWP_RT { get; set; }
        public string NPWP_RW { get; set; }
        public string NPWP_Kelurahan { get; set; }
        public string NPWP_Kecamatan { get; set; }
        public string NPWP_Kota { get; set; }
        public string NPWP_KodePos { get; set; }

        public string BankAccountName { get; set; } // nama di rekening
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string BankAccountNumber { get; set; }
        public int BankAccountCurrency { get; set; }

        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string ExtensionNumber { get; set; }
        public string Secretary { get; set; }

        // Pengetahuan Bahasa
        public int Inggris { get; set; }
        public int Mandarin { get; set; }
        public int Indonesia { get; set; }
        public string OtherLang { get; set; }
        public int OtherGrade { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        //public virtual EmployeeEducation LastEducation { get; set; }
        public virtual ICollection<EmployeeFamilyInfo> EmployeeFamilyInfos { get; set; }
        //public virtual ICollection<EmployeeRelative> EmployeeRelatives { get; set; }
        public virtual ICollection<EmployeeStatusInfo> EmployeeStatusInfos { get; set; }
        public virtual ICollection<EmployeeProfessionLicense> EmployeeProfessionLicenses { get; set; }
        public virtual ICollection<EmployeeExperience> EmployeeExperiences { get; set; }
        public virtual ICollection<EmployeeEducationMini> EmployeeEducationMinis { get; set; }
    }
}
