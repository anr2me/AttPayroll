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
using zkemkeeper;
using FPDevice;
using WebView.Hubs;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR;
using System.Timers;
using System.Reflection;

namespace WebView.Controllers
{
    
    public class FPMachineController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("FPMachineController");
        public IFPMachineService _fpMachineService;
        public ICompanyInfoService _companyInfoService;
        public IEmployeeService _employeeService;

        public IFPUserService _fpUserService = new FPUserService(new FPUserRepository(), new FPUserValidator());
        public IFPTemplateService _fpTemplateService = new FPTemplateService(new FPTemplateRepository(), new FPTemplateValidator());
        public IFPAttLogService _fpAttLogService = new FPAttLogService(new FPAttLogRepository(), new FPAttLogValidator());

        public JsonSerializerSettings JSONsettings = new JsonSerializerSettings()
        {
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
        };

        public FPMachineController()
        {
            _fpMachineService = new FPMachineService(new FPMachineRepository(), new FPMachineValidator());
            _companyInfoService = new CompanyInfoService(new CompanyInfoRepository(), new CompanyInfoValidator());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
            //_fpUserService = new FPUserService(new FPUserRepository(), new FPUserValidator());
            //_fpTemplateService = new FPTemplateService(new FPTemplateRepository(), new FPTemplateValidator());
            //_fpAttLogService = new FPAttLogService(new FPAttLogRepository(), new FPAttLogValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.FPMachine, Core.Constants.Constant.MenuGroupName.Setting))
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
            var q = _fpMachineService.GetQueryable().Where(x => !x.IsDeleted).ToList();
            foreach (var model in q)
            {
                model.IsConnected = _fpMachineService.IsConnected(model.Id);
                if (model.IsConnected) _fpMachineService.RefreshObject(model, _fpUserService, _fpTemplateService);
            }

            var query = (from model in (q.AsQueryable())
                         select new
                         {
                             model.Id,
                             model.CompanyInfoId,
                             CompanyInfo = model.CompanyInfo.Name,
                             model.MachineNumber,
                             model.MachineName,
                             model.Password,
                             model.IsAutoConnect,
                             model.IsConnected,
                             model.IsInSync,
                             model.IsClearLogAfterDownload,
                             model.CommType,
                             model.EthernetIP,
                             model.EthernetPort,
                             model.EthernetMAC,
                             model.SerialPort,
                             model.SerialBaudRate,
                             model.TimeZone,
                             //model.ProductName,
                             model.Platform,
                             model.FirmwareVer,
                             model.ArithmeticVer,
                             model.SerialNumber,
                             model.UserCount,
                             model.AdminCount,
                             model.PasswordCount,
                             model.FPCount,
                             model.FCCount,
                             model.AttLogCount,
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
                             model.CompanyInfoId,
                             model.CompanyInfo,
                             model.MachineNumber,
                             model.MachineName,
                             model.Password,
                             model.IsAutoConnect,
                             _fpMachineService.IsConnected(model.Id), //model.IsConnected,
                             _fpMachineService.IsInSync(model.Id), //model.IsInSync,
                             model.IsClearLogAfterDownload,
                             model.CommType,
                             model.EthernetIP,
                             model.EthernetPort,
                             model.EthernetMAC,
                             model.SerialPort,
                             model.SerialBaudRate,
                             model.TimeZone,
                             //model.ProductName,
                             model.Platform,
                             model.FirmwareVer,
                             model.ArithmeticVer,
                             model.SerialNumber,
                             model.UserCount,
                             model.AdminCount,
                             model.PasswordCount,
                             model.FPCount,
                             model.FCCount,
                             model.AttLogCount,
                             model.CreatedAt,
                             model.UpdatedAt,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            FPMachine model = new FPMachine();
            model.Errors = new Dictionary<string, string>();
            try
            {
                model = _fpMachineService.GetObjectById(Id);
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
                model.Id,
                model.CompanyInfoId,
                CompanyInfo = model.CompanyInfo != null ? model.CompanyInfo.Name : "",
                model.MachineNumber,
                model.MachineName,
                model.Password,
                model.IsAutoConnect,
                model.IsClearLogAfterDownload,
                model.CommType,
                model.EthernetIP,
                model.EthernetPort,
                model.SerialPort,
                model.SerialBaudRate,
                model.TimeZone,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetDefaultInfo()
        {
            FPMachine model = new FPMachine();
            model.Errors = new Dictionary<string, string>();
            try
            {
                model = _fpMachineService.GetQueryable().FirstOrDefault();
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
                model.Id,
                model.CompanyInfoId,
                CompanyInfo = model.CompanyInfo != null ? model.CompanyInfo.Name : "",
                model.MachineNumber,
                model.MachineName,
                model.Password,
                model.IsAutoConnect,
                model.IsClearLogAfterDownload,
                model.CommType,
                model.EthernetIP,
                model.EthernetPort,
                model.SerialPort,
                model.SerialBaudRate,
                model.TimeZone,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(FPMachine model)
        {
            model.Errors = new Dictionary<string, string>();
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.FPMachine, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    model.Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        model
                    }, JsonRequestBehavior.AllowGet);
                }

                //model.TimeZone = GeneralFunction.IanaToWindows(model.TimeZone);
                model = _fpMachineService.CreateObject(model, _companyInfoService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model
            });
        }

        [HttpPost]
        public dynamic Update(FPMachine model)
        {
            model.Errors = new Dictionary<string, string>();
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.FPMachine, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    model.Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        model
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _fpMachineService.GetObjectById(model.Id);
                data.CompanyInfoId = model.CompanyInfoId;
                data.MachineName = model.MachineName;
                if (!data.IsConnected) data.MachineNumber = model.MachineNumber;
                data.Password = model.Password;
                data.CommType = model.CommType;
                data.EthernetIP = model.EthernetIP;
                data.EthernetPort = model.EthernetPort;
                data.SerialPort = model.SerialPort;
                data.SerialBaudRate = model.SerialBaudRate;
                data.IsClearLogAfterDownload = model.IsClearLogAfterDownload;
                data.TimeZone = model.TimeZone; // GeneralFunction.IanaToWindows(model.TimeZone);

                model = _fpMachineService.UpdateObject(data, _companyInfoService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model
            });
        }

        [HttpPost]
        public dynamic UpdateEnable(bool isAutoConnect, FPMachine model)
        {
            model.Errors = new Dictionary<string, string>();
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.FPMachine, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    model.Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        model
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _fpMachineService.GetObjectById(model.Id);
                data.IsAutoConnect = isAutoConnect;
                model = _fpMachineService.UpdateObject(data, _companyInfoService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model
            });
        }

