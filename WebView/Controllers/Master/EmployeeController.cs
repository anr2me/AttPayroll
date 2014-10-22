using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.Service;
using Core.Interface.Service;
using Core.DomainModel;
using Data.Repository;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace WebView.Controllers
{
    
    public class EmployeeController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("EmployeeController");
        public IEmployeeService _employeeService;
        public ITitleInfoService _titleInfoService;
        public IDivisionService _divisionService;
        public IDepartmentService _departmentService;
        public IBranchOfficeService _branchOfficeService;

        public EmployeeController()
        {
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
            _titleInfoService = new TitleInfoService(new TitleInfoRepository(), new TitleInfoValidator());
            _divisionService = new DivisionService(new DivisionRepository(), new DivisionValidator());
            _departmentService = new DepartmentService(new DepartmentRepository(), new DepartmentValidator());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
            _branchOfficeService = new BranchOfficeService(new BranchOfficeRepository(), new BranchOfficeValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.PersonalInfo, Core.Constants.Constant.MenuGroupName.Setting))
            {
                return Content(Core.Constants.Constant.ErrorPage.PageViewNotAllowed);
            }

            return View(this);
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "", int ParentId = 0)
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _employeeService.GetQueryable().Include("Division").Include("TitleInfo").Include("EmployeeWorkingTime").Where(x => (ParentId == 0 || x.DivisionId == ParentId));

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.NIK,
                             model.Name,
                             TitleInfoName = model.TitleInfo.Name,
                             model.PlaceOfBirth,
                             model.BirthDate,
                             model.Address,
                             model.PhoneNumber,
                             model.Email,
                             model.Sex,
                             model.MaritalStatus,
                             model.Children,
                             model.Religion,
                             model.NPWP,
                             model.NPWPDate,
                             model.JamsostekNo,
                             //model.Bank,
                             //model.BankAccount,
                             //model.BankAccountName,
                             model.WorkingStatus,
                             model.StartWorkingDate,
                             model.AppointmentDate,
                             model.ActiveStatus,
                             model.NonActiveDate,
                             model.CreatedAt,
                             model.UpdatedAt,
                         }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

            var list = query.AsEnumerable();

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            // default last page
            if (totalPages > 0)
            {
                if (!page.HasValue)
                {
                    pageIndex = totalPages - 1;
                    page = totalPages;
                }
            }

            list = list.Skip(pageIndex * pageSize).Take(pageSize);

            return Json(new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from model in list
                    select new
                    {
                        id = model.Id,
                        cell = new object[] {
                             model.Id,
                             model.NIK,
                             model.Name,
                             model.TitleInfoName,
                             model.PlaceOfBirth,
                             model.BirthDate,
                             model.Address,
                             model.PhoneNumber,
                             model.Email,
                             model.Sex,
                             model.MaritalStatus,
                             model.Children,
                             model.Religion,
                             model.NPWP,
                             model.NPWPDate,
                             model.JamsostekNo,
                             //model.Bank,
                             //model.BankAccount,
                             //model.BankAccountName,
                             model.WorkingStatus,
                             model.StartWorkingDate,
                             model.AppointmentDate,
                             model.ActiveStatus,
                             model.NonActiveDate,
                             model.CreatedAt,
                             model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            Employee model = new Employee();
            try
            {
                model = _employeeService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Id,
                model.NIK,
                model.Name,
                model.TitleInfoId,
                TitleInfoName = model.TitleInfo.Name,
                model.DivisionId,
                DivisionName = model.Division.Name,
                model.PlaceOfBirth,
                model.BirthDate,
                model.Address,
                model.PhoneNumber,
                model.Email,
                model.Sex,
                model.MaritalStatus,
                model.Children,
                model.Religion,
                model.NPWP,
                model.NPWPDate,
                model.JamsostekNo,
                model.Bank,
                model.BankAccount,
                model.BankAccountName,
                model.WorkingStatus,
                model.StartWorkingDate,
                model.AppointmentDate,
                model.ActiveStatus,
                model.NonActiveDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetDefaultInfo()
        {
            Employee model = new Employee();
            try
            {
                model = _employeeService.GetQueryable().FirstOrDefault();
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Id,
                model.NIK,
                model.Name,
                model.TitleInfoId,
                TitleInfoName = model.TitleInfo.Name,
                model.DivisionId,
                DivisionName = model.Division.Name,
                model.PlaceOfBirth,
                model.BirthDate,
                model.Address,
                model.PhoneNumber,
                model.Email,
                model.Sex,
                model.MaritalStatus,
                model.Children,
                model.Religion,
                model.NPWP,
                model.NPWPDate,
                model.JamsostekNo,
                model.Bank,
                model.BankAccount,
                model.BankAccountName,
                model.WorkingStatus,
                model.StartWorkingDate,
                model.AppointmentDate,
                model.ActiveStatus,
                model.NonActiveDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(Employee model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.PersonalInfo, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _employeeService.CreateObject(model, _divisionService, _titleInfoService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Update(Employee model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.PersonalInfo, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _employeeService.GetObjectById(model.Id);
                data.NIK = model.NIK;
                data.Name = model.Name;
                data.TitleInfoId = model.TitleInfoId;
                data.DivisionId = model.DivisionId;
                data.PlaceOfBirth = model.PlaceOfBirth;
                data.BirthDate = model.BirthDate;
                data.Address = model.Address;
                data.PhoneNumber = model.PhoneNumber;
                data.Email = model.Email;
                data.Sex = model.Sex;
                data.MaritalStatus = model.MaritalStatus;
                data.Children = model.Children;
                data.Religion = model.Religion;
                data.NPWP = model.NPWP;
                data.NPWPDate = model.NPWPDate;
                data.JamsostekNo = model.JamsostekNo;
                data.WorkingStatus = model.WorkingStatus;
                data.StartWorkingDate = model.StartWorkingDate;
                data.AppointmentDate = model.AppointmentDate;
                data.ActiveStatus = model.ActiveStatus;
                data.NonActiveDate = model.NonActiveDate;
                model = _employeeService.UpdateObject(data, _divisionService, _titleInfoService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(Employee model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.PersonalInfo, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _employeeService.GetObjectById(model.Id);
                model = _employeeService.SoftDeleteObject(data);
            }

            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }
    }
}
