using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IFPMachineRepository : IRepository<FPMachine>
    {
        IQueryable<FPMachine> GetQueryable();
        IList<FPMachine> GetAll();
        FPMachine GetObjectById(int Id);
        FPMachine CreateObject(FPMachine fpMachine);
        FPMachine UpdateObject(FPMachine fpMachine);
        FPMachine SoftDeleteObject(FPMachine fpMachine);
        bool DeleteObject(int Id);
    }
}