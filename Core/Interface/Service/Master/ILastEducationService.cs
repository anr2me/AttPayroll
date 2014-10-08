using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ILastEducationService
    {
        ILastEducationValidator GetValidator();
        IQueryable<LastEducation> GetQueryable();
        IList<LastEducation> GetAll();
        LastEducation GetObjectById(int Id);
        LastEducation CreateObject(LastEducation lastEducation);
        LastEducation CreateObject(string Institute, string Major, string Level, DateTime EnrollmentDate, Nullable<DateTime> GraduationDate);
        LastEducation UpdateObject(LastEducation lastEducation);
        LastEducation SoftDeleteObject(LastEducation lastEducation);
        bool DeleteObject(int Id);
    }
}