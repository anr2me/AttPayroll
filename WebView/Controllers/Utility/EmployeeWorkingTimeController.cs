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
    
    public class EmployeeWorkingTimeController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("EmployeeWorkingTimeController");
        public IWorkingTimeService _workingTimeService;
        public IEmployeeService _employeeService;
        public IEmployeeWorkingTimeService _employeeWorkingTimeService;

        public EmployeeWorkingTimeController()
        {
            _workingTimeService = new WorkingTimeService(new WorkingTimeRepository(), new WorkingTimeValidator());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
            _employeeWorkingTimeService = new EmployeeWorkingTimeService(new EmployeeWorkingTimeRepository(), new EmployeeWorkingTimeValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.EmployeeWorkingTime, Core.Constants.Constant.MenuGroupName.Setting))
            {
                return Content(Core.Constants.Constant.ErrorPage.PageViewNotAllowed);
            }

            return View(this);
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _employeeWorkingTimeService.GetQueryable().Where(x => x.WorkingTimeId == id).Include("WorkingTime").Include("Employee").Include("TitleInfo").Include("Division");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.WorkingTimeId,
                             model.EmployeeId,
                             EmployeeNIK = model.Employee.NIK,
                             EmployeeName = model.Employee.Name,
                             Title = model.Employee.TitleInfo.Name,
                             Division = model.Employee.Division.Name,
                             //model.IsShiftable,
                             //model.IsEnabled,
                             model.StartDate,
                             model.EndDate,
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
                             model.WorkingTimeId,
                             model.EmployeeId,
                             model.EmployeeNIK,
                             model.EmployeeName,
                             model.Title,
                             model.Division,
                             model.StartDate,
                             model.EndDate,
                             model.CreatedAt,
                             model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
        
        public dynamic GetInfo(int Id)
        {
            EmployeeWorkingTime model = new EmployeeWorkingTime();
            try
            {
                model = _employeeWorkingTimeService.GetObjectById(Id);
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
                model.WorkingTimeId,
                model.EmployeeId,
                EmployeeNIK = model.Employee.NIK,
                EmployeeName = model.Employee.Name,
                Title = model.Employee.TitleInfo.Name,
                Division = model.Employee.Division.Name,
                model.StartDate,
                model.EndDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetDefaultInfo()
        {
            EmployeeWorkingTime model = new EmployeeWorkingTime();
            try
            {
                model = _employeeWorkingTimeService.GetQueryable().FirstOrDefault();
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
                model.WorkingTimeId,
                model.EmployeeId,
                EmployeeNIK = model.Employee.NIK,
                EmployeeName = model.Employee.Name,
                Title = model.Employee.TitleInfo.Name,
                Division = model.Employee.Division.Name,
                model.StartDate,
                model.EndDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(EmployeeWorkingTime model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.EmployeeWorkingTime, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _employeeWorkingTimeService.CreateObject(model, _workingTimeService, _employeeService);
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
        public dynamic Update(EmployeeWorkingTime model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.EmployeeWorkingTime, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _employeeWorkingTimeService.GetObjectById(model.Id);
                data.WorkingTimeId = model.WorkingTimeId;
                data.EmployeeId = model.EmployeeId;
                //data.IsEnabled = model.IsEnabled;
                //data.IsShiftable = model.IsShiftable;
                data.StartDate = model.StartDate;
                data.EndDate = model.EndDate;
                model = _employeeWorkingTimeService.UpdateObject(data, _workingTimeService, _employeeService);
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
        public dynamic UpdateEnable(EmployeeWorkingTime model, bool isEnabled)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.EmployeeWorkingTime, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _employeeWorkingTimeService.GetObjectById(model.Id);
                //data.IsEnabled = isEnabled;
                model = _employeeWorkingTimeService.UpdateObject(data, _workingTimeService, _employeeService);
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
        public dynamic Delete(EmployeeWorkingTime model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.EmployeeWorkingTime, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _employeeWorkingTimeService.GetObjectById(model.Id);
                model = _employeeWorkingTimeService.SoftDeleteObject(data, _employeeService);
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
