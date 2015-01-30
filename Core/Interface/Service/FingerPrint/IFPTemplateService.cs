using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Repository;

namespace Core.Interface.Service
{
    public interface IFPTemplateService
    {
        IFPTemplateValidator GetValidator();
        IFPTemplateRepository GetRepository();
        IQueryable<FPTemplate> GetQueryable();
        IList<FPTemplate> GetAll();
        IList<FPTemplate> GetObjectsByFPUserID(int FPUserID);
        FPTemplate GetObjectById(int Id);
        FPTemplate GetObjectByUserFingerID(int FPUserID, int FingerID);
        FPTemplate CreateObject(FPTemplate fpTemplate, IFPUserService _fpUserService);
        FPTemplate UpdateObject(FPTemplate fpTemplate, IFPUserService _fpUserService);
        FPTemplate UpdateOrCreateObject(FPTemplate fpTemplate, IFPUserService _fpUserService);
        FPTemplate SoftDeleteObject(FPTemplate fpTemplate, IFPUserService _fpUserService);
        bool DeleteObject(int Id);
        bool IsFingerIDDuplicated(FPTemplate fpTemplate);
        bool IsUserInSync(int FPUserId);
    }
}