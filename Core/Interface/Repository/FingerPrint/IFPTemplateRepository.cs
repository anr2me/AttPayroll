using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IFPTemplateRepository : IRepository<FPTemplate>
    {
        IQueryable<FPTemplate> GetQueryable();
        IList<FPTemplate> GetAll();
        FPTemplate GetObjectById(int Id);
        FPTemplate CreateObject(FPTemplate fpTemplate);
        FPTemplate UpdateObject(FPTemplate fpTemplate);
        FPTemplate SoftDeleteObject(FPTemplate fpTemplate);
        bool DeleteObject(int Id);
    }
}