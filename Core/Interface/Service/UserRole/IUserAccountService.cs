using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IUserAccountService
    {
        IUserAccountValidator GetValidator();
        IQueryable<UserAccount> GetQueryable();
        IList<UserAccount> GetAll();
        UserAccount GetObjectById(int Id);
        UserAccount GetObjectByUserName(string userName);
        UserAccount CreateObject(UserAccount userAccount);
        UserAccount UpdateObject(UserAccount userAccount);
        UserAccount SoftDeleteObject(UserAccount userAccount);
        bool DeleteObject(int Id);
        bool IsUserNameDuplicated(UserAccount userAccount);
    }
}