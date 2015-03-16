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
    
    public class WorkingTimeController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("WorkingTimeController");
        public IWorkingTimeService _workingTimeService;
        public IWorkingDayService _workingDayService;
        public IEmployeeWorkingTimeService _employeeWorkingTimeService;

        public WorkingTimeController()
        {
            _workingTimeService = new WorkingTimeService(new WorkingTimeRepository(), new WorkingTimeValidator());
            _workingDayService = new WorkingDayService(new WorkingDayRepository(), new WorkingDayValidator());
            _employeeWorkingTimeService = new EmployeeWorkingTimeService(new EmployeeWorkingTimeRepository(), new EmployeeWorkingTimeValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.WorkingTime, Core.Constants.Constant.MenuGroupName.Setting))
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
            var q = _workingTimeService.GetQueryable().Include("EmployeeWorkingTimes").Include("WorkingDays");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.Name,
                             model.MinCheckIn,
                             model.CheckIn,
                             model.MaxCheckIn,
                             model.BreakOut,
                             model.BreakIn,
                             model.MinCheckOut,
                             model.CheckOut,
                             model.MaxCheckOut,
                             model.CheckInTolerance,
                             model.CheckOutTolerance,
                             model.WorkInterval,
                             model.BreakInterval,
                             model.TimeZone,
                             model.TimeZoneOffset,
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
                            model.Code,
                            model.Name,
                            model.MinCheckIn,
                            model.CheckIn,
                            model.MaxCheckIn,
                            model.BreakOut,
                            model.BreakIn,
                            model.MinCheckOut,
                            model.CheckOut,
                            model.MaxCheckOut,
                            model.CheckInTolerance,
                            model.CheckOutTolerance,
                            model.WorkInterval,
                            model.BreakInterval,
                            model.TimeZone,
                            model.TimeZoneOffset,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            WorkingTime model = new WorkingTime();
            try
            {
                model = _workingTimeService.GetObjectById(Id);
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
                model.Code,
                model.Name,
                model.MinCheckIn,
                model.CheckIn,
                model.MaxCheckIn,
                model.BreakOut,
                model.BreakIn,
                model.MinCheckOut,
                model.CheckOut,
                model.MaxCheckOut,
                model.CheckInTolerance,
                model.CheckOutTolerance,
                model.WorkInterval,
                model.BreakInterval,
                model.TimeZone,
                model.TimeZoneOffset,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetDefaultInfo()
        {
            WorkingTime model = new WorkingTime();
            try
            {
                model = _workingTimeService.GetQueryable().FirstOrDefault();
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
                model.Code,
                model.Name,
                model.MinCheckIn,
                model.CheckIn,
                model.MaxCheckIn,
                model.BreakOut,
                model.BreakIn,
                model.MinCheckOut,
                model.CheckOut,
                model.MaxCheckOut,
                model.CheckInTolerance,
                model.CheckOutTolerance,
                model.WorkInterval,
                model.BreakInterval,
                model.TimeZone,
                model.TimeZoneOffset,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(WorkingTime model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.WorkingTime, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }
                string winTZ = model.TimeZone.ToUpper(); // FPDevice.Convertion.IanaToWindows(fpMachine.TimeZone);
                TimeZoneInfo destTZ = TimeZoneInfo.GetSystemTimeZones().Where(x => x.Id.ToUpper() == winTZ).FirstOrDefault();
                model.MinCheckIn = _workingTimeService.SetTimeZone(model.MinCheckIn, destTZ, model.TimeZoneOffset);
                model.CheckIn = _workingTimeService.SetTimeZone(model.CheckIn, destTZ, model.TimeZoneOffset);
                model.MaxCheckIn = _workingTimeService.SetTimeZone(model.MaxCheckIn, destTZ, model.TimeZoneOffset);
                model.BreakOut = _workingTimeService.SetTimeZone(model.BreakOut, destTZ, model.TimeZoneOffset);
                model.BreakIn = _workingTimeService.SetTimeZone(model.BreakIn, destTZ, model.TimeZoneOffset);
                model.MinCheckOut = _workingTimeService.SetTimeZone(model.MinCheckOut, destTZ, model.TimeZoneOffset);
                model.CheckOut = _workingTimeService.SetTimeZone(model.CheckOut, destTZ, model.TimeZoneOffset);
                model.MaxCheckOut = _workingTimeService.SetTimeZone(model.MaxCheckOut, destTZ, model.TimeZoneOffset);
                model = _workingTimeService.CreateObject(model, _workingDayService);
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
        public dynamic Update(WorkingTime model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.WorkingTime, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _workingTimeService.GetObjectById(model.Id);
                data.Code = model.Code;
                data.Name = model.Name;
                data.TimeZone = model.TimeZone;
                data.TimeZoneOffset = model.TimeZoneOffset;
                string winTZ = model.TimeZone.ToUpper(); // FPDevice.Convertion.IanaToWindows(fpMachine.TimeZone);
                TimeZoneInfo destTZ = TimeZoneInfo.GetSystemTimeZones().Where(x => x.Id.ToUpper() == winTZ).FirstOrDefault();
                data.MinCheckIn = _workingTimeService.SetTimeZone(model.MinCheckIn, destTZ, model.TimeZoneOffset);
                data.CheckIn = _workingTimeService.SetTimeZone(model.CheckIn, destTZ, model.TimeZoneOffset);
                data.MaxCheckIn = _workingTimeService.SetTimeZone(model.MaxCheckIn, destTZ, model.TimeZoneOffset);
                data.BreakOut = _workingTimeService.SetTimeZone(model.BreakOut, destTZ, model.TimeZoneOffset);
                data.BreakIn = _workingTimeService.SetTimeZone(model.BreakIn, destTZ, model.TimeZoneOffset);
                data.MinCheckOut = _workingTimeService.SetTimeZone(model.MinCheckOut, destTZ, model.TimeZoneOffset);
                data.CheckOut = _workingTimeService.SetTimeZone(model.CheckOut, destTZ, model.TimeZoneOffset);
                data.MaxCheckOut = _workingTimeService.SetTimeZone(model.MaxCheckOut, destTZ, model.TimeZoneOffset);
                data.CheckInTolerance = model.CheckInTolerance;
                data.CheckOutTolerance = model.CheckOutTolerance;
                data.WorkInterval = model.WorkInterval;
                data.BreakInterval = model.BreakInterval;
                
                model = _workingTimeService.UpdateObject(data, _workingDayService);
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
        public dynamic Delete(WorkingTime model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.WorkingTime, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _workingTimeService.GetObjectById(model.Id);
                model = _workingTimeService.SoftDeleteObject(data, _employeeWorkingTimeService);
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
