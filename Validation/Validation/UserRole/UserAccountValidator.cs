using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;
using Service;

namespace Validation.Validation
{
    public class UserAccountValidator : IUserAccountValidator
    {

        public UserAccount VIsValidUsername(UserAccount userAccount)
        {
            if (userAccount.Username == null || userAccount.Username.Trim() == "")
            {
                userAccount.Errors.Add("Username", "Tidak boleh kosong");
            }
            return userAccount;
        }

        public UserAccount VIsValidPassword(UserAccount userAccount)
        {
            if (userAccount.Password == null)
            {
                userAccount.Errors.Add("Password", "Tidak boleh kosong");
            }
            return userAccount;
        }

        public UserAccount VIsCorrectOldPassword(UserAccount userAccount, string OldPassword, IUserAccountService _userAccountService)
        {
            if (OldPassword == null || OldPassword.Trim() == "")
            {
                userAccount.Errors.Add("Generic", "OldPassword Tidak boleh kosong");
            }
            else
            {
                StringEncryption se = new StringEncryption();
                string encOldPassword = se.Encrypt(OldPassword);
                UserAccount userAccount2 = _userAccountService.GetObjectById(userAccount.Id);
                if (encOldPassword != userAccount2.Password)
                {
                    userAccount.Errors.Add("Generic", "Old Password Salah");
                }
            }
            return userAccount;
        }

        public UserAccount VIsCorrectNewPassword(UserAccount userAccount, string NewPassword, string ConfirmPassword)
        {
            if (NewPassword == null || NewPassword.Trim() == "")
            {
                userAccount.Errors.Add("Generic", "New Password Tidak boleh kosong");
            }
            else
            {
                if (NewPassword.Trim() != ConfirmPassword.Trim())
                {
                    userAccount.Errors.Add("Generic", "Confirm Password tidak sama dengan New Password");
                }
            }
            return userAccount;
        }

        public UserAccount VHasUniqueUsername(UserAccount userAccount, IUserAccountService _userAccountService)
        {
            UserAccount userAccount2 = _userAccountService.GetObjectByUsername(userAccount.Username.Trim());
            if (userAccount2 != null && userAccount.Id != userAccount2.Id)
            {
                userAccount.Errors.Add("Username", "Sudah ada");
            }
            return userAccount;
        }

        public UserAccount VIsValidDeletedId(UserAccount userAccount, int LoggedId)
        {
            if (userAccount.Id == LoggedId)
            {
                userAccount.Errors.Add("Generic", "Tidak boleh menghapus account anda sendiri");
            }
            return userAccount;
        }

        public UserAccount VIsNonAdmin(UserAccount userAccount)
        {
            if (userAccount.IsAdmin)
            {
                userAccount.Errors.Add("Generic", "Tidak boleh Admin");
            }
            return userAccount;
        }

        public UserAccount VCreateObject(UserAccount userAccount, IUserAccountService _userAccountService)
        {
            VIsValidUsername(userAccount);
            if (!isValid(userAccount)) { return userAccount; }
            VIsValidPassword(userAccount);
            if (!isValid(userAccount)) { return userAccount; }
            VHasUniqueUsername(userAccount, _userAccountService);
            return userAccount;
        }

        public UserAccount VUpdateObject(UserAccount userAccount, IUserAccountService _userAccountService)
        {
            VIsNonAdmin(userAccount);
            if (!isValid(userAccount)) { return userAccount; }
            VCreateObject(userAccount, _userAccountService);
            return userAccount;
        }

        public UserAccount VUpdateObjectPassword(UserAccount userAccount, string OldPassword, string NewPassword, string ConfirmPassword, IUserAccountService _userAccountService)
        {
            VCreateObject(userAccount, _userAccountService);
            if (!isValid(userAccount)) { return userAccount; }
            VIsCorrectNewPassword(userAccount, NewPassword, ConfirmPassword);
            if (!isValid(userAccount)) { return userAccount; }
            VIsCorrectOldPassword(userAccount, OldPassword, _userAccountService);
            return userAccount;
        }

        public UserAccount VDeleteObject(UserAccount userAccount, int LoggedId)
        {
            VIsValidDeletedId(userAccount, LoggedId);
            return userAccount;
        }

        public bool ValidCreateObject(UserAccount userAccount, IUserAccountService _userAccountService)
        {
            VCreateObject(userAccount, _userAccountService);
            return isValid(userAccount);
        }

        public bool ValidUpdateObjectPassword(UserAccount userAccount, string OldPassword, string NewPassword, string ConfirmPassword, IUserAccountService _userAccountService)
        {
            VUpdateObjectPassword(userAccount, OldPassword, NewPassword, ConfirmPassword, _userAccountService);
            return isValid(userAccount);
        }

        public bool ValidUpdateObject(UserAccount userAccount, IUserAccountService _userAccountService)
        {
            VUpdateObject(userAccount, _userAccountService);
            return isValid(userAccount);
        }

        public bool ValidDeleteObject(UserAccount userAccount, int LoggedId)
        {
            userAccount.Errors.Clear();
            VDeleteObject(userAccount, LoggedId);
            return isValid(userAccount);
        }

        public bool isValid(UserAccount obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(UserAccount obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
