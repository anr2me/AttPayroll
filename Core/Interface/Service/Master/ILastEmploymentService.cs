using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ILastEmploymentService
    {
        ILastEmploymentValidator GetValidator();
        IQueryable<LastEmployment> GetQueryable();
        IList<LastEmployment> GetAll();
        LastEmployment GetObjectById(int Id);
        LastEmployment CreateObject(LastEmployment lastEmployment);
        LastEmployment CreateObject(string Company, string Title, DateTime StartDate, Nullable<DateTime> EndDate, string ResignReason);
        LastEmployment UpdateObject(LastEmployment lastEmployment);
        LastEmployment SoftDeleteObject(LastEmployment lastEmployment);
        bool DeleteObject(int Id);
    }
}