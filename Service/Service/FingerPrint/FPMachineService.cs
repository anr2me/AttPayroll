using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;
using Data.Repository;
using System.Data;
using System.Data.Entity;
using FPDevice;

namespace Service.Service
{
    public class FPMachineService : IFPMachineService
    {
        private IFPMachineRepository _repository;
        private IFPMachineValidator _validator;

        public FPMachineService(IFPMachineRepository _fpMachineRepository, IFPMachineValidator _fpMachineValidator)
        {
            _repository = _fpMachineRepository;
            _validator = _fpMachineValidator;
        }

        public IFPMachineValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<FPMachine> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<FPMachine> GetAll()
        {
            return _repository.GetAll();
        }

        public FPMachine GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public FPMachine GetObjectByMachineNumber(int MachineNumber)
        {
            var obj = GetQueryable().Where(x => !x.IsDeleted && x.IsConnected && x.MachineNumber == MachineNumber).Include(x => x.CompanyInfo).FirstOrDefault();
            if (obj != null) obj.Errors = new Dictionary<string, string>();
            return obj;
        }

        public FPMachine CreateObject(FPMachine fpMachine, ICompanyInfoService _companyInfoService)
        {
            fpMachine.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(fpMachine, this, _companyInfoService))
            {
                //fpMachine.fpDevice = new FPDevice.ZKEvents();
                _repository.CreateObject(fpMachine);
                if (GetObjectById(fpMachine.Id) != null)
                {
                    AddMachine(fpMachine.Id); //FPMachines.fpDevices.Add(fpMachine.Id, new FPDevice.ZKEvents());
                }
            }
            return fpMachine;
        }

        public FPMachine UpdateObject(FPMachine fpMachine, ICompanyInfoService _companyInfoService)
        {
            if(_validator.ValidUpdateObject(fpMachine, this, _companyInfoService))
            {
                //if (fpMachine.fpDevice == null) fpMachine.fpDevice = new FPDevice.ZKEvents();
                //if (fpMachine.fpDevice.bIsConnected) fpMachine.fpDevice.Disconnect();
                if (FPMachines.fpDevices[fpMachine.Id] == null) AddMachine(fpMachine.Id); //FPMachines.fpDevices.Add(fpMachine.Id, new FPDevice.ZKEvents());
                fpMachine.IsConnected = FPMachines.fpDevices[fpMachine.Id].bIsConnected; // fpMachine.fpDevice.bIsConnected;
                _repository.UpdateObject(fpMachine);
            }
            return fpMachine;
        }

        [STAThread]
        public FPMachine SoftDeleteObject(FPMachine fpMachine)
        {
            if (_validator.ValidDeleteObject(fpMachine, this))
            {
                fpMachine.IsConnected = false;
                if (FPMachines.fpDevices[fpMachine.Id] != null) // (fpMachine.fpDevice != null)
                {
                    lock (FPMachines.fpDevices[fpMachine.Id]._locker)
                    {
                        if (FPMachines.fpDevices[fpMachine.Id].bIsConnected) FPMachines.fpDevices[fpMachine.Id].Disconnect();
                        fpMachine.IsConnected = FPMachines.fpDevices[fpMachine.Id].bIsConnected;
                        FPMachines.fpDevices[fpMachine.Id].bIsDestroying = true;
                    }
                    FPMachines.fpDevices[fpMachine.Id].Dispose();
                    FPMachines.fpDevices[fpMachine.Id] = null;
                    FPMachines.fpDevices.Remove(fpMachine.Id);
                }
                _repository.SoftDeleteObject(fpMachine);
            }
            return fpMachine;
        }

        [STAThread]
        public FPMachine ConnectObject(FPMachine fpMachine)
        {
            if (_validator.ValidConnectObject(fpMachine, this))
            {
                if (FPMachines.fpDevices[fpMachine.Id] == null) AddMachine(fpMachine.Id); //FPMachines.fpDevices.Add(fpMachine.Id, new FPDevice.ZKEvents());
                lock (FPMachines.fpDevices[fpMachine.Id]._locker)
                {
                    switch ((Constant.FPCommType)fpMachine.CommType)
                    {
                        case Constant.FPCommType.Ethernet: FPMachines.fpDevices[fpMachine.Id].ConnectIP(fpMachine.EthernetIP, fpMachine.EthernetPort); break;
                        case Constant.FPCommType.Serial: FPMachines.fpDevices[fpMachine.Id].ConnectRS232(fpMachine.SerialPort, fpMachine.SerialBaudRate, fpMachine.MachineNumber); break;
                        default: FPMachines.fpDevices[fpMachine.Id].ConnectUSB(true, fpMachine.MachineNumber); break;
                    }
                    fpMachine.IsConnected = FPMachines.fpDevices[fpMachine.Id].bIsConnected;

                    if (fpMachine.IsConnected)
                    {
                        fpMachine.MachineNumber = FPMachines.fpDevices[fpMachine.Id].iMachineNumber;
                    }
                    else
                    {
                        //int idwErrorCode = FPMachines.fpDevices[fpMachine.Id].GetLastError();
                        string err = FPMachines.fpDevices[fpMachine.Id].GetLastErrorMsg();
                        fpMachine.Errors.Add("Generic", "Connect Error : " + err);
                    }
                }
                _repository.UpdateObject(fpMachine);
            }
            return fpMachine;
        }

        [STAThread]
        public FPMachine DisconnectObject(FPMachine fpMachine)
        {
            if (_validator.ValidDisconnectObject(fpMachine, this))
            {
                fpMachine.IsConnected = false;
                if (FPMachines.fpDevices[fpMachine.Id] != null)
                {
                    lock (FPMachines.fpDevices[fpMachine.Id]._locker)
                    {
                        FPMachines.fpDevices[fpMachine.Id].Disconnect();
                        fpMachine.IsConnected = FPMachines.fpDevices[fpMachine.Id].bIsConnected;
                    }
                }
                _repository.UpdateObject(fpMachine);
            }
            return fpMachine;
        }

        [STAThread]
        public FPMachine RefreshObject(FPMachine fpMachine, IFPUserService _fpUserService, IFPTemplateService _fpTemplateService)
        {
            if (_validator.ValidRefreshObject(fpMachine, this))
            {
                if (FPMachines.fpDevices[fpMachine.Id] != null) //FPMachines.fpDevices.Add(fpMachine.Id, new FPDevice.ZKEvents());
                {
                    lock (FPMachines.fpDevices[fpMachine.Id]._locker)
                    {
                        fpMachine.IsConnected = FPMachines.fpDevices[fpMachine.Id].bIsConnected;
                        if (fpMachine.IsConnected)
                        {
                            fpMachine.MachineNumber = FPMachines.fpDevices[fpMachine.Id].iMachineNumber;
                            fpMachine.IsInSync = true;
                            string st = "";
                            if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetPlatform(fpMachine.MachineNumber, ref st))
                            {
                                fpMachine.Platform = st;
                            }
                            if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetFirmwareVersion(fpMachine.MachineNumber, ref st))
                            {
                                fpMachine.FirmwareVer = st;
                            }
                            if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetSysOption(fpMachine.MachineNumber, "~ZKFPVersion", out st))
                            {
                                fpMachine.ArithmeticVer = st;
                            }
                            if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetSerialNumber(fpMachine.MachineNumber, out st))
                            {
                                fpMachine.SerialNumber = st;
                            }
                            if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetDeviceMAC(fpMachine.MachineNumber, ref st))
                            {
                                fpMachine.EthernetMAC = st;
                            }
                            int usercount = 0;
                            if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetDeviceStatus(fpMachine.MachineNumber, (int)FPDevice.DeviceStatusType.UserCount, ref usercount))
                            {
                                fpMachine.UserCount = _fpUserService.GetQueryable().Where(x => !x.IsDeleted).Count();
                                if (fpMachine.UserCount != usercount) fpMachine.IsInSync = false;
                                fpMachine.UserCount = usercount;
                            }
                            int admincount = 0;
                            if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetDeviceStatus(fpMachine.MachineNumber, (int)FPDevice.DeviceStatusType.AdminCount, ref admincount))
                            {
                                if (fpMachine.AdminCount != admincount) fpMachine.IsInSync = false;
                                fpMachine.AdminCount = admincount;
                            }
                            int pwdcount = 0;
                            if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetDeviceStatus(fpMachine.MachineNumber, (int)FPDevice.DeviceStatusType.PasswordCount, ref pwdcount))
                            {
                                if (fpMachine.PasswordCount != pwdcount) fpMachine.IsInSync = false;
                                fpMachine.PasswordCount = pwdcount;
                            }
                            int fpcount = 0;
                            if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetDeviceStatus(fpMachine.MachineNumber, (int)FPDevice.DeviceStatusType.FPTemplateCount, ref fpcount))
                            {
                                fpMachine.FPCount = _fpTemplateService.GetQueryable().Where(x => !x.IsDeleted).Count();
                                if (fpMachine.FPCount != fpcount) fpMachine.IsInSync = false;
                                fpMachine.FPCount = fpcount;
                            }
                            int facecount = 0;
                            if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetDeviceStatus(fpMachine.MachineNumber, (int)FPDevice.DeviceStatusType.FaceCount, ref facecount))
                            {
                                int cnt = ((facecount >= 9999999) ? 0 : facecount);
                                if (fpMachine.FCCount != cnt) fpMachine.IsInSync = false;
                                fpMachine.FCCount = cnt;
                            }
                            int attcount = 0;
                            if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetDeviceStatus(fpMachine.MachineNumber, (int)FPDevice.DeviceStatusType.AttLogCount, ref attcount))
                            {
                                fpMachine.AttLogCount = attcount;
                            }
                        }
                    }
                    _repository.UpdateObject(fpMachine);
                }
            }
            return fpMachine;
        }

        public FPMachine SyncObject(FPMachine fpMachine, IFPUserService _fpUserService, IFPTemplateService _fpTemplateService)
        {
            if (_validator.ValidSyncObject(fpMachine, this, _fpUserService, _fpTemplateService))
            {

                fpMachine.IsInSync = true;
                _repository.UpdateObject(fpMachine);
            }
            return fpMachine;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(FPMachine fpMachine)
        {
            IQueryable<FPMachine> objs = _repository.FindAll(x => x.MachineName == fpMachine.MachineName && !x.IsDeleted && x.Id != fpMachine.Id);
            return (objs.Count() > 0 ? true : false);
        }

        public bool IsConnected(FPMachine fpMachine)
        {
            if (GetObjectById(fpMachine.Id) != null)
            {
                if (FPMachines.fpDevices[fpMachine.Id] == null) AddMachine(fpMachine.Id); //FPMachines.fpDevices.Add(fpMachine.Id, new FPDevice.ZKEvents());
                fpMachine.IsConnected = FPMachines.fpDevices[fpMachine.Id].bIsConnected;
            }
            return fpMachine.IsConnected;
        }

        public bool IsConnected(int FPMachineId)
        {
            FPMachine fpMachine = GetObjectById(FPMachineId);
            if (fpMachine != null)
            {
                if (FPMachines.fpDevices[FPMachineId] == null) return false; // FPMachines.fpDevices.Add(FPMachineId, new FPDevice.ZKEvents());
                fpMachine.IsConnected = FPMachines.fpDevices[FPMachineId].bIsConnected;
                return fpMachine.IsConnected;
            }
            return false;
        }

        public bool IsInSync(int FPMachineId)
        {
            FPMachine fpMachine = GetObjectById(FPMachineId);
            if (fpMachine != null)
            {
                if (FPMachines.fpDevices[FPMachineId] == null) return false; // FPMachines.fpDevices.Add(FPMachineId, new FPDevice.ZKEvents());
                fpMachine.IsConnected = FPMachines.fpDevices[FPMachineId].bIsConnected;
                //if (fpMachine.IsConnected) RefreshObject(fpMachine);
                return fpMachine.IsInSync;
            }
            return false;
        }

        public bool DownloadAttLog(FPMachine fpMachine, bool ClearAfterDownload, IFPUserService _fpUserService, IFPAttLogService _fpAttLogService)
        {
            if (fpMachine == null || fpMachine.Id <= 0)
            {
                fpMachine.Errors.Add("Generic", "FingerPrint Machine ID not found!");
            }
            else
            {
                if (FPMachines.fpDevices[fpMachine.Id] == null) fpMachine.IsConnected = false; // FPMachines.fpDevices.Add(FPMachineId, new FPDevice.ZKEvents());
                else fpMachine.IsConnected = FPMachines.fpDevices[fpMachine.Id].bIsConnected;
                if (fpMachine.IsConnected == false)
                {
                    fpMachine.Errors.Add("Generic", "Please connect the device first!");
                    return false;
                }
                bool ok = true;
                lock (FPMachines.fpDevices[fpMachine.Id]._locker)
                {
                    FPMachines.fpDevices[fpMachine.Id].axCZKEM1.EnableDevice(fpMachine.MachineNumber, false); // Prevent user from using the device
                    try
                    {
                        //int idwErrorCode = 0;

                        int idwEnrollNumber = 0;
                        int idwVerifyMode = 0;
                        int idwInOutMode = 0;

                        int idwYear = 0;
                        int idwMonth = 0;
                        int idwDay = 0;
                        int idwHour = 0;
                        int idwMinute = 0;
                        int idwSecond = 0;
                        int idwWorkCode = 0;
                        int idwReserved = 0;

                        int iGLCount = 0;
                        int iIndex = 0;

                        if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.ReadGeneralLogData(fpMachine.MachineNumber)) //read all the attendance records to the memory
                        {
                            while (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetGeneralExtLogData(fpMachine.MachineNumber, ref idwEnrollNumber, ref idwVerifyMode, ref idwInOutMode,
                                     ref idwYear, ref idwMonth, ref idwDay, ref idwHour, ref idwMinute, ref idwSecond, ref idwWorkCode, ref idwReserved)) //get records from the memory
                            {
                                iGLCount++;
                                var fpUser = _fpUserService.GetObjectByPIN(idwEnrollNumber);
                                if (fpUser != null)
                                {
                                    var fpAttLog = new FPAttLog()
                                    {
                                        DeviceID = fpMachine.MachineNumber,
                                        FPUserId = fpUser.Id,
                                        PIN = idwEnrollNumber,
                                        PIN2 = fpUser.PIN2,
                                        VerifyMode = idwVerifyMode,
                                        InOutMode = idwInOutMode,
                                        WorkCode = idwWorkCode,
                                        Reserved = idwReserved,
                                        Time_second = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond),
                                    };
                                    _fpAttLogService.FindOrCreateObject(fpAttLog, _fpUserService);
                                }
                                iIndex++;
                            }
                        }
                        int errc = FPMachines.fpDevices[fpMachine.Id].GetLastError();
                        if (errc != (int)FPDevice.ErrorCode.NoError && errc != (int)FPDevice.ErrorCode.DataNotFound)
                        {
                            fpMachine.Errors.Add("Generic", "Download Failed (Error:" + FPMachines.fpDevices[fpMachine.Id].GetErrorMsg(errc) + ")");
                            ok = false;
                        }
                        else if (ClearAfterDownload)
                        {
                            // Delete all attendance records on machine
                            if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.ClearGLog(fpMachine.MachineNumber))
                            {
                                FPMachines.fpDevices[fpMachine.Id].axCZKEM1.RefreshData(fpMachine.MachineNumber); //the data in the device should be refreshed
                            }
                        }
                    }
                    finally
                    {
                        FPMachines.fpDevices[fpMachine.Id].axCZKEM1.EnableDevice(fpMachine.MachineNumber, true);
                    }
                }
                return ok;
            }
            return false;
        }

        public bool DownloadAllUserData(FPMachine fpMachine, IFPUserService _fpUserService, IFPTemplateService _fpTemplateService, IEmployeeService _employeeService)
        {
            if (fpMachine == null || fpMachine.Id <= 0)
            {
                fpMachine.Errors.Add("Generic", "FingerPrint Machine ID not found!");
            }
            else
            {
                if (FPMachines.fpDevices[fpMachine.Id] == null) fpMachine.IsConnected = false; // FPMachines.fpDevices.Add(FPMachineId, new FPDevice.ZKEvents());
                else fpMachine.IsConnected = FPMachines.fpDevices[fpMachine.Id].bIsConnected;
                if (fpMachine.IsConnected == false)
                {
                    fpMachine.Errors.Add("Generic","Please connect the device first!");
                    return false;
                }
                bool ok = true;
                lock (FPMachines.fpDevices[fpMachine.Id]._locker)
                {
                    //judge whether the device supports 9.0 fingerprint arithmetic
                    string sOption = "~ZKFPVersion";
                    string sValue = "";
                    if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetSysOption(fpMachine.MachineNumber, sOption, out sValue))
                    {
                        //FPTemplate of v9 (uses 512B) and v10(uses 1.6kB) are not compatible
                        //if (sValue == "10")
                        //{
                        //    fpMachine.Errors.Add("Generic", "Your device is not using 9.0 arithmetic!");
                        //    return false;
                        //}
                    }
                    FPMachines.fpDevices[fpMachine.Id].axCZKEM1.EnableDevice(fpMachine.MachineNumber, false); // Prevent user from using the device
                    try
                    {
                        FPMachines.fpDevices[fpMachine.Id].axCZKEM1.BeginBatchUpdate(fpMachine.MachineNumber, (int)FPDevice.BatchUpdateFlag.Overwrite); //create memory space for batching data

                        if (!FPMachines.fpDevices[fpMachine.Id].axCZKEM1.ReadAllUserID(fpMachine.MachineNumber)) //read all the user information to the memory
                        {
                            string err = FPMachines.fpDevices[fpMachine.Id].GetLastErrorMsg();
                            if (FPMachines.fpDevices[fpMachine.Id].iLastError != (int)FPDevice.ErrorCode.DataNotFound)
                            {
                                fpMachine.Errors.Add("Generic", "Unable to read All UserID (Error:" + err + ")");
                                ok = false;
                            }
                        }
                        if (ok && !FPMachines.fpDevices[fpMachine.Id].axCZKEM1.ReadAllTemplate(fpMachine.MachineNumber)) //read all the users' fingerprint templates to the memory
                        {
                            string err = FPMachines.fpDevices[fpMachine.Id].GetLastErrorMsg();
                            if (FPMachines.fpDevices[fpMachine.Id].iLastError != (int)FPDevice.ErrorCode.DataNotFound)
                            {
                                fpMachine.Errors.Add("Generic", "Unable to read All Templates (Error:" + err + ")");
                                ok = false;
                            }
                        }
                        int idwEnrollNumber = 0;
                        string sName = "";
                        string sPassword = "";
                        int iPrivilege = 0;
                        bool bEnabled = false;
                        string card ="";
                        int pin2 = 0;
                        int grp = 0;
                        string TZstr = "";

                        //string sTmpData = "";
                        byte[] bTmpData = new byte[2048];
                        int iTmpLength = 0;
                        int iFlag = 0;
                        //FPDevice._Template9_ tmp9;
                        int vs = (int)FPDevice.VerifyMethod.UnSet;
                        byte[] rsv = new byte[4];

                        if (ok)
                        {
                            // Mark All User as Not InSync first
                            foreach (var fpUser in _fpUserService.GetAll())
                            {
                                fpUser.IsInSync = false;
                            }

                            while (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetAllUserInfo(fpMachine.MachineNumber, ref idwEnrollNumber, ref sName, ref sPassword, ref iPrivilege, ref bEnabled)) //get all the users' information from the memory
                            {
                                FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetPIN2(idwEnrollNumber, ref pin2);
                                FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetStrCardNumber(out card);
                                FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetUserGroup(fpMachine.MachineNumber, idwEnrollNumber, ref grp);
                                FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetUserTZStr(fpMachine.MachineNumber, idwEnrollNumber, ref TZstr);
                                FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetUserInfoEx(fpMachine.MachineNumber, idwEnrollNumber, out vs, out rsv[0]);
                                var err = FPMachines.fpDevices[fpMachine.Id].GetLastErrorMsg();

                                FPUser fpUser = _fpUserService.GetObjectByPIN(idwEnrollNumber);
                                if (fpUser == null) fpUser = new FPUser();
                                fpUser.PIN = idwEnrollNumber;
                                fpUser.Name = sName;
                                fpUser.Password = sPassword;
                                fpUser.Privilege = (byte)iPrivilege;
                                fpUser.IsEnabled = bEnabled;
                                fpUser.Card = card;
                                fpUser.PIN2 = pin2;
                                fpUser.Group = (byte)grp;
                                fpUser.TimeZones = TZstr;
                                fpUser.VerifyMode = vs;
                                fpUser.Reserved = BitConverter.ToString(rsv).Replace("-",""); //rsv.CopyTo(fpUser.Reserved, 0);
                                fpUser.IsInSync = true;
                                _fpUserService.UpdateOrCreateObject(fpUser, _employeeService);
                                //Mark non existing fpTemplates as not InSync
                                var tmplist = _fpTemplateService.GetQueryable().Where(x => x.FPUserId == fpUser.Id);
                                foreach (var tmp in tmplist.Where(x => x.IsInSync && !x.IsDeleted).ToList())
                                {
                                    tmp.IsInSync = false;
                                    //_fpTemplateService.GetRepository().Update(tmp);
                                }
                                //Create or Update existing fpTemplates
                                for (int idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                                {
                                    //if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetUserTmpExStr(fpMachine.MachineNumber, idwEnrollNumber, idwFingerIndex, ref sTmpData, ref iTmpLength)) //get the corresponding templates string and length from the memory
                                    if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetUserTmpEx(fpMachine.MachineNumber, idwEnrollNumber.ToString(), idwFingerIndex, out iFlag, out bTmpData[0], out iTmpLength))
                                    {
                                        FPTemplate fpTemplate = _fpTemplateService.GetQueryable().Where(x => x.FingerID == idwFingerIndex && x.FPUserId == fpUser.Id && !x.IsDeleted).FirstOrDefault();
                                        if (fpTemplate == null) fpTemplate = new FPTemplate();
                                        fpTemplate.FPUserId = fpUser.Id;
                                        fpTemplate.PIN = (int)fpUser.PIN2;
                                        fpTemplate.FingerID = (short)idwFingerIndex;
                                        //fpTemplate.Template = sTmpData;
                                        fpTemplate.Template = Convert.ToBase64String(bTmpData, 0, iTmpLength); //Encoding.UTF8.GetString(bTmpData, 0, iTmpLength);
                                        fpTemplate.Size = fpTemplate.Template.Length; //iTmpLength
                                        //tmp9 = FPDevice.ConvertStruct.ByteArrayToStruct<FPDevice._Template9_>(bTmpData);
                                        fpTemplate.Valid = (byte)iFlag; //1; // tmp9.Valid;
                                        fpTemplate.IsInSync = true;
                                        _fpTemplateService.UpdateOrCreateObject(fpTemplate, _fpUserService);
                                    }
                                }
                                tmplist = tmplist.Where(x => !x.IsInSync);
                                fpUser.IsInSync = (tmplist.Count() == 0);
                                _fpUserService.UpdateObject(fpUser, _employeeService); // reupdate to prevent fpTemplate from changing fpUser.IsInSync to False when Updating
                            }
                        }
                        int errc = FPMachines.fpDevices[fpMachine.Id].GetLastError();
                        if (errc != (int)FPDevice.ErrorCode.NoError && errc != (int)FPDevice.ErrorCode.DataNotFound)
                        {
                            fpMachine.Errors.Add("Generic", "Download Failed (Error:" + FPMachines.fpDevices[fpMachine.Id].GetErrorMsg(errc) + ")");
                            ok = false;
                            //FPMachines.fpDevices[fpMachine.Id].axCZKEM1.CancelBatchUpdate(fpMachine.MachineNumber);
                        }
                        FPMachines.fpDevices[fpMachine.Id].axCZKEM1.BatchUpdate(fpMachine.MachineNumber); //download all the information in the memory
                    }
                    finally
                    {
                        //FPMachines.fpDevices[fpMachine.Id].axCZKEM1.RefreshData(fpMachine.MachineNumber); //the data in the device should be refreshed
                        FPMachines.fpDevices[fpMachine.Id].axCZKEM1.EnableDevice(fpMachine.MachineNumber, true);
                    }
                }
                return ok;
            }
            return false;
        }

        public bool UploadAllUserData(FPMachine fpMachine, bool ReSyncAll, bool SyncDateTime, IFPUserService _fpUserService, IFPTemplateService _fpTemplateService, IEmployeeService _employeeService)
        {
            if (fpMachine == null || fpMachine.Id <= 0)
            {
                fpMachine.Errors.Add("Generic", "FingerPrint Machine ID not found!");
            }
            else
            {
                if (FPMachines.fpDevices[fpMachine.Id] == null) fpMachine.IsConnected = false; // FPMachines.fpDevices.Add(FPMachineId, new FPDevice.ZKEvents());
                else fpMachine.IsConnected = FPMachines.fpDevices[fpMachine.Id].bIsConnected;
                if (fpMachine.IsConnected == false)
                {
                    fpMachine.Errors.Add("Generic", "Please connect the device first!");
                    return false;
                }
                bool ok = true;
                lock (FPMachines.fpDevices[fpMachine.Id]._locker)
                {
                    //judge whether the device supports 9.0 fingerprint arithmetic
                    string sOption = "~ZKFPVersion";
                    string sValue = "";
                    if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetSysOption(fpMachine.MachineNumber, sOption, out sValue))
                    {
                        //if (sValue == "10")
                        //{
                        //    fpMachine.Errors.Add("Generic", "Your device is not using 9.0 arithmetic!");
                        //    return false;
                        //}
                    }
                    FPMachines.fpDevices[fpMachine.Id].axCZKEM1.EnableDevice(fpMachine.MachineNumber, false); // Prevent user from using the device
                    try
                    {
                        FPMachines.fpDevices[fpMachine.Id].axCZKEM1.BeginBatchUpdate(fpMachine.MachineNumber, (int)FPDevice.BatchUpdateFlag.Overwrite); //create memory space for batching data
                        
                        int idwEnrollNumber = 0;
                        string sName = "";
                        string sPassword = "";
                        int iPrivilege = 0;
                        bool bEnabled = false;
                        string card = "";
                        //int pin2 = 0;
                        int grp = 0;
                        string TZstr = "";

                        string sTmpData = "";
                        byte[] bTmpData = new byte[2048];
                        //int iTmpLength = 0;
                        //FPDevice._Template9_ tmp9;
                        byte[] rsv = new byte[4];
                        //int iLastEnrollNumber = 0; //the former enrollnumber you have upload (define original value as 0)

                        // if ReSync = True then Delete All User Data first
                        if (ReSyncAll)
                        {
                            FPMachines.fpDevices[fpMachine.Id].axCZKEM1.ClearAdministrators(fpMachine.MachineNumber);
                            //FPMachines.fpDevices[fpMachine.Id].axCZKEM1.ClearGLog(fpMachine.MachineNumber); // Clear attendance data
                            //FPMachines.fpDevices[fpMachine.Id].axCZKEM1.ClearData(fpMachine.MachineNumber, (int)FPDevice.DataFlag.UserInfo);
                            FPMachines.fpDevices[fpMachine.Id].axCZKEM1.ClearKeeperData(fpMachine.MachineNumber); // Clear All Data
                        }

                        //Sync Machine's DateTime
                        if (SyncDateTime)
                        {
                            DateTime curutc = DateTime.Now.ToUniversalTime().AddMinutes((double)fpMachine.TimeZoneOffset);
                            string winTZ = fpMachine.TimeZone.ToUpper(); // FPDevice.Convertion.IanaToWindows(fpMachine.TimeZone);
                            TimeZoneInfo destTZ = TimeZoneInfo.GetSystemTimeZones().Where(x => x.Id.ToUpper() == winTZ).FirstOrDefault();
                            DateTime curtime = TimeZoneInfo.ConvertTime(curutc, destTZ);
                            FPMachines.fpDevices[fpMachine.Id].axCZKEM1.SetDeviceTime2(fpMachine.MachineNumber, curtime.Year, curtime.Month, curtime.Day,curtime.Hour, curtime.Minute, curtime.Second);
                        }

                        var userlist = _fpUserService.GetQueryable().Where(x => (ReSyncAll || !x.IsInSync));
                        // Remove deleted user first to prevent deleting existing user with the same UserID
                        foreach (var fpUser in userlist.Where(x => x.IsDeleted).ToList()) //for (int i = 0; i < list.Count; i++)
                        {
                            idwEnrollNumber = fpUser.PIN;
                            //if (ReSyncAll || !fpUser.IsInSync) //identify whether the user information has been uploaded
                            {
                                if (fpUser.IsDeleted)
                                {
                                    if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.DeleteEnrollData(fpMachine.MachineNumber, idwEnrollNumber, fpMachine.MachineNumber, (int)FPDevice.EnrollBackupNumber.AllUserData)) // 12 = delete the user and it's linked data (eg. templates)
                                    {
                                        fpUser.IsInSync = true;
                                        _fpUserService.GetRepository().Update(fpUser);
                                        //_fpUserService.DeleteObject(fpUser.Id);
                                    }
                                }
                            }
                        }
                        // TODO: Create New Users to get the EnrollNumber

                        // Update Users
                        foreach (var fpUser in userlist.Where(x => !x.IsDeleted).ToList()) //for (int i = 0; i < list.Count; i++)
                        {
                            idwEnrollNumber = fpUser.PIN;
                            sName = fpUser.Name ?? "";
                            iPrivilege = fpUser.Privilege;
                            sPassword = fpUser.Password ?? "";
                            bEnabled = fpUser.IsEnabled;
                            //pin2 = fpUser.PIN2;
                            card = fpUser.Card ?? "";
                            grp = fpUser.Group;
                            TZstr = fpUser.TimeZones ?? "";

                            //if (ReSyncAll || !fpUser.IsInSync) //identify whether the user information has been uploaded
                            {
                                //FPMachines.fpDevices[fpMachine.Id].axCZKEM1.GetUserInfo(fpMachine.MachineNumber, idwEnrollNumber, sName, sPassword, iPrivilege, bEnabled);
                                if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.SetUserInfo(fpMachine.MachineNumber, idwEnrollNumber, sName, sPassword, iPrivilege, bEnabled)) //upload user information to the memory
                                {
                                    // TODO: Newly created data doesn't have RollNumber yet
                                    //FPMachines.fpDevices[fpMachine.Id].axCZKEM1.SetPIN2(idwEnrollNumber, pin2);
                                    FPMachines.fpDevices[fpMachine.Id].axCZKEM1.SetStrCardNumber(card);
                                    FPMachines.fpDevices[fpMachine.Id].axCZKEM1.SetUserGroup(fpMachine.MachineNumber, idwEnrollNumber, grp);
                                    FPMachines.fpDevices[fpMachine.Id].axCZKEM1.SetUserTZStr(fpMachine.MachineNumber, idwEnrollNumber, TZstr);

                                    var tmplist = _fpTemplateService.GetQueryable().Where(x => x.FPUserId == fpUser.Id && (ReSyncAll || !x.IsInSync));
                                    // Remove deleted template first
                                    foreach (var tmp in tmplist.Where(x => x.IsDeleted).ToList())
                                    {
                                        //if (ReSyncAll || !tmp.IsInSync)
                                        {
                                            if (FPMachines.fpDevices[fpMachine.Id].axCZKEM1.DeleteEnrollData(fpMachine.MachineNumber, idwEnrollNumber, fpMachine.MachineNumber, tmp.FingerID))
                                            {
                                                //tmp.IsInSync = true;
                                                //_fpTemplateService.GetRepository().Update(tmp);
                                                _fpTemplateService.DeleteObject(tmp.Id);
                                            }
                                        }
                                    }
                                    ok = true;
                                    // Update or Create template
                                    foreach (var tmp in tmplist.Where(x => !x.IsDeleted).ToList())
                                    {
                                        //if (ReSyncAll || !tmp.IsInSync)
                                        {
                                            //bTmpData = Convert.FromBase64String(tmp.Template); //Encoding.UTF8.GetBytes(tmp.Template);
                                            sTmpData = tmp.Template;
                                            if (!FPMachines.fpDevices[fpMachine.Id].axCZKEM1.SetUserTmpExStr(fpMachine.MachineNumber, idwEnrollNumber.ToString(), tmp.FingerID, (int)FPDevice.TemplateFlag.Normal, sTmpData)) //upload templates information to the memory
                                            {
                                                var err = FPMachines.fpDevices[fpMachine.Id].GetLastError();
                                                if (err != (int)FPDevice.ErrorCode.TimedOut && err != (int)FPDevice.ErrorCode.NotInitialized)
                                                {
                                                    //tmp.IsDeleted = true;
                                                    //_fpTemplateService.GetRepository().Update(tmp);
                                                    _fpTemplateService.DeleteObject(tmp.Id);
                                                }
                                                ok = false;
                                                break;
                                            }
                                            tmp.IsInSync = true;
                                            _fpTemplateService.GetRepository().Update(tmp);
                                        }
                                    }
                                    if (fpUser.VerifyMode != (int)FPDevice.VerifyMethod.UnSet)
                                    {
                                        rsv = FPDevice.Convertion.HexStringToByteArray(fpUser.Reserved ?? "00");
                                        FPMachines.fpDevices[fpMachine.Id].axCZKEM1.SetUserInfoEx(fpMachine.MachineNumber, idwEnrollNumber, fpUser.VerifyMode, ref rsv[0]);
                                    }
                                    else
                                    {
                                        FPMachines.fpDevices[fpMachine.Id].axCZKEM1.DeleteUserInfoEx(fpMachine.MachineNumber, idwEnrollNumber);
                                    }
                                    if (ok)
                                    {
                                        fpUser.IsInSync = ok;
                                        _fpUserService.GetRepository().Update(fpUser);
                                    }
                                }
                                else
                                {
                                    fpMachine.Errors.Add("Generic", "Upload failed (Error:" + FPMachines.fpDevices[fpMachine.Id].GetLastErrorMsg() + ")");
                                    break;
                                }
                            }
                            //iLastEnrollNumber = idwEnrollNumber;//change the value of iLastEnrollNumber dynamicly
                        }
                        int errc = FPMachines.fpDevices[fpMachine.Id].GetLastError();
                        if (errc != (int)FPDevice.ErrorCode.NoError && errc != (int)FPDevice.ErrorCode.IOError)
                        {
                            fpMachine.Errors.Add("Generic", "Upload Failed (Error:" + FPMachines.fpDevices[fpMachine.Id].GetErrorMsg(errc) + ")");
                            ok = false;
                            //FPMachines.fpDevices[fpMachine.Id].axCZKEM1.CancelBatchUpdate(fpMachine.MachineNumber);
                        }
                        FPMachines.fpDevices[fpMachine.Id].axCZKEM1.BatchUpdate(fpMachine.MachineNumber); //download all the information in the memory
                    }
                    finally
                    {
                        FPMachines.fpDevices[fpMachine.Id].axCZKEM1.RefreshData(fpMachine.MachineNumber); //the data in the device should be refreshed
                        FPMachines.fpDevices[fpMachine.Id].axCZKEM1.EnableDevice(fpMachine.MachineNumber, true);
                    }
                }
                return ok;
            }
            return false;
        }

        public void AddMachine(int FPMachineId)
        {
            FPMachines.fpDevices.Add(FPMachineId, new FPDevice.ZKEvents());
            FPMachines.fpDevices[FPMachineId].Tag = FPMachineId;
            //FPMachines.fpDevices[FPMachineId].axCZKEM1.OnAttTransactionEx += new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(OnAttTransactionEx);
        }

        

    }
}