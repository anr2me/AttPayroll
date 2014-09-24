using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Constants
{
    public partial class Constant
    {
        public enum Sex
        {
            Male,
            Female
        }

        public enum Marital
        {
            Single,
            Married,
        }

        public enum Religion
        {
            Budha,
            Hindu,
            Islam,
            Katolik,
            Kristen,
            Other
        }

        public enum WorkingStatus
        {
            Intern,
            Contract,
            Probation,
            Jobholder,
        }

        public class UserType
        {
            public static string Admin = "Admin";
            public static string Super = "Super";
            public static string User = "User";
        }
    }
}
