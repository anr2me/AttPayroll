using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBranchOfficeRepository : IRepository<BranchOffice>
    {
        IQueryable<BranchOffice> GetQueryable();
        IList<BranchOffice> GetAll();
        BranchOffice GetObjectById(int Id);
        BranchOffice GetObjectByName(string Name);
        BranchOffice CreateObject(BranchOffice company);
        BranchOffice UpdateObject(BranchOffice company);
        BranchOffice SoftDeleteObject(BranchOffice company);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(BranchOffice company);
    }
}