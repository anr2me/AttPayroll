using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IFPMachineService
    {
        IFPMachineValidator GetValidator();
        IQueryable<FPMachine> GetQueryable();
        IList<FPMachine> GetAll();
        FPMachine GetObjectById(int Id);
        FPMachine GetObjectByMachineNumber(int MachineNumber);
        FPMachine CreateObject(FPMachine fpMachine, ICompanyInfoService _companyInfoService);
        FPMachine UpdateObject(FPMachine fpMachine, ICompanyInfoService _companyInfoService);
        FPMachine SoftDeleteObject(FPMachine fpMachine);
        FPMachine ConnectObject(FPMachine fpMachine);
        FPMachine DisconnectObject(FPMachine fpMachine);
        FPMachine RefreshObject(FPMachine fpMachine, IFPUserService _fpUserService, IFPTemplateService _fpTemplateService);
        FPMachine SyncObject(FPMachine fpMachine, IFPUserService _fpUserService, IFPTemplateService _fpTemplateService, IEmployeeService _employeeService);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(FPMachine fpMachine);
        bool IsConnected(FPMachine fpMachine);
        bool IsConnected(int FPMachineId);
        bool IsInSync(int FPMachineId);
        bool DownloadAllUserData(FPMachine fpMachine, IFPUserService _fpUserService, IFPTemplateService _fpTemplateService, IEmployeeService _employeeService);
        bool UploadAllUserData(FPMachine fpMachine, bool ReSyncAll, bool SyncDateTime, IFPUserService _fpUserService, IFPTemplateService _fpTemplateService, IEmployeeService _employeeService);
        bool DownloadAttLog(FPMachine fpMachine, bool ClearAfterDownload, IFPUserService _fpUserService, IFPAttLogService _fpAttLogService);
        void AddMachine(int FPMachineId);
    }
}