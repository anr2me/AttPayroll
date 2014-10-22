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

    public class SpecCompanyData : nspec
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

        void BranchOffice_validation()
        {

            it["validates_branchoffice"] = () =>
            {
                d.branch1.Errors.Count().should_be(0);
            };

            it["branchoffice_with_no_code"] = () =>
            {
                BranchOffice obj = new BranchOffice()
                {
                    Name = "no code",
                    Address = "address",
                    PhoneNumber = "phone",
                    Email = "email",
                };
                obj = d._branchOfficeService.CreateObject(obj);
                obj.Errors.Count().should_not_be(0);
            };

            it["branchoffice_with_same_code"] = () =>
            {
                BranchOffice obj = new BranchOffice()
                {
                    Code = d.branch1.Code,
                    Name = "same code",
                    Address = "address",
                    PhoneNumber = "phone",
                    Email = "email",
                };
                obj = d._branchOfficeService.CreateObject(obj);
                obj.Errors.Count().should_not_be(0);
            };

            it["branchoffice_with_no_name"] = () =>
            {
                BranchOffice obj = new BranchOffice()
                {
                    Code = "no_name",
                    Address = "address",
                    PhoneNumber = "phone",
                    Email = "email",
                };
                obj = d._branchOfficeService.CreateObject(obj);
                obj.Errors.Count().should_not_be(0);
            };

            it["branchoffice_with_no_address"] = () =>
            {
                BranchOffice obj = new BranchOffice()
                {
                    Code = "code",
                    Name = "no address",
                    PhoneNumber = "phone",
                    Email = "email",
                };
                obj = d._branchOfficeService.CreateObject(obj);
                obj.Errors.Count().should_not_be(0);
            };

            it["branchoffice_with_no_phone"] = () =>
            {
                BranchOffice obj = new BranchOffice()
                {
                    Code = "code",
                    Name = "no phone",
                    Address = "address",
                    Email = "email",
                };
                obj = d._branchOfficeService.CreateObject(obj);
                obj.Errors.Count().should_not_be(0);
            };

            it["branchoffice_with_no_email"] = () =>
            {
                BranchOffice obj = new BranchOffice()
                {
                    Code = "code",
                    Name = "no email",
                    PhoneNumber = "phone",
                    Address = "address",
                };
                obj = d._branchOfficeService.CreateObject(obj);
                obj.Errors.Count().should_not_be(0);
            };

            it["update_branchoffice_with_no_code"] = () =>
            {
                d.branch1.Code = "";
                d._branchOfficeService.UpdateObject(d.branch1);
                d.branch1.Errors.Count().should_not_be(0);
            };

            it["delete_branchoffice_having_departments"] = () =>
            {
                d._branchOfficeService.SoftDeleteObject(d.branch1, d._departmentService);
                d.branch1.Errors.Count().should_not_be(0);
            };

        }

        void Department_validation()
        {

            it["validates_department"] = () =>
            {
                d.dept1.Errors.Count().should_be(0);
            };

            it["department_with_no_branchoffice"] = () =>
            {
                Department obj = new Department()
                {
                    Code = "code",
                    Name = "no branchoffice",
                };
                obj = d._departmentService.CreateObject(obj, d._branchOfficeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["department_with_no_code"] = () =>
            {
                Department obj = new Department()
                {
                    BranchOfficeId = d.branch1.Id,
                    Name = "no code",
                };
                obj = d._departmentService.CreateObject(obj, d._branchOfficeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["department_with_same_code"] = () =>
            {
                Department obj = new Department()
                {
                    BranchOfficeId = d.branch1.Id,
                    Code = d.dept1.Code,
                    Name = "same code",
                };
                obj = d._departmentService.CreateObject(obj, d._branchOfficeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["department_with_no_name"] = () =>
            {
                Department obj = new Department()
                {
                    BranchOfficeId = d.branch1.Id,
                    Code = "no_name",
                };
                obj = d._departmentService.CreateObject(obj, d._branchOfficeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["update_department_with_no_code"] = () =>
            {
                d.dept1.Code = "";
                d._departmentService.UpdateObject(d.dept1, d._branchOfficeService);
                d.dept1.Errors.Count().should_not_be(0);
            };

            it["delete_department_having_divisions"] = () =>
            {
                d._departmentService.SoftDeleteObject(d.dept1, d._divisionService);
                d.dept1.Errors.Count().should_not_be(0);
            };

        }

        void Division_validation()
        {

            it["validates_division"] = () =>
            {
                d.div1.Errors.Count().should_be(0);
            };

            it["division_with_no_department"] = () =>
            {
                Division obj = new Division()
                {
                    Code = "code",
                    Name = "no department",
                };
                obj = d._divisionService.CreateObject(obj, d._departmentService);
                obj.Errors.Count().should_not_be(0);
            };

            it["division_with_no_code"] = () =>
            {
                Division obj = new Division()
                {
                    DepartmentId = d.dept1.Id,
                    Name = "no code",
                };
                obj = d._divisionService.CreateObject(obj, d._departmentService);
                obj.Errors.Count().should_not_be(0);
            };

            it["division_with_same_code"] = () =>
            {
                Division obj = new Division()
                {
                    DepartmentId = d.dept1.Id,
                    Code = d.div1.Code,
                    Name = "same code",
                };
                obj = d._divisionService.CreateObject(obj, d._departmentService);
                obj.Errors.Count().should_not_be(0);
            };

            it["division_with_no_name"] = () =>
            {
                Division obj = new Division()
                {
                    DepartmentId = d.dept1.Id,
                    Code = "no_name",
                };
                obj = d._divisionService.CreateObject(obj, d._departmentService);
                obj.Errors.Count().should_not_be(0);
            };

            it["update_division_with_no_code"] = () =>
            {
                d.div1.Code = "";
                d._divisionService.UpdateObject(d.div1, d._departmentService);
                d.div1.Errors.Count().should_not_be(0);
            };

            it["delete_division_having_employees"] = () =>
            {
                d._divisionService.SoftDeleteObject(d.div1, d._employeeService);
                d.div1.Errors.Count().should_not_be(0);
            };

        }

    }
}