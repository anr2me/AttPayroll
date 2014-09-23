using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ILastEducationRepository : IRepository<LastEducation>
    {
        IQueryable<LastEducation> GetQueryable();
        IList<LastEducation> GetAll();
        LastEducation GetObjectById(int Id);
        LastEducation CreateObject(LastEducation lastEducation);
        LastEducation UpdateObject(LastEducation lastEducation);
        LastEducation SoftDeleteObject(LastEducation lastEducation);
        bool DeleteObject(int Id);
    }
}