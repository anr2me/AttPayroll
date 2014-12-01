using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IDepartmentService
    {
        IDepartmentValidator GetValidator();
        IQueryable<Department> GetQueryable();
        IList<Department> GetAll();
        IList<Department> GetObjectsByBranchOfficeId(int BranchOfficeId);
        Department GetObjectById(int Id);
        Department GetObjectByCode(string Code);
        Department GetObjectByName(string Name);
        Department FindOrCreateObject(int branchOfficeId, string Code, string Name, string Description, IBranchOfficeService _branchOfficeService);
        Department CreateObject(Department department, IBranchOfficeService _branchOfficeService);
        Department CreateObject(int branchOfficeId, string Code, string Name, string Description, IBranchOfficeService _branchOfficeService);
        Department UpdateObject(Department department, IBranchOfficeService _branchOfficeService);
        Department SoftDeleteObject(Department department, IDivisionService _divisionService);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(Department department);
        bool IsNameDuplicated(Department department);
    }
}