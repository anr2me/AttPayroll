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
using System.Data.Objects.SqlClient;

namespace WebView.Controllers
{
    
    public class SalarySlipController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("SalarySlipController");
        public ISalarySlipService _salarySlipService;
        public IFormulaService _formulaService;
        public ISalarySlipDetailService _salarySlipDetailService;
        public ISalaryItemService _salaryItemService;

        public SalarySlipController()
        {
            _salarySlipService = new SalarySlipService(new SalarySlipRepository(), new SalarySlipValidator());
            _formulaService = new FormulaService(new FormulaRepository(), new FormulaValidator());
            _salarySlipDetailService = new SalarySlipDetailService(new SalarySlipDetailRepository(), new SalarySlipDetailValidator());
            _salaryItemService = new SalaryItemService(new SalaryItemRepository(), new SalaryItemValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.SalarySlip, Core.Constants.Constant.MenuGroupName.Master))
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
            var q = _salarySlipService.GetQueryable().Include("SalaryItem").Include("SalarySlipDetails");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Index,
                             model.Code,
                             model.Name,
                             model.SalaryItemId,
                             SalarySign = model.SalarySign,
                             model.IsEnabled,
                             model.IsMainSalary,
                             model.IsDetailSalary,
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
                             model.Index,
                             model.Code,
                             model.Name,
                             model.SalaryItemId,
                             model.SalarySign,
                             model.IsEnabled,
                             model.IsMainSalary,
                             model.IsDetailSalary,
                             model.CreatedAt,
                             model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            SalarySlip model = new SalarySlip();
            try
            {
                model = _salarySlipService.GetObjectById(Id);
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
                model.Index,
                model.Code,
                model.Name,
                model.SalaryItemId,
                model.SalarySign,
                model.IsEnabled,
                model.IsMainSalary,
                model.IsDetailSalary,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetDefaultInfo()
        {
            SalarySlip model = new SalarySlip();
            try
            {
                model = _salarySlipService.GetQueryable().FirstOrDefault();
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
                model.Index,
                model.Code,
                model.Name,
                model.SalaryItemId,
                model.SalarySign,
                model.IsEnabled,
                model.IsMainSalary,
                model.IsDetailSalary,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(SalarySlip model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.SalarySlip, Core.Constants.Constant.MenuGroupName.Master))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _salarySlipService.CreateObject(model, _salaryItemService);
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
        public dynamic Update(SalarySlip model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.SalarySlip, Core.Constants.Constant.MenuGroupName.Master))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salarySlipService.GetObjectById(model.Id);
                data.Index = model.Index;
                data.Code = model.Code;
                data.Name = model.Name;
                //data.SalaryItemId = model.SalaryItemId;
                data.SalarySign = model.SalarySign;
                data.IsEnabled = model.IsEnabled;
                data.IsMainSalary = model.IsMainSalary;
                data.IsDetailSalary = model.IsDetailSalary;
                model = _salarySlipService.UpdateObject(data, _salaryItemService);
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
        public dynamic UpdateEnable(bool isEnabled, bool isMain, bool isDetail, SalarySlip model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.SalarySlip, Core.Constants.Constant.MenuGroupName.Master))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salarySlipService.GetObjectById(model.Id);
                data.IsEnabled = isEnabled;
                data.IsMainSalary = isMain;
                data.IsDetailSalary = isDetail;
                model = _salarySlipService.UpdateObject(data, _salaryItemService);
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
        public dynamic Delete(SalarySlip model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.SalarySlip, Core.Constants.Constant.MenuGroupName.Master))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salarySlipService.GetObjectById(model.Id);
                model = _salarySlipService.SoftDeleteObject(data, _salaryItemService);
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
            var q = _salarySlipDetailService.GetQueryable().Include("SalarySlip").Include("Formula").Include("SalaryItem").Where(x => x.SalarySlipId == id);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Index,
                             model.SalarySlipId,
                             model.SalarySign,
                             model.FormulaId,
                             FirstCode = model.Formula.FirstSalaryItem.Code,
                             model.Formula.FormulaOp,
                             SecondCode = model.Formula.IsSecondValue ? SqlFunctions.StringConvert(model.Formula.SecondValue,20,4).Trim() : model.Formula.SecondSalaryItem.Code,
                             model.HasMinValue,
                             model.MinValue,
                             model.HasMaxValue,
                             model.MaxValue,
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
                             model.Index,
                             model.SalarySlipId,
                             model.SalarySign,
                             model.FormulaId,
                             model.FirstCode,
                             model.FormulaOp,
                             model.SecondCode,
                             model.HasMinValue,
                             model.MinValue,
                             model.HasMaxValue,
                             model.MaxValue,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            SalarySlipDetail model = new SalarySlipDetail();
            try
            {
                model = _salarySlipDetailService.GetObjectById(Id);
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
                model.Index,
                model.SalarySlipId,
                model.SalarySign,
                model.FormulaId,
                FirstCode = model.Formula.FirstSalaryItem.Code,
                model.Formula.FormulaOp,
                SecondCode = model.Formula.IsSecondValue ? model.Formula.SecondValue.ToString() : model.Formula.SecondSalaryItem.Code,
                model.Formula.IsSecondValue,
                model.Formula.SecondValue,
                model.HasMinValue,
                model.MinValue,
                model.HasMaxValue,
                model.MaxValue,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic InsertDetail(int FirstSalaryItem, string FormulaOp, int SecondSalaryItem, bool IsSecondValue, Nullable<decimal> SecondValue, SalarySlipDetail model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.SalarySlip, Core.Constants.Constant.MenuGroupName.Utility))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }
                Formula formula = new Formula
                {
                    FirstSalaryItemId = FirstSalaryItem,
                    FormulaOp = FormulaOp,
                    IsSecondValue = IsSecondValue,
                    SecondValue = SecondValue.GetValueOrDefault(),
                    SecondSalaryItemId = SecondSalaryItem,
                };
                _formulaService.CreateObject(formula, _salaryItemService);
                model.FormulaId = formula.Id;
                model = _salarySlipDetailService.CreateObject(model, _salarySlipService, _formulaService);
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
        public dynamic UpdateDetail(int FirstSalaryItem, string FormulaOp, int SecondSalaryItem, bool IsSecondValue, Nullable<decimal> SecondValue, SalarySlipDetail model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.SalarySlip, Core.Constants.Constant.MenuGroupName.Utility))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salarySlipDetailService.GetObjectById(model.Id);
                Formula formula = _formulaService.GetObjectById(data.FormulaId);
                if (formula != null)
                {
                    formula.FirstSalaryItemId = FirstSalaryItem;
                    formula.FormulaOp = FormulaOp;
                    formula.IsSecondValue = IsSecondValue;
                    formula.SecondValue = SecondValue.GetValueOrDefault();
                    formula.SecondSalaryItemId = SecondSalaryItem;
                }
                _formulaService.UpdateObject(formula, _salaryItemService);

                data.Index = model.Index;
                data.SalarySlipId = model.SalarySlipId;
                data.SalarySign = model.SalarySign;
                data.HasMinValue = model.HasMinValue;
                data.MinValue = model.MinValue;
                data.HasMaxValue = model.HasMaxValue;
                data.MaxValue = model.MaxValue;
                model = _salarySlipDetailService.UpdateObject(data, _salarySlipService, _formulaService);
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
        public dynamic DeleteDetail(SalarySlipDetail model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.SalarySlip, Core.Constants.Constant.MenuGroupName.Master))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salarySlipDetailService.GetObjectById(model.Id);
                model = _salarySlipDetailService.SoftDeleteObject(data);
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
