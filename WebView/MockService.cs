using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.IO;
using Core.DomainModel;
using Core.Constants;
using Core.Interface.Service;
using Service.Service;
using Data.Repository;
using Validation.Validation;

namespace Service
{
    public class MockService
    {
        public ITitleInfoService _titleInfoService;
        public IDivisionService _divisionService;

        public TitleInfo jabatan;
        public Division divisi;

        public DataTable employeePersonalDT;

        public void SetDTSchema<T>(DataTable dt)
            where T : new()
        {
            dt.Columns.Clear();
            var obj = new T();
            foreach (var field in obj.GetType().GetProperties())
            {
                dt.Columns.Add(field.Name, field.PropertyType); // DataTable not supporting Nullable type
            }
            DataColumn[] keys = new DataColumn[1];
            keys[0] = dt.Columns[0];
            dt.PrimaryKey = keys;
        }

        public void InsertRow<T>(DataTable dt, T row)
            //where T : new()
        {
            DataRow dr = dt.NewRow();
            //var obj = new T();
            foreach (var field in row.GetType().GetProperties())
            {
                dr[field.Name] = field.GetValue(field); // GetValue(row)
            }
            dt.Rows.Add(dr);
        }

        public T GetRow<T>(DataTable dt, int id)
        where T : new()
        {
            DataRow dr = dt.Rows.Find(id);
            var obj = new T();
            foreach (var field in obj.GetType().GetProperties())
            {
                field.SetValue(obj, dr[field.Name]); // GetValue(row)
            }
            return obj;
        }

        public void DeleteRow(DataTable dt, int id)
        {
            DataRow dr = dt.Rows.Find(id);
            dt.Rows.Remove(dr);
        }

        public MockService()
        {
            _titleInfoService = new TitleInfoService(new TitleInfoRepository(), new TitleInfoValidator());
            _divisionService = new DivisionService(new DivisionRepository(), new DivisionValidator());

            employeePersonalDT = new DataTable("EmployeePersonal");
            //SetDTSchema<EmployeePersonal>(employeePersonalDT);
        }

        public EmployeePersonal GenerateEmployeePersonal(int Id)
        {
            EmployeePersonal obj = new EmployeePersonal
            {
                Id = 74,
                FirstName = "SYAMSUL",
                LastName = "HADI",
                NickName = "HADI",
                StartWorkingDate = new DateTime(2010, 2, 5),
                IDNumber = "1234567890",
                ExpirationDate = new DateTime(2015, 6, 10),
                IDName = "SYAMSUL HADI",
                PlaceOfBirth = "Jakarta",
                BirthDate = new DateTime(1985, 2, 3),
                GolDarah = "B",
                JenisGolDarah = "-",
                LastEducation = "S1 TeknikInformatika",
                Religion = 1,
                Nationality = "Indonesia",
                EmergencyContactName = "Junaedi",
                EmergencyPhoneNumber = "021-5556666",
                EmergencyContactRemark = "Adik",
                IDAddress = "Jl. Tanjung Duren Raya No.10",
                IDCity = "Jakarta",
                IDPostalCode = "11470",
                Address = "Jl. Tanjung Duren Raya No.10",
                City = "Jakarta",
                PostalCode = "11470",
                PhoneNumber = "021-5557777",
                TertanggungPPH21 = 1320000,
                NPWP = "1928374645",
                NPWP_Address = "Jl. Tanjung Duren Raya No.10",
                NPWP_Kecamatan = "Grogol Petamburan",
                NPWP_Kelurahan = "Tanjung Duren Selatan",
                NPWP_KodePos = "11470",
                NPWP_Kota = "Jakarta",
                NPWP_RT = "9",
                NPWP_RW = "6",
                NPWP_Name = "SYAMSUL HADI",
                BankAccountName = "SYAMSUL HADI",
                BankName = "BCA",
                BankBranch = "KCP Tanjung Duren",
                BankAccountNumber = "4567891234",
                Email = "SYAMSULHADI@gmail.com",
                MobileNumber = "0812345678",
                Indonesia = 3,
                Mandarin = 1,
                Inggris = 2,
                ChildNumber = 1,
                OutOfBrothers = 3,
                NIK = "08051000089",
                JobType = 1,
                TitleInfoId = 74,
                TitleInfoName = _titleInfoService.GetObjectById(74).Name,
                DivisionId = 148,
                DivisionName = _divisionService.GetObjectById(148).Name,
                JoinedDate = new DateTime(2010, 2, 5),
                StartingDate = new DateTime(2010, 2, 5),
                EndingDate = new DateTime(2015, 2, 5),
                CreatedAt = new DateTime(2014, 10, 2),
                Errors = new Dictionary<string, string>(),
            };
            if (Id == 2 || Id == 80)
            {
                obj.Id = 80;
                obj.FirstName = "Riky";
                obj.LastName = "Volta";
                obj.NickName = "Riky";
                obj.IDNumber = "2341567809";
                obj.BirthDate = new DateTime(1987, 5, 6);
                obj.IDName = obj.FirstName + " " + obj.LastName;
                obj.GolDarah = "A";
                obj.JenisGolDarah = "+";
                obj.LastEducation = "S1 Akuntansi";
                obj.Religion = 3;
                obj.MaritalStatus = (int)Constant.MaritalStatus.Married;
                obj.MarriedDate = new DateTime(2010, 3, 2);
                obj.EmergencyContactName = "Rosid";
                obj.EmergencyPhoneNumber = "021-5554444";
                obj.IDAddress = "Jl. Tanjung Palapa No.5";
                obj.PhoneNumber = "021-5553333";
                obj.NPWP = "4328542153";
                obj.NPWP_Address = "Jl. Tanjung Palapa No.5";
                obj.NPWP_Name = obj.IDName;
                obj.BankAccountName = obj.IDName;
                obj.BankAccountNumber = "5372519680";
                obj.Email = "rikyvolta@gmail.com";
                obj.MobileNumber = "0813876543";
                obj.NIK = "3264187651";
                obj.TitleInfoId = 75;
                obj.TitleInfoName = _titleInfoService.GetObjectById(obj.TitleInfoId).Name;
                obj.SupervisorId = 74;
                obj.SupervisorName = "SYAMSUL HADI";
                obj.SuperiorId = 74;
                obj.SuperiorName = "SYAMSUL HADI";
            }
            else if (Id > 2 && Id != 74)
            {
                obj.Id = 0;
                obj.Errors.Add("Generic", "Unable to create record");
            };
            //InsertRow<EmployeePersonal>(employeePersonalDT, obj);
            return obj;
        }