        [HttpPost]
        public dynamic Delete(FPMachine model)
        {
            model.Errors = new Dictionary<string, string>();
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.FPMachine, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    model.Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        model
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _fpMachineService.GetObjectById(model.Id);
                model = _fpMachineService.SoftDeleteObject(data);
            }

            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model
            });
        }

        public dynamic Connect(int Id)
        {
            FPMachine model = new FPMachine();
            model.Errors = new Dictionary<string, string>();
            try
            {
                model = _fpMachineService.GetObjectById(Id);
                if (FPMachines.fpDevices[model.Id] == null)
                {
                    _fpMachineService.AddMachine(model.Id);
                    FPMachines.fpDevices[model.Id].OnAttTransactionEx = OnAttTransactionEx;
                    FPMachines.fpDevices[model.Id].OnDeleteTemplate = OnDeleteTemplate;
                    FPMachines.fpDevices[model.Id].OnEnrollFinger = OnEnrollFinger;
                    FPMachines.fpDevices[model.Id].OnConnected = OnConnected;
                    FPMachines.fpDevices[model.Id].OnDisConnected = OnDisConnected;
                    FPMachines.fpDevices[model.Id].OnEMData = OnEMData;
                }
                model = _fpMachineService.ConnectObject(model);
            }
            catch (Exception ex)
            {
                LOG.Error("Connect", ex);

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

        public dynamic Disconnect(int Id)
        {
            FPMachine model = new FPMachine();
            model.Errors = new Dictionary<string, string>();
            try
            {
                model = _fpMachineService.GetObjectById(Id);
                model = _fpMachineService.DisconnectObject(model);
            }
            catch (Exception ex)
            {
                LOG.Error("Disconnect", ex);

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

        public dynamic CheckSync(int Id)
        {
            FPMachine model = new FPMachine();
            model.Errors = new Dictionary<string, string>();
            try
            {
                model = _fpMachineService.GetObjectById(Id);
                model.IsInSync = _fpMachineService.IsInSync(model.Id);
            }
            catch (Exception ex)
            {
                LOG.Error("CheckSync", ex);

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

        public dynamic DownloadLog(int Id)
        {
            var model = new FPMachine();
            model.Errors = new Dictionary<string, string>();
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.FPUser, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    model.Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        model
                    }, JsonRequestBehavior.AllowGet);
                }
                model = _fpMachineService.GetObjectById(Id);
                _fpMachineService.DownloadAttLog(model, model.IsClearLogAfterDownload, _fpUserService, _fpAttLogService);
            }
            catch (Exception ex)
            {
                LOG.Error("Download Failed", ex);
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

        public dynamic Download(int Id)
        {
            var model = new FPMachine();
            model.Errors = new Dictionary<string, string>();
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.FPUser, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    model.Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        model
                    }, JsonRequestBehavior.AllowGet);
                }
                model = _fpMachineService.GetObjectById(Id);
                _fpMachineService.DownloadAllUserData(model, _fpUserService, _fpTemplateService, _employeeService);
            }
            catch (Exception ex)
            {
                LOG.Error("Download Failed", ex);
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

        public dynamic Upload(int Id)
        {
            var model = new FPMachine();
            model.Errors = new Dictionary<string, string>();
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.FPUser, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    model.Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        model
                    }, JsonRequestBehavior.AllowGet);
                }
                model = _fpMachineService.GetObjectById(Id);
                _fpMachineService.UploadAllUserData(model, true, true, _fpUserService, _fpTemplateService, _employeeService);
            }
            catch (Exception ex)
            {
                LOG.Error("Upload Failed", ex);
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

        public dynamic Sync(int Id)
        {
            var model = new FPMachine();
            model.Errors = new Dictionary<string, string>();
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.FPUser, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    model.Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        model
                    }, JsonRequestBehavior.AllowGet);
                }
                model = _fpMachineService.GetObjectById(Id);
                _fpMachineService.UploadAllUserData(model, false, true, _fpUserService, _fpTemplateService, _employeeService);
                if (!model.Errors.Any())
                {
                    _fpMachineService.DownloadAllUserData(model, _fpUserService, _fpTemplateService, _employeeService);
                }
            }
            catch (Exception ex)
            {
                LOG.Error("Sync Failed", ex);
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

        #region FingerPrint Events Handler
        public void OnConnected(int iMachineNumber, int Tag)
        {
            try
            {
                var fpMachine = _fpMachineService.GetObjectById(Tag); //GetObjectByMachineNumber(iMachineNumber);
                if (fpMachine != null)
                {
                    fpMachine.IsConnected = true;
                    _fpMachineService.RefreshObject(fpMachine, _fpUserService, _fpTemplateService);

                    //Notify clients
                    dynamic model = new
                    {
                        fpMachine.Id,
                        fpMachine.CompanyInfoId,
                        CompanyInfo = fpMachine.CompanyInfo.Name,
                        fpMachine.MachineNumber,
                        fpMachine.MachineName,
                        fpMachine.Password,
                        fpMachine.IsAutoConnect,
                        fpMachine.IsConnected,
                        fpMachine.IsInSync,
                        fpMachine.CommType,
                        fpMachine.EthernetIP,
                        fpMachine.EthernetPort,
                        fpMachine.EthernetMAC,
                        fpMachine.SerialPort,
                        fpMachine.SerialBaudRate,
                        fpMachine.TimeZone,
                        //fpMachine.ProductName,
                        fpMachine.Platform,
                        fpMachine.SerialNumber,
                        fpMachine.UserCount,
                        fpMachine.AdminCount,
                        fpMachine.PasswordCount,
                        fpMachine.FPCount,
                        fpMachine.FCCount,
                        fpMachine.AttLogCount,
                        fpMachine.CreatedAt,
                        fpMachine.UpdatedAt,
                    };
                    string modelstr = JsonConvert.SerializeObject(model, JSONsettings);
                    var feedback = GlobalHost.ConnectionManager.GetHubContext<FeedbackHub>();
                    //Calls addNewRecord on all clients (javascript function)
                    feedback.Clients.All.updateRecord(fpMachine.Id.ToString(), modelstr);
                }
            }
            finally
            {

            }
        }

        public void OnDisConnected(int iMachineNumber, int Tag)
        {
            try
            {
                var fpMachine = _fpMachineService.GetObjectById(Tag); //GetObjectByMachineNumber(iMachineNumber);
                if (fpMachine != null)
                {
                    fpMachine.IsConnected = false;

                    //Notify clients
                    dynamic model = new
                    {
                        fpMachine.Id,
                        fpMachine.CompanyInfoId,
                        CompanyInfo = fpMachine.CompanyInfo.Name,
                        fpMachine.MachineNumber,
                        fpMachine.MachineName,
                        fpMachine.Password,
                        fpMachine.IsAutoConnect,
                        fpMachine.IsConnected,
                        fpMachine.IsInSync,
                        fpMachine.CommType,
                        fpMachine.EthernetIP,
                        fpMachine.EthernetPort,
                        fpMachine.EthernetMAC,
                        fpMachine.SerialPort,
                        fpMachine.SerialBaudRate,
                        fpMachine.TimeZone,
                        //fpMachine.ProductName,
                        fpMachine.Platform,
                        fpMachine.SerialNumber,
                        fpMachine.UserCount,
                        fpMachine.AdminCount,
                        fpMachine.PasswordCount,
                        fpMachine.FPCount,
                        fpMachine.FCCount,
                        fpMachine.AttLogCount,
                        fpMachine.CreatedAt,
                        fpMachine.UpdatedAt,
                    };
                    string modelstr = JsonConvert.SerializeObject(model, JSONsettings);
                    var feedback = GlobalHost.ConnectionManager.GetHubContext<FeedbackHub>();
                    //Calls addNewRecord on all clients (javascript function)
                    feedback.Clients.All.updateRecord(fpMachine.Id.ToString(), modelstr);
                }
            }
            finally
            {

            }
        }

        public void OnAttTransactionEx(string sEnrollNumber, int iIsInValid, int iAttState, int iVerifyMethod, int iYear, int iMonth, int iDay, int iHour, int iMinute, int iSecond, int iWorkCode, int iMachineNumber, int Tag)
        {
            try
            {
                int pin = int.Parse(sEnrollNumber);
                var fpUser = _fpUserService.GetObjectByPIN(pin);
                if (fpUser != null)
                {
                    var fpAttLog = new FPAttLog()
                    {
                        DeviceID = iMachineNumber, //1,
                        FPUserId = fpUser.Id,
                        PIN = pin,
                        PIN2 = fpUser.PIN2,
                        VerifyMode = iVerifyMethod,
                        InOutMode = iAttState,
                        WorkCode = iWorkCode,
                        Time_second = new DateTime(iYear, iMonth, iDay, iHour, iMinute, iSecond),
                    };
                    _fpAttLogService.FindOrCreateObject(fpAttLog, _fpUserService);

                    //Notify clients
                    dynamic model = new {
                             fpAttLog.Id,
                             fpAttLog.PIN,
                             fpAttLog.PIN2,
                             fpAttLog.Time_second,
                             fpAttLog.DeviceID,
                             fpAttLog.VerifyMode,
                             fpAttLog.InOutMode,
                             fpAttLog.WorkCode,
                             fpAttLog.Reserved,
                             fpAttLog.CreatedAt,
                             fpAttLog.UpdatedAt,
                    };
                    string modelstr = JsonConvert.SerializeObject(model, JSONsettings);
                    var feedback = GlobalHost.ConnectionManager.GetHubContext<FeedbackHub>();
                    //Calls addNewRecord on all clients (javascript function)
                    feedback.Clients.All.addNewRecord(fpAttLog.FPUserId.ToString(), modelstr);
                }
            }
            finally
            {

            }
        }

        public void OnDeleteTemplate(int iEnrollNumber, int iFingerIndex)
        {
            try
            {
                var fpUser = _fpUserService.GetObjectByPIN(iEnrollNumber);
                if (fpUser != null)
                {
                    var tmp = _fpTemplateService.GetObjectByUserFingerID(fpUser.Id, iFingerIndex);
                    if (tmp != null)
                    {
                        //tmp.IsInSync = false;
                        _fpTemplateService.SoftDeleteObject(tmp, _fpUserService);

                        //Notify clients
                        dynamic model = new
                        {
                            tmp.Id,
                        };
                        string modelstr = JsonConvert.SerializeObject(model, JSONsettings);
                        var feedback = GlobalHost.ConnectionManager.GetHubContext<FeedbackHub>();
                        //Calls addNewRecord on all clients (javascript function)
                        feedback.Clients.All.delRecord(tmp.FPUserId.ToString(), modelstr);
                    }
                }
            }
            finally
            {

            }
        }

        public void OnEnrollFinger(int iEnrollNumber, int iFingerIndex, int iActionResult, int iTemplateLength, int iMachineNumber, int Tag)
        {
            try
            {
                if (iActionResult == 0) //Success
                {
                    //RTEvent OnEnrollFiger Has been Triggered
                    int FPMachineID = Tag; // 0;
                    //FPMachine fpMachine = null;
                    //foreach (var device in FPMachines.fpDevices)
                    //{
                    //    if (device.Value.iMachineNumber == iMachineNumber)
                    //    {
                    //        FPMachineID = device.Value.Tag;
                    //        //fpMachine = _fpMachineService.GetObjectById(FPMachineID);
                    //        break;
                    //    }
                    //}
                    var fpUser = _fpUserService.GetObjectByPIN(iEnrollNumber);
                    if (fpUser != null)
                    {
                        byte[] bTmpData = new byte[2048];
                        int iTmpLength = 0;
                        int iFlag = 0;
                        if (FPMachines.fpDevices[FPMachineID].axCZKEM1.GetUserTmpEx(iMachineNumber, iEnrollNumber.ToString(), iFingerIndex, out iFlag, out bTmpData[0], out iTmpLength))
                        {
                            FPTemplate fpTemplate = _fpTemplateService.GetQueryable().Where(x => x.FingerID == iFingerIndex && x.FPUserId == fpUser.Id && !x.IsDeleted).FirstOrDefault();
                            if (fpTemplate == null) fpTemplate = new FPTemplate();
                            fpTemplate.FPUserId = fpUser.Id;
                            fpTemplate.PIN = (int)fpUser.PIN2;
                            fpTemplate.FingerID = (short)iFingerIndex;
                            //fpTemplate.Template = sTmpData;
                            fpTemplate.Template = Convert.ToBase64String(bTmpData, 0, iTmpLength); //Encoding.UTF8.GetString(bTmpData, 0, iTmpLength);
                            fpTemplate.Size = fpTemplate.Template.Length; //iTmpLength
                            //tmp9 = FPDevice.ConvertStruct.ByteArrayToStruct<FPDevice._Template9_>(bTmpData);
                            fpTemplate.Valid = (byte)iFlag; //1; // tmp9.Valid;
                            fpTemplate.IsInSync = true;
                            _fpTemplateService.UpdateOrCreateObject(fpTemplate, _fpUserService);

                            //Notify clients
                            dynamic model = new
                            {
                                fpTemplate.Id,
                                fpTemplate.PIN,
                                fpTemplate.FPUserId,
                                FPUserPIN = fpTemplate.FPUser.PIN,
                                FPUser = fpTemplate.FPUser.Name,
                                fpTemplate.FingerID,
                                fpTemplate.Valid,
                                fpTemplate.IsInSync,
                                fpTemplate.Size,
                                fpTemplate.Template,
                                fpTemplate.CreatedAt,
                                fpTemplate.UpdatedAt,
                            };
                            string modelstr = JsonConvert.SerializeObject(model, JSONsettings);
                            var feedback = GlobalHost.ConnectionManager.GetHubContext<FeedbackHub>();
                            //Calls addNewRecord on all clients (javascript function)
                            feedback.Clients.All.addNewRecord(fpTemplate.FPUserId.ToString(), modelstr);
                        }
                    }
                }
                else
                {
                    //RTEvent OnEnrollFiger was Triggered by Error
                }
            }
            finally
            {

            }
        }

        public virtual unsafe void OnEMData(int DataType, int DataLen, ref sbyte DataBuffer)
        {
            //RTEvent OnEMData Has been Triggered...;
            try
            {
                var data = new sbyte[DataLen];
                data[0] = DataBuffer;
            }
            finally
            {

            }
        }
        #endregion

       

    }
}

        