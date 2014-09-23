using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ILastEmploymentRepository : IRepository<LastEmployment>
    {
        IQueryable<LastEmployment> GetQueryable();
        IList<LastEmployment> GetAll();
        LastEmployment GetObjectById(int Id);
        LastEmployment CreateObject(LastEmployment lastEmployment);
        LastEmployment UpdateObject(LastEmployment lastEmployment);
        LastEmployment SoftDeleteObject(LastEmployment lastEmployment);
        bool DeleteObject(int Id);
    }
}