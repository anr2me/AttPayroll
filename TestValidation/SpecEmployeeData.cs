using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using NSpec;
using Service.Service;
using Core.Interface.Service;
using Data.Context;
using System.Data.Entity;
using Data.Repository;
using Validation.Validation;

namespace TestValidation
{

    public class SpecEmployeeData : nspec
    {

        DataBuilder d;
        void before_each()
        {
            // something to do before checked
            var db = new AttPayrollEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();
                d.PopulateData();

                

            }
        }

        void Employee_validation()
        {

            it["validates_employee"] = () =>
            {
                d.emp1.Errors.Count().should_be(0);
            };

            it["employee_with_no_division"] = () =>
            {
                Employee obj = new Employee()
                {
                    TitleInfoId = d.tit1.Id,
                    NIK = "nik",
                    Name = "no division",
                    Address = "address",
                    PhoneNumber = "phone",
                };
                obj = d._employeeService.CreateObject(obj, d._divisionService, d._titleInfoService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employee_with_no_title"] = () =>
            {
                Employee obj = new Employee()
                {
                    DivisionId = d.div1.Id,
                    NIK = "nik",
                    Name = "no title",
                    Address = "address",
                    PhoneNumber = "phone",
                };
                obj = d._employeeService.CreateObject(obj, d._divisionService, d._titleInfoService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employee_with_no_nik"] = () =>
            {
                Employee obj = new Employee()
                {
                    DivisionId = d.div1.Id,
                    TitleInfoId = d.tit1.Id,
                    Name = "no nik",
                    Address = "address",
                    PhoneNumber = "phone",
                };
                obj = d._employeeService.CreateObject(obj, d._divisionService, d._titleInfoService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employee_with_same_nik"] = () =>
            {
                Employee obj = new Employee()
                {
                    DivisionId = d.div1.Id,
                    TitleInfoId = d.tit1.Id,
                    NIK = d.emp1.NIK,
                    Name = "same nik",
                    Address = "address",
                    PhoneNumber = "phone",
                };
                obj = d._employeeService.CreateObject(obj, d._divisionService, d._titleInfoService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employee_with_no_name"] = () =>
            {
                Employee obj = new Employee()
                {
                    DivisionId = d.div1.Id,
                    TitleInfoId = d.tit1.Id,
                    NIK = "no_name",
                    Address = "address",
                    PhoneNumber = "phone",
                };
                obj = d._employeeService.CreateObject(obj, d._divisionService, d._titleInfoService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employee_with_no_address"] = () =>
            {
                Employee obj = new Employee()
                {
                    DivisionId = d.div1.Id,
                    TitleInfoId = d.tit1.Id,
                    NIK = "nik",
                    Name = "no address",
                    PhoneNumber = "phone",
                };
                obj = d._employeeService.CreateObject(obj, d._divisionService, d._titleInfoService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employee_with_no_phone"] = () =>
            {
                Employee obj = new Employee()
                {
                    DivisionId = d.div1.Id,
                    TitleInfoId = d.tit1.Id,
                    NIK = "nik",
                    Name = "no phone",
                    Address = "address",
                };
                obj = d._employeeService.CreateObject(obj, d._divisionService, d._titleInfoService);
                obj.Errors.Count().should_not_be(0);
            };

            it["update_employee_with_no_nik"] = () =>
            {
                d.emp1.NIK = "";
                d._employeeService.UpdateObject(d.emp1, d._divisionService, d._titleInfoService);
                d.emp1.Errors.Count().should_not_be(0);
            };

            it["delete_employee"] = () =>
            {
                d._employeeService.SoftDeleteObject(d.emp1);
                d.emp1.Errors.Count().should_be(0);
            };
            
        }

        void EmployeeEducation_validation()
        {

            it["validates_employeeEducation"] = () =>
            {
                d.empEdu1.Errors.Count().should_be(0);
            };

            it["employeeEducation_with_no_employee"] = () =>
            {
                EmployeeEducation obj = new EmployeeEducation()
                {
                    Institute = "institute",
                    Major = "major",
                    Level = "level",
                    EnrollmentDate = DateTime.Now, 
                };
                obj = d._employeeEducationService.CreateObject(obj, d._employeeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employeeEducation_with_no_institute"] = () =>
            {
                EmployeeEducation obj = new EmployeeEducation()
                {
                    EmployeeId = d.emp1.Id,
                    Major = "major",
                    Level = "level",
                    EnrollmentDate = DateTime.Now,
                };
                obj = d._employeeEducationService.CreateObject(obj, d._employeeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employeeEducation_with_no_major"] = () =>
            {
                EmployeeEducation obj = new EmployeeEducation()
                {
                    EmployeeId = d.emp1.Id,
                    Institute = "institute",
                    Level = "level",
                    EnrollmentDate = DateTime.Now,
                };
                obj = d._employeeEducationService.CreateObject(obj, d._employeeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employeeEducation_with_no_level"] = () =>
            {
                EmployeeEducation obj = new EmployeeEducation()
                {
                    EmployeeId = d.emp1.Id,
                    Institute = "institute",
                    Major = "major",
                    EnrollmentDate = DateTime.Now,
                };
                obj = d._employeeEducationService.CreateObject(obj, d._employeeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employeeEducation_with_no_enrollmentdate"] = () =>
            {
                EmployeeEducation obj = new EmployeeEducation()
                {
                    EmployeeId = d.emp1.Id,
                    Institute = "institute",
                    Major = "major",
                    Level = "level",
                };
                obj = d._employeeEducationService.CreateObject(obj, d._employeeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["update_employeeEducation_with_no_institute"] = () =>
            {
                d.empEdu1.Institute = "";
                d._employeeEducationService.UpdateObject(d.empEdu1, d._employeeService);
                d.empEdu1.Errors.Count().should_not_be(0);
            };

            it["delete_employeeEducation"] = () =>
            {
                d._employeeEducationService.SoftDeleteObject(d.empEdu1);
                d.empEdu1.Errors.Count().should_be(0);
            };

        }

        void TitleInfo_validation()
        {

            it["validates_titleInfo"] = () =>
            {
                d.tit1.Errors.Count().should_be(0);
            };

            it["titleInfo_with_no_code"] = () =>
            {
                TitleInfo obj = new TitleInfo()
                {
                    Name = "no code",
                };
                obj = d._titleInfoService.CreateObject(obj);
                obj.Errors.Count().should_not_be(0);
            };

            it["titleInfo_with_same_code"] = () =>
            {
                TitleInfo obj = new TitleInfo()
                {
                    Code = d.tit1.Code,
                    Name = "same code",
                };
                obj = d._titleInfoService.CreateObject(obj);
                obj.Errors.Count().should_not_be(0);
            };

            it["titleInfo_with_no_name"] = () =>
            {
                TitleInfo obj = new TitleInfo()
                {
                    Code = "no_name",
                };
                obj = d._titleInfoService.CreateObject(obj);
                obj.Errors.Count().should_not_be(0);
            };

            it["update_titleInfo_with_no_code"] = () =>
            {
                d.tit1.Code = "";
                d._titleInfoService.UpdateObject(d.tit1);
                d.tit1.Errors.Count().should_not_be(0);
            };

            it["delete_titleInfo_having_employees"] = () =>
            {
                d._titleInfoService.SoftDeleteObject(d.tit1, d._employeeService);
                d.tit1.Errors.Count().should_not_be(0);
            };

        }

        void EmployeeWorkingTime_validation()
        {

            it["validates_employeeWorkingTime"] = () =>
            {
                d.ewt1.Errors.Count().should_be(0);
            };

            it["employeeWorkingTime_with_no_workingTime"] = () =>
            {
                EmployeeWorkingTime obj = new EmployeeWorkingTime()
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1),
                };
                obj = d._employeeWorkingTimeService.CreateObject(obj, d._workingTimeService, d._employeeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employeeWorkingTime_with_no_startdate"] = () =>
            {
                EmployeeWorkingTime obj = new EmployeeWorkingTime()
                {
                    WorkingTimeId = d.wt1.Id,
                    EndDate = DateTime.Today.AddDays(1),
                };
                obj = d._employeeWorkingTimeService.CreateObject(obj, d._workingTimeService, d._employeeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employeeWorkingTime_with_no_enddate"] = () =>
            {
                EmployeeWorkingTime obj = new EmployeeWorkingTime()
                {
                    WorkingTimeId = d.wt1.Id,
                    StartDate = DateTime.Today,
                };
                obj = d._employeeWorkingTimeService.CreateObject(obj, d._workingTimeService, d._employeeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["update_employeeWorkingTime_with_no_workingtime"] = () =>
            {
                d.ewt1.WorkingTimeId = 0;
                d._employeeWorkingTimeService.UpdateObject(d.ewt1, d._workingTimeService, d._employeeService);
                d.ewt1.Errors.Count().should_not_be(0);
            };

            it["delete_employeeWorkingTime_having_employees"] = () =>
            {
                d._employeeWorkingTimeService.SoftDeleteObject(d.ewt1, d._employeeService);
                d.ewt1.Errors.Count().should_not_be(0);
            };

        }

    }
}