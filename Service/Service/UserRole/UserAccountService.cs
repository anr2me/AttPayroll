using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;


namespace Service.Service
{
    public class UserAccountService : IUserAccountService
    {
        private IUserAccountRepository _repository;
        private IUserAccountValidator _validator;

        public UserAccountService(IUserAccountRepository _userAccountRepository, IUserAccountValidator _userAccountValidator)
        {
            _repository = _userAccountRepository;
            _validator = _userAccountValidator;
        }

        public IUserAccountValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<UserAccount> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<UserAccount> GetAll()
        {
            return _repository.GetAll();
        }

        public UserAccount GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public UserAccount GetObjectByIsAdmin(bool IsAdmin)
        {
            return _repository.GetQueryable().Where(x => x.IsAdmin == IsAdmin).FirstOrDefault();
        }

        public UserAccount GetObjectByUsername(string username)
        {
            return _repository.GetQueryable().Where(x => x.Username == username).FirstOrDefault();
        }

        public UserAccount IsLoginValid(string username, string password)
        {
            StringEncryption se = new StringEncryption();
            string encpassword = se.Encrypt(password);
            string lowerusername = username.ToLower();
            UserAccount userAccount = _repository.GetQueryable().Where(x => x.Username.ToLower() == lowerusername && x.Password == encpassword && !x.IsDeleted).FirstOrDefault();
            if (userAccount != null) { userAccount.Errors = new Dictionary<string, string>(); }
            return userAccount;
        }

        public UserAccount CreateObject(UserAccount userAccount)
        {
            userAccount.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(userAccount, this))
            {
                userAccount.Username = userAccount.Username.Trim();
                StringEncryption se = new StringEncryption();
                //string realpassword = userAccount.Password;
                userAccount.Password = se.Encrypt(userAccount.Password);
                _repository.CreateObject(userAccount);
                //userAccount.Password = realpassword; // set back to unencrypted password to prevent encrypting an already encrypted password on the next update
            }
            return userAccount;
        }

        public UserAccount CreateObject(string username, string password, string name, string description, bool IsAdmin)
        {
            UserAccount userAccount = new UserAccount()
            {
                Username = username,
                Password = password,
                Name = name,
                Description = description,
                IsAdmin = IsAdmin
            };
            return CreateObject(userAccount);
        }

        public UserAccount UpdateObjectPassword(UserAccount userAccount, string OldPassword, string NewPassword, string ConfirmPassword)
        {
            if (_validator.ValidUpdateObjectPassword(userAccount, OldPassword, NewPassword, ConfirmPassword, this))
            {
                userAccount.Username = userAccount.Username.Trim();
                StringEncryption se = new StringEncryption();
                //string realpassword = userAccount.Password;
                userAccount.Password = se.Encrypt(NewPassword);
                _repository.UpdateObject(userAccount);
                //userAccount.Password = realpassword; // set back to unencrypted password to prevent encrypting an already encrypted password on the next update
            }
            return userAccount;
        }

        public UserAccount UpdateObject(UserAccount userAccount)
        {
            if (_validator.ValidUpdateObject(userAccount, this))
            {
                userAccount.Username = userAccount.Username.Trim();
                StringEncryption se = new StringEncryption();
                //string realpassword = userAccount.Password;
                userAccount.Password = se.Encrypt(userAccount.Password);
                _repository.UpdateObject(userAccount);
                //userAccount.Password = realpassword; // set back to unencrypted password to prevent encrypting an already encrypted password on the next update
            }
            return userAccount;
        }

        public UserAccount SoftDeleteObject(UserAccount userAccount, int LoggedId)
        {
            return (_validator.ValidDeleteObject(userAccount, LoggedId) ? _repository.SoftDeleteObject(userAccount) : userAccount);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public UserAccount FindOrCreateSysAdmin()
        {
            UserAccount userAccount = GetObjectByIsAdmin(true);
            if (userAccount == null)
            {
                userAccount = CreateObject(Core.Constants.Constant.UserType.Admin, "sysadmin", "Administrator", "Administrator", true);
            }
            return userAccount;
        }


    }
}
