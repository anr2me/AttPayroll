using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IUserAccountRepository : IRepository<UserAccount>
    {
        IQueryable<UserAccount> GetQueryable();
        IList<UserAccount> GetAll();
        UserAccount GetObjectById(int Id);
        UserAccount CreateObject(UserAccount userAccount);
        UserAccount UpdateObject(UserAccount userAccount);
        UserAccount SoftDeleteObject(UserAccount userAccount);
        bool DeleteObject(int Id);
    }
}