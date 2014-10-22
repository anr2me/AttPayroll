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
    
    public class SalaryEmployeeController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("SalaryEmployeeController");
        public ISalaryEmployeeService _salaryEmployeeService;
        public IEmployeeService _employeeService;
        public ISalaryEmployeeDetailService _salaryEmployeeDetailService;
        public ISalaryItemService _salaryItemService;
        public ISalaryStandardDetailService _salaryStandardDetailService;

        public SalaryEmployeeController()
        {
            _salaryEmployeeService = new SalaryEmployeeService(new SalaryEmployeeRepository(), new SalaryEmployeeValidator());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
            _salaryEmployeeDetailService = new SalaryEmployeeDetailService(new SalaryEmployeeDetailRepository(), new SalaryEmployeeDetailValidator());
            _salaryItemService = new SalaryItemService(new SalaryItemRepository(), new SalaryItemValidator());
            _salaryStandardDetailService = new SalaryStandardDetailService(new SalaryStandardDetailRepository(), new SalaryStandardDetailValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.EmployeeSalary, Core.Constants.Constant.MenuGroupName.Setting))
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
            var q = _salaryEmployeeService.GetQueryable().Include("Employee").Include("SalaryEmployeeDetails").Include("TitleInfo");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.EmployeeId,
                             EmployeeNIK = model.Employee.NIK,
                             EmployeeName = model.Employee.Name,
                             TitleInfoName = model.Employee.TitleInfo.Name,
                             model.EffectiveDate,
                             model.Description,
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
                             model.TitleInfoName,
                             model.EffectiveDate,
                             model.Description,
                             model.CreatedAt,
                             model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            SalaryEmployee model = new SalaryEmployee();
            try
            {
                model = _salaryEmployeeService.GetObjectById(Id);
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
                model.EffectiveDate,
                model.Description,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetDefaultInfo()
        {
            SalaryEmployee model = new SalaryEmployee();
            try
            {
                model = _salaryEmployeeService.GetQueryable().FirstOrDefault();
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
                model.EffectiveDate,
                model.Description,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(SalaryEmployee model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.EmployeeSalary, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _salaryEmployeeService.CreateObject(model, _employeeService, _salaryEmployeeDetailService, _salaryItemService, _salaryStandardDetailService);
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
        public dynamic Update(SalaryEmployee model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.EmployeeSalary, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salaryEmployeeService.GetObjectById(model.Id);
                data.EmployeeId = model.EmployeeId;
                data.EffectiveDate = model.EffectiveDate;
                data.Description = model.Description;
                model = _salaryEmployeeService.UpdateObject(data, _employeeService);
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
        public dynamic Delete(SalaryEmployee model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.EmployeeSalary, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salaryEmployeeService.GetObjectById(model.Id);
                model = _salaryEmployeeService.SoftDeleteObject(data);
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


        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _salaryEmployeeDetailService.GetQueryable().Include("SalaryEmployee").Include("SalaryItem").Where(x => x.SalaryEmployeeId == id);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.SalaryEmployeeId,
                             model.SalaryItemId,
                             SalaryItemCode = model.SalaryItem.Code,
                             SalaryItemName = model.SalaryItem.Name,
                             model.Amount,
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
                             model.SalaryEmployeeId,
                             model.SalaryItemId,
                             model.SalaryItemCode,
                             model.SalaryItemName,
                             model.Amount,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            SalaryEmployeeDetail model = new SalaryEmployeeDetail();
            try
            {
                model = _salaryEmployeeDetailService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors = new Dictionary<string, string>();
                model.Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model.Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Id,
                model.SalaryEmployeeId,
                model.SalaryItemId,
                SalaryItemCode = model.SalaryItem.Code,
                SalaryItemName = model.SalaryItem.Name,
                model.Amount,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic InsertDetail(SalaryEmployeeDetail model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.EmployeeSalary, Core.Constants.Constant.MenuGroupName.Utility))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }
                model = _salaryEmployeeDetailService.CreateObject(model, _salaryEmployeeService, _salaryItemService);
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
                model.Errors,
            });
        }

        [HttpPost]
        public dynamic UpdateDetail(SalaryEmployeeDetail model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.EmployeeSalary, Core.Constants.Constant.MenuGroupName.Utility))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salaryEmployeeDetailService.GetObjectById(model.Id);
                data.SalaryItemId = model.SalaryItemId;
                data.Amount = model.Amount;
                data.SalaryEmployeeId = model.SalaryEmployeeId;
                model = _salaryEmployeeDetailService.UpdateObject(data, _salaryEmployeeService, _salaryItemService);
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
                model.Errors,
            });
        }

        [HttpPost]
        public dynamic DeleteDetail(SalaryEmployeeDetail model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.EmployeeSalary, Core.Constants.Constant.MenuGroupName.Utility))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salaryEmployeeDetailService.GetObjectById(model.Id);
                model = _salaryEmployeeDetailService.SoftDeleteObject(data);
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
                model.Errors,
            });
        }


    }
}
