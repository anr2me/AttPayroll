using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using System.Data;

namespace Data.Repository
{
    public class UserAccountRepository : EfRepository<UserAccount>, IUserAccountRepository
    {
        private AttPayrollEntities entities;
        public UserAccountRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<UserAccount> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<UserAccount> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public UserAccount GetObjectById(int Id)
        {
            UserAccount userAccount = Find(x => x.Id == Id && !x.IsDeleted);
            if (userAccount != null) { userAccount.Errors = new Dictionary<string, string>(); }
            return userAccount;
        }

        public UserAccount GetObjectByIsAdmin(bool IsAdmin)
        {
            UserAccount userAccount = FindAll(x => x.IsAdmin == IsAdmin && !x.IsDeleted).FirstOrDefault();
            if (userAccount != null) { userAccount.Errors = new Dictionary<string, string>(); }
            return userAccount;
        }

        public UserAccount GetObjectByUsername(string username)
        {
            string lowerusername = username.ToLower();
            UserAccount userAccount = Find(x => x.Username.ToLower() == lowerusername && !x.IsDeleted);
            if (userAccount != null) { userAccount.Errors = new Dictionary<string, string>(); }
            return userAccount;
        }

        public UserAccount IsLoginValid(string username, string password)
        {
            string lowerusername = username.ToLower();
            UserAccount userAccount = Find(x => x.Username.ToLower() == lowerusername && x.Password == password && !x.IsDeleted);
            if (userAccount != null) { userAccount.Errors = new Dictionary<string, string>(); }
            return userAccount;
        }

        public UserAccount CreateObject(UserAccount userAccount)
        {
            userAccount.IsDeleted = false;
            userAccount.CreatedAt = DateTime.Now;
            return Create(userAccount);
        }

        public UserAccount UpdateObject(UserAccount userAccount)
        {
            userAccount.UpdatedAt = DateTime.Now;
            Update(userAccount);
            return userAccount;
        }

        public UserAccount SoftDeleteObject(UserAccount userAccount)
        {
            userAccount.IsDeleted = true;
            userAccount.DeletedAt = DateTime.Now;
            Update(userAccount);
            return userAccount;
        }

        public bool DeleteObject(int Id)
        {
            UserAccount userAccount = Find(x => x.Id == Id);
            return (Delete(userAccount) == 1) ? true : false;
        }

    }
}