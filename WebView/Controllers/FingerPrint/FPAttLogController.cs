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
using Newtonsoft.Json;

namespace WebView.Controllers
{
    
    public class FPAttLogController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("FPAttLogController");
        public IFPUserService _fpUserService;
        public IFPAttLogService _fpAttLogService;
        public IEmployeeService _employeeService;

        public FPAttLogController()
        {
            _fpUserService = new FPUserService(new FPUserRepository(), new FPUserValidator());
            _fpAttLogService = new FPAttLogService(new FPAttLogRepository(), new FPAttLogValidator());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.FPAttLog, Core.Constants.Constant.MenuGroupName.Setting))
            {
                return Content(Core.Constants.Constant.ErrorPage.PageViewNotAllowed);
            }

            return View(this);
        }

        public dynamic GetListFPAttLog(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _fpAttLogService.GetQueryable().Include(x => x.FPUser).Where(x => x.FPUserId == id).ToList();

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.PIN,
                             model.PIN2,
                             model.Time_second,
                             model.DeviceID,
                             model.VerifyMode,
                             model.InOutMode,
                             model.WorkCode,
                             model.Reserved,
                             model.CreatedAt,
                             model.UpdatedAt,
                         }).AsQueryable().Where(filter).OrderBy(sidx + " " + sord); //.ToList();

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
                             model.PIN,
                             model.PIN2,
                             model.Time_second,
                             model.DeviceID,
                             model.VerifyMode,
                             model.InOutMode,
                             model.WorkCode,
                             model.Reserved,
                             model.CreatedAt,
                             model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        //public dynamic GetListFPUser(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "", int ParentId = 0)
        //{
        //    // Construct where statement
        //    string strWhere = GeneralFunction.ConstructWhere(filters);
        //    string filter = null;
        //    GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
        //    if (filter == "") filter = "true";

        //    // Get Data
        //    var q = _fpUserService.GetQueryable().Include("EmployeeFPUsers").Include("FPAttLogs");

        //    var query = (from model in q
        //                 select new
        //                 {
        //                     model.Id,
        //                     model.Code,
        //                     model.Name,
        //                     model.MinCheckIn,
        //                     model.CheckIn,
        //                     model.MaxCheckIn,
        //                     model.BreakOut,
        //                     model.BreakIn,
        //                     model.MinCheckOut,
        //                     model.CheckOut,
        //                     model.MaxCheckOut,
        //                     model.CheckInTolerance,
        //                     model.CheckOutTolerance,
        //                     model.WorkInterval,
        //                     model.BreakInterval,
        //                     model.CreatedAt,
        //                     model.UpdatedAt,
        //                 }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

        //    var list = query.AsEnumerable();

        //    var pageIndex = Convert.ToInt32(page) - 1;
        //    var pageSize = rows;
        //    var totalRecords = query.Count();
        //    var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
        //    // default last page
        //    if (totalPages > 0)
        //    {
        //        if (!page.HasValue)
        //        {
        //            pageIndex = totalPages - 1;
        //            page = totalPages;
        //        }
        //    }

        //    list = list.Skip(pageIndex * pageSize).Take(pageSize);

        //    return Json(new
        //    {
        //        total = totalPages,
        //        page = page,
        //        records = totalRecords,
        //        rows = (
        //            from model in list
        //            select new
        //            {
        //                id = model.Id,
        //                cell = new object[] {
        //                    model.Id,
        //                    model.Code,
        //                    model.Name,
        //                    model.MinCheckIn,
        //                    model.CheckIn,
        //                    model.MaxCheckIn,
        //                    model.BreakOut,
        //                    model.BreakIn,
        //                    model.MinCheckOut,
        //                    model.CheckOut,
        //                    model.MaxCheckOut,
        //                    model.CheckInTolerance,
        //                    model.CheckOutTolerance,
        //                    model.WorkInterval,
        //                    model.BreakInterval,
        //                    model.CreatedAt,
        //                    model.UpdatedAt,
        //              }
        //            }).ToArray()
        //    }, JsonRequestBehavior.AllowGet);
        //}


        public dynamic GetInfo(int Id)
        {
            FPAttLog model = new FPAttLog();
            try
            {
                model = _fpAttLogService.GetObjectById(Id);
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
                model.FPUserId,
                FPUser = model.FPUser.Name,
                model.PIN,
                model.PIN2,
                model.Time_second,
                model.DeviceID,
                model.VerifyMode,
                model.InOutMode,
                model.WorkCode,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetDefaultInfo()
        {
            FPAttLog model = new FPAttLog();
            try
            {
                model = _fpAttLogService.GetQueryable().FirstOrDefault();
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
                model.FPUserId,
                FPUser = model.FPUser.Name,
                model.PIN,
                model.PIN2,
                model.Time_second,
                model.DeviceID,
                model.VerifyMode,
                model.InOutMode,
                model.WorkCode,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(FPAttLog model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.FPAttLog, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _fpAttLogService.CreateObject(model, _fpUserService);
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
        public dynamic Update(FPAttLog model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.FPAttLog, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _fpAttLogService.GetObjectById(model.Id);
                data.FPUserId = model.FPUserId;
                data.PIN = model.PIN;
                data.PIN2 = model.PIN2;
                data.Time_second = model.Time_second;
                data.DeviceID = model.DeviceID;
                data.InOutMode = model.InOutMode;
                data.VerifyMode = model.VerifyMode;
                data.WorkCode = model.WorkCode;
                model = _fpAttLogService.UpdateObject(data, _fpUserService);
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
        public dynamic Delete(FPAttLog model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.FPAttLog, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _fpAttLogService.GetObjectById(model.Id);
                model = _fpAttLogService.SoftDeleteObject(data);
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