        public EmployeeFamilyInfo GenerateEmployeeFamilyInfo(int Id)
        {
            EmployeeFamilyInfo obj = new EmployeeFamilyInfo
            {
                Id = 1,
                EmployeePersonalId = 74,
                EmployeePersonalNIK = "2345678910",
                EmployeePersonalName = "SYAMSUL HADI",
                Name = "Udin Mahudin",
                Relationship = (int)Constant.Relationship.Ayah,
                BirthDate = new DateTime(1960, 6, 5),
                Job = "Pensiunan PNS",
                Sex = (int)Constant.Sex.Male,
                MaritalStatus = (int)Constant.MaritalStatus.Married,
                LastEducation = "SMK",
                IsRelative = true,
                CreatedAt = new DateTime(2014, 10, 2),
                Errors = new Dictionary<string, string>(),
            };
            if (Id == 2)
            {
                obj.Id = Id;
                obj.EmployeePersonalId = 80;
                obj.EmployeePersonalNIK = "2341567809";
                obj.EmployeePersonalName = "Riki Volta";
                obj.Name = "Nina Marina";
                obj.Relationship = (int)Constant.Relationship.Istri;
                obj.BirthDate = new DateTime(1987, 2, 5);
                obj.Job = "PNS";
                obj.Sex = (int)Constant.Sex.Female;
                obj.MaritalStatus = (int)Constant.MaritalStatus.Married;
                obj.LastEducation = "S1 Ekonomi";
                obj.IsRelative = false;
            }
            else if (Id == 3)
            {
                obj.Id = Id;
                obj.EmployeePersonalId = 80;
                obj.EmployeePersonalNIK = "2341567809";
                obj.EmployeePersonalName = "Riki Volta";
                obj.Name = "Mini Volta";
                obj.Relationship = (int)Constant.Relationship.Anak;
                obj.BirthDate = new DateTime(2005, 3, 5);
                obj.Job = "Pelajar";
                obj.Sex = (int)Constant.Sex.Female;
                obj.MaritalStatus = (int)Constant.MaritalStatus.Single;
                obj.LastEducation = "SD";
                obj.IsRelative = false;
            }
            else if (Id == 4)
            {
                obj.Id = Id;
                obj.EmployeePersonalId = 80;
                obj.EmployeePersonalNIK = "2341567809";
                obj.EmployeePersonalName = "Riki Volta";
                obj.Name = "Siti Nurhaliza";
                obj.Relationship = (int)Constant.Relationship.Ibu;
                obj.BirthDate = new DateTime(1965, 1, 5);
                obj.Job = "Ibu Rumah Tangga";
                obj.Sex = (int)Constant.Sex.Female;
                obj.MaritalStatus = (int)Constant.MaritalStatus.Married;
                obj.LastEducation = "SMA";
                obj.IsRelative = false;
            }
            else if (Id > 4)
            {
                obj.Id = 0;
                obj.Errors.Add("Generic", "Unable to create record");
            };
            return obj;
        }

        public Education GenerateEducation(int Id)
        {
            Education obj = new Education
            {
                Id = 1,
                Jenjang = 0,
                Errors = new Dictionary<string, string>(),
            };
            if (Id == 2)
            {
                obj.Id = Id;
                obj.Jenjang = 1;
            }
            else if (Id == 3)
            {
                obj.Id = Id;
                obj.Jenjang = 2;
            }
            else if (Id == 4)
            {
                obj.Id = Id;
                obj.Jenjang = 2;
                obj.Jurusan = "SMK";
            }
            else if (Id == 5)
            {
                obj.Id = Id;
                obj.Jenjang = 4;
                obj.Jurusan = "Akuntansi";
            }
            else if (Id == 6)
            {
                obj.Id = Id;
                obj.Jenjang = 4;
                obj.Jurusan = "Manajemen";
            }
            else if (Id == 7)
            {
                obj.Id = Id;
                obj.Jenjang = 5;
                obj.Jurusan = "Informatika";
            }
            else if (Id == 8)
            {
                obj.Id = Id;
                obj.Jenjang = 5;
                obj.Jurusan = "Ekonomi";
            }
            else if (Id > 8)
            {
                obj.Id = 0;
                obj.Errors.Add("Generic", "Unable to create record");
            };
            return obj;
        }

        public Applicant GenerateApplicant(int Id)
        {
            Applicant obj = new Applicant
            {
                Id = 1,
                Name = "Miranda",
                BirthDate = new DateTime(1988, 6, 5),
                Address = "Jl. Pramuka No.3",
                City = "Jakarta",
                PhoneNumber = "021-4369865",
                EducationId = 6,
                EducationName = "D3Manajemen",
                Email = "miranda@gmail.com",
                Position = "HR Manajer",
                Source = "Internal",
                CreatedAt = new DateTime(2014, 10, 2),
                Errors = new Dictionary<string, string>(),
            };
            if (Id == 2)
            {
                obj.Id = Id;
                obj.Status = 1;
                obj.Name = "Saputra";
                obj.BirthDate = new DateTime(1987, 4, 5);
                obj.Address = "Jl. Pemuda No.7";
                obj.City = "Jakarta";
                obj.PhoneNumber = "021-6543168";
                obj.EducationId = 7;
                obj.EducationName = "S1Informatika";
                obj.Email = "saputra@gmail.com";
                obj.Position = "IT Support";
                obj.Source = "Kurir";
            }
            else if (Id > 2)
            {
                obj.Id = 0;
                obj.Errors.Add("Generic", "Unable to create record");
            };
            return obj;
        }

        public EmployeeEducationMini GenerateEmployeeEducationMini(int Id)
        {
            EmployeeEducationMini obj = new EmployeeEducationMini
            {
                Id = 1,
                NamaSekolah = "Universitas Indonesia",
                Alamat = "Depok",
                Tahun = "2005",
                EmployeePersonalId = 74,
                EmployeePersonalNIK = "2345678910",
                EmployeePersonalName = "SYAMSUL HADI",
                CreatedAt = new DateTime(2014, 10, 2),
                Errors = new Dictionary<string, string>(),
            };
            if (Id == 2)
            {
                obj.Id = Id;
                obj.NamaSekolah = "Universitas Gunadarma";
                obj.EmployeePersonalId = 80;
                obj.EmployeePersonalNIK = "2341567809";
                obj.EmployeePersonalName = "Riki Volta";
            }
            else if (Id > 2)
            {
                obj.Id = 0;
                obj.Errors.Add("Generic", "Unable to create record");
            };
            return obj;
        }

        public JobDesc GenerateJobDesc(int Id)
        {
            JobDesc obj = new JobDesc
            {
                Id = 1,
                TitleInfoId = 77,
                TitleInfoName = "STAFF GUDANG",
                FungsiJabatan = "Bertanggungjawab atas keluar masuknya barang, kebersihan dan keamanan serta penataan barang di Gudang sesuai dengan kebijakan dan prosedur yang telah ditetapkan",
                TugasTambahan = "Tugas-tugas lain yang akan diberikan oleh atasan sesuai cakupan tanggung jawabnya",
                MinEducationId = 4,
                MinEducationName = "SLTA SMK",
                TechnicalCompetency = "Pengetahuan tentang gudang",
                CreatedAt = new DateTime(2014, 10, 2),
                Errors = new Dictionary<string, string>(),
            };
            if (Id > 1)
            {
                obj.Id = 0;
                obj.TitleInfoId = 0;
                obj.Errors.Add("Generic", "Unable to create record");
            };
            return obj;
        }

        public EmployeeExperience GenerateEmployeeExperience(int Id)
        {
            EmployeeExperience obj = new EmployeeExperience
            {
                Id = 0,
                Errors = new Dictionary<string, string>(),
            };
            obj.Errors.Add("Generic", "Unable to create record");
            return obj;
        }

        public EmployeeProfessionLicense GenerateEmployeeProfessionLicense(int Id)
        {
            EmployeeProfessionLicense obj = new EmployeeProfessionLicense
            {
                Id = 0,
                Errors = new Dictionary<string, string>(),
            };
            obj.Errors.Add("Generic", "Unable to create record");
            return obj;
        }

        public EmployeeProfessionMembership GenerateEmployeeProfessionMembership(int Id)
        {
            EmployeeProfessionMembership obj = new EmployeeProfessionMembership
            {
                Id = 0,
                Errors = new Dictionary<string, string>(),
            };
            obj.Errors.Add("Generic", "Unable to create record");
            return obj;
        }

        public JobDescMembership GenerateJobDescMembership(int Id)
        {
            JobDescMembership obj = new JobDescMembership
            {
                Id = 1,
                ApplicantId = 2,
                ApplicantName = "Saputra",
                JobDescId = 1,
                JobDescName = "STAFF GUDANG",
                CreatedAt = new DateTime(2014, 10, 2),
                Errors = new Dictionary<string, string>(),
            };
            if (Id > 1)
            {
                obj.Id = 0;
                obj.ApplicantId = 0;
                obj.JobDescId = 0;
                obj.Errors.Add("Generic", "Unable to create record");
            };
            return obj;
        }

        public JobDescAssignment GenerateJobDescAssignment(int Id)
        {
            JobDescAssignment obj = new JobDescAssignment
            {
                Id = 1,
                ApplicantId = 2,
                ApplicantName = "Saputra",
                EmployeePersonalId = 80,
                JobDescId = 1,
                JobDescName = "STAFF GUDANG",
                DivisionId = 149,
                DivisionName = "GUDANG",
                SuperiorId = 74,
                SuperiorName = "SYAMSUL HADI",
                SupervisorId = 74,
                SupervisorName = "SYAMSUL HADI",
                Location = "JAPAN 28",
                EffectiveDate = new DateTime(2015, 1, 1),
                CreatedAt = new DateTime(2014, 10, 2),
                Errors = new Dictionary<string, string>(),
            };
            if (Id > 1)
            {
                obj.Id = 0;
                obj.JobDescId = 0;
                obj.ApplicantId = 0;
                obj.Errors.Add("Generic", "Unable to create record");
            };
            return obj;
        }

        public KRA GenerateKRA(int Id)
        {
            KRA obj = new KRA
            {
                Id = 1,
                JobDescId = 1,
                JobDescName = "STAFF GUDANG",
                KeyResultArea = "Mengelola fisik barang persediaan di gudang sesuai dengn SOP",
                Errors = new Dictionary<string, string>(),
            };
            if (Id > 1)
            {
                obj.Id = 0;
                obj.JobDescId = 0;
                obj.Errors.Add("Generic", "Unable to create record");
            };
            return obj;
        }

        public KPI GenerateKPI(int Id)
        {
            KPI obj = new KPI
            {
                Id = 1,
                KRAId = 1,
                KeyPerformanceIndicator = "Tidak ada barang yang expired",
                Errors = new Dictionary<string, string>(),
            };
            if (Id > 1)
            {
                obj.Id = 0;
                obj.KRAId = 0;
                obj.Errors.Add("Generic", "Unable to create record");
            };
            return obj;
        }

    }
}