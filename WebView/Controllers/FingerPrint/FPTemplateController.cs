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
using System.ComponentModel;

namespace WebView.Controllers
{
    
    public class FPTemplateController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("FPTemplateController");
        public ICompanyInfoService _companyInfoService;
        public IEmployeeService _employeeService;
        public IFPUserService _fpUserService;
        public IFPTemplateService _fpTemplateService;
        public IFPAttLogService _fpAttLogService;

        public FPTemplateController()
        {
            _companyInfoService = new CompanyInfoService(new CompanyInfoRepository(), new CompanyInfoValidator());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
            _fpUserService = new FPUserService(new FPUserRepository(), new FPUserValidator());
            _fpTemplateService = new FPTemplateService(new FPTemplateRepository(), new FPTemplateValidator());
            _fpAttLogService = new FPAttLogService(new FPAttLogRepository(), new FPAttLogValidator());
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
            var q = _fpTemplateService.GetQueryable().Where(x => (x.FPUserId == ParentId || ParentId == 0) && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.PIN,
                             model.FPUserId,
                             FPUserPIN = model.FPUser.PIN,
                             FPUser = model.FPUser.Name,
                             model.FingerID,
                             model.Valid,
                             model.IsInSync,
                             model.Size,
                             model.Template,
                             model.CreatedAt,
                             model.UpdatedAt,
                         }).Where(filter).OrderBy(sidx + " " + sord);

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
                             model.PIN,
                             model.FPUserId,
                             model.FPUserPIN,
                             model.FPUser,
                             model.FingerID,
                             model.Valid,
                             model.IsInSync,
                             model.Size,
                             model.Template,
                             model.CreatedAt,
                             model.UpdatedAt,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            FPTemplate model = new FPTemplate();
            model.Errors = new Dictionary<string, string>();
            try
            {
                model = _fpTemplateService.GetObjectById(Id);
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
                model.PIN,
                model.FPUserId,
                FPUserPIN = model.FPUser.PIN,
                FPUser = model.FPUser.Name,
                model.FingerID,
                model.Valid,
                model.Size,
                model.Template,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetDefaultInfo()
        {
            FPTemplate model = new FPTemplate();
            model.Errors = new Dictionary<string, string>();
            try
            {
                model = _fpTemplateService.GetQueryable().FirstOrDefault();
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
                model.PIN,
                model.FPUserId,
                FPUserPIN = model.FPUser.PIN,
                FPUser = model.FPUser.Name,
                model.FingerID,
                model.Valid,
                model.Size,
                model.Template,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(FPTemplate model)
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

                model.Size = model.Template.Length;
                model.IsInSync = false;
                model = _fpTemplateService.CreateObject(model, _fpUserService);
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
        public dynamic Update(FPTemplate model)
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

                var data = _fpTemplateService.GetObjectById(model.Id);
                data.FPUserId = model.FPUserId;
                data.PIN = model.PIN;
                data.FingerID = model.FingerID;
                data.Valid = model.Valid;
                data.Template = model.Template;
                data.Size = model.Template.Length; //model.Size;
                data.IsInSync = false;

                model = _fpTemplateService.UpdateObject(data, _fpUserService);
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
        public dynamic Delete(FPTemplate model)
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

                var data = _fpTemplateService.GetObjectById(model.Id);
                data.IsInSync = false;
                model = _fpTemplateService.SoftDeleteObject(data, _fpUserService);
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
