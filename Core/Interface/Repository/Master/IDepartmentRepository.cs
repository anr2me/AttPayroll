using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        IQueryable<Department> GetQueryable();
        IList<Department> GetAll();
        Department GetObjectById(int Id);
        Department GetObjectByName(string Name);
        Department CreateObject(Department department);
        Department UpdateObject(Department department);
        Department SoftDeleteObject(Department department);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(Department department);
    }
}