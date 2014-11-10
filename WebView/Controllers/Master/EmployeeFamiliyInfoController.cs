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
using Service;

namespace WebView.Controllers
{
    
    public class EmployeeFamilyInfoController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("EmployeeFamilyInfoController");
        public IEmployeeService _employeeService;
        public MockService _mockService;

        public EmployeeFamilyInfoController()
        {
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
            _mockService = new MockService();
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.EmployeeFamilyInfo, Core.Constants.Constant.MenuGroupName.Setting))
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
            IList<EmployeeFamilyInfo> q = new List<EmployeeFamilyInfo>();
            for (int i = 1; i < 5; i++)
            {
                q.Add(_mockService.GenerateEmployeeFamilyInfo(i));
            }

            var query = q.Where(x => x.EmployeePersonalId == id);

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
                        cell = model.GetType()
                                .GetProperties()
                                .Select(p =>
                                {
                                    object value = p.GetValue(model, null);
                                    return value == null ? null : value; //.ToString();
                                })
                                .ToArray()
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
        
        public dynamic GetInfo(int Id)
        {
            EmployeeFamilyInfo model = new EmployeeFamilyInfo();
            try
            {
                model = _mockService.GenerateEmployeeFamilyInfo(Id); //_employeePersonalService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetDefaultInfo()
        {
            EmployeeFamilyInfo model = new EmployeeFamilyInfo();
            try
            {
                model = _mockService.GenerateEmployeeFamilyInfo(1); //_employeePersonalService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(EmployeeFamilyInfo model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.EmployeeFamilyInfo, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = new EmployeeFamilyInfo(); // _employeeEmployeeFamilyInfoService.CreateObject(model, _workingTimeService, _employeeService);
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
        public dynamic Update(EmployeeFamilyInfo model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.EmployeeFamilyInfo, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                //var data = _employeeEmployeeFamilyInfoService.GetObjectById(model.Id);
                //data.WorkingTimeId = model.WorkingTimeId;
                //data.EmployeeId = model.EmployeeId;
                ////data.IsEnabled = model.IsEnabled;
                ////data.IsShiftable = model.IsShiftable;
                //data.StartDate = model.StartDate;
                //data.EndDate = model.EndDate;
                //model = _employeeEmployeeFamilyInfoService.UpdateObject(data, _workingTimeService, _employeeService);
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
        public dynamic UpdateEnable(EmployeeFamilyInfo model, bool isEnabled)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.EmployeeFamilyInfo, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                //var data = _employeeEmployeeFamilyInfoService.GetObjectById(model.Id);
                ////data.IsEnabled = isEnabled;
                //model = _employeeEmployeeFamilyInfoService.UpdateObject(data, _workingTimeService, _employeeService);
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
        public dynamic Delete(EmployeeFamilyInfo model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.EmployeeFamilyInfo, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                //var data = _employeeEmployeeFamilyInfoService.GetObjectById(model.Id);
                //model = _employeeEmployeeFamilyInfoService.SoftDeleteObject(data, _employeeService);
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
