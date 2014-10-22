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
    
    public class EmployeeLeaveController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("EmployeeLeaveController");
        public IEmployeeLeaveService _employeeLeaveService;
        public IEmployeeService _employeeService;
        public ISalaryItemService _salaryItemService;
        public ISalaryStandardDetailService _salaryStandardDetailService;

        public EmployeeLeaveController()
        {
            _employeeLeaveService = new EmployeeLeaveService(new EmployeeLeaveRepository(), new EmployeeLeaveValidator());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
            _salaryItemService = new SalaryItemService(new SalaryItemRepository(), new SalaryItemValidator());
            _salaryStandardDetailService = new SalaryStandardDetailService(new SalaryStandardDetailRepository(), new SalaryStandardDetailValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.EmployeeLeave, Core.Constants.Constant.MenuGroupName.Setting))
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
            var q = _employeeLeaveService.GetQueryable().Include("Employee").Include("EmployeeLeaveDetails").Include("TitleInfo");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.EmployeeId,
                             EmployeeNIK = model.Employee.NIK,
                             EmployeeName = model.Employee.Name,
                             Title = model.Employee.TitleInfo.Name,
                             Division = model.Employee.Division.Name,
                             model.StartDate,
                             model.EndDate,
                             model.Remark,
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
                             model.EmployeeId,
                             model.EmployeeNIK,
                             model.EmployeeName,
                             model.Title,
                             model.Division,
                             model.StartDate,
                             model.EndDate,
                             model.Remark,
                             model.CreatedAt,
                             model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            EmployeeLeave model = new EmployeeLeave();
            try
            {
                model = _employeeLeaveService.GetObjectById(Id);
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
                model.EmployeeId,
                EmployeeNIK = model.Employee.NIK,
                EmployeeName = model.Employee.Name,
                model.StartDate,
                model.EndDate,
                model.Remark,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetDefaultInfo()
        {
            EmployeeLeave model = new EmployeeLeave();
            try
            {
                model = _employeeLeaveService.GetQueryable().FirstOrDefault();
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
                model.EmployeeId,
                EmployeeNIK = model.Employee.NIK,
                EmployeeName = model.Employee.Name,
                model.StartDate,
                model.EndDate,
                model.Remark,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(EmployeeLeave model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.EmployeeLeave, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _employeeLeaveService.CreateObject(model, _employeeService);
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
        public dynamic Update(EmployeeLeave model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.EmployeeLeave, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _employeeLeaveService.GetObjectById(model.Id);
                data.EmployeeId = model.EmployeeId;
                data.StartDate = model.StartDate;
                data.EndDate = model.EndDate;
                data.Remark = model.Remark;
                model = _employeeLeaveService.UpdateObject(data, _employeeService);
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
        public dynamic Delete(EmployeeLeave model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.EmployeeLeave, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _employeeLeaveService.GetObjectById(model.Id);
                model = _employeeLeaveService.SoftDeleteObject(data);
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
