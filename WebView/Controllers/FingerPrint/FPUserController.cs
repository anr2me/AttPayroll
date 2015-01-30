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
    
    public class FPUserController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("FPUserController");
        public IFPUserService _fpUserService;
        public IFPTemplateService _fpTemplateService;
        public IEmployeeService _employeeService;

        public FPUserController()
        {
            _fpUserService = new FPUserService(new FPUserRepository(), new FPUserValidator());
            _fpTemplateService = new FPTemplateService(new FPTemplateRepository(), new FPTemplateValidator());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.FPUser, Core.Constants.Constant.MenuGroupName.Setting))
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
            var q = _fpUserService.GetQueryable().Where(x => !x.IsDeleted).Include(x => x.FPTemplates);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.EmployeeId,
                             EmployeeNIK = model.Employee != null ? model.Employee.NIK : "",
                             EmployeeName = model.Employee != null ? model.Employee.Name : "",
                             model.PIN,
                             model.PIN2,
                             model.Privilege,
                             model.Name,
                             model.Password,
                             model.Card,
                             model.Group,
                             model.TimeZones,
                             model.VerifyMode,
                             model.IsEnabled,
                             model.IsInSync,
                             FPCount = model.FPTemplates.Where(x => !x.IsDeleted).Count(),
                             model.Reserved, //Reserved = BitConverter.ToString(model.Reserved),
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
                //rows = list,
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
                             model.PIN,
                             model.PIN2,
                             model.Privilege,
                             model.Name,
                             model.Password,
                             model.Card,
                             model.Group,
                             model.TimeZones,
                             model.VerifyMode,
                             model.IsEnabled,
                             model.IsInSync, //_fpTemplateService.IsUserInSync(model.Id),
                             model.FPCount,
                             model.Reserved,
                             model.Remark,
                             model.CreatedAt,
                             model.UpdatedAt,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            FPUser model = new FPUser();
            model.Errors = new Dictionary<string, string>();
            try
            {
                model = _fpUserService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                
                model.Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model.Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Id,
                model.EmployeeId,
                EmployeeNIK = model.Employee != null ? model.Employee.NIK : "",
                EmployeeName = model.Employee != null ? model.Employee.Name : "",
                model.PIN,
                model.PIN2,
                model.Privilege,
                model.Name,
                model.Password,
                model.Card,
                model.Group,
                model.TimeZones,
                model.VerifyMode,
                model.IsEnabled,
                model.Remark,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetDefaultInfo()
        {
            FPUser model = new FPUser();
            model.Errors = new Dictionary<string, string>();
            try
            {
                model = _fpUserService.GetQueryable().Where(x => !x.IsDeleted).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model.Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Id,
                model.EmployeeId,
                EmployeeNIK = model.Employee != null ? model.Employee.NIK : "",
                EmployeeName = model.Employee != null ? model.Employee.Name : "",
                model.PIN,
                model.PIN2,
                model.Privilege,
                model.Name,
                model.Password,
                model.Card,
                model.Group,
                model.TimeZones,
                model.VerifyMode,
                model.IsEnabled,
                model.Remark,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(FPUser model)
        {
            model.Errors = new Dictionary<string, string>();
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.FPUser, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    model.Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        model.Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model.IsInSync = false;
                model = _fpUserService.CreateObject(model, _employeeService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model.Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Update(FPUser model)
        {
            model.Errors = new Dictionary<string, string>();
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.FPUser, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    model.Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        model.Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _fpUserService.GetObjectById(model.Id);
                data.IsInSync = (data.PIN == model.PIN && data.Name == model.Name && data.Password == model.Password && data.Card == model.Card && data.Privilege == model.Privilege && data.Group == model.Group && data.TimeZones == model.TimeZones && data.VerifyMode == model.VerifyMode && data.IsEnabled == model.IsEnabled);
                data.EmployeeId = model.EmployeeId;
                data.PIN = model.PIN;
                data.PIN2 = model.PIN2;
                data.Name = model.Name;
                data.Password = model.Password;
                data.Card = model.Card;
                data.Privilege = model.Privilege;
                data.Group = model.Group;
                data.TimeZones = model.TimeZones;
                data.VerifyMode = model.VerifyMode;
                data.IsEnabled = model.IsEnabled;
                data.Remark = model.Remark;

                model = _fpUserService.UpdateObject(data, _employeeService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model.Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UpdateEnable(bool isEnabled, FPUser model)
        {
            model.Errors = new Dictionary<string, string>();
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.FPUser, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    model.Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        model.Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _fpUserService.GetObjectById(model.Id);
                data.IsInSync = (data.IsEnabled == model.IsEnabled);
                data.IsEnabled = isEnabled;
                model = _fpUserService.UpdateObject(data, _employeeService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model.Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(FPUser model)
        {
            model.Errors = new Dictionary<string, string>();
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.FPUser, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    model.Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        model.Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _fpUserService.GetObjectById(model.Id);
                data.IsInSync = false;
                model = _fpUserService.SoftDeleteObject(data);
            }

            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model.Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }
    }
}
