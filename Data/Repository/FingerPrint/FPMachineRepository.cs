using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;
using System.Data.Entity;

namespace Data.Repository
{
    public class FPMachineRepository : EfRepository<FPMachine>, IFPMachineRepository
    {
        private AttPayrollEntities entities;
        public FPMachineRepository()
        {
            entities = new AttPayrollEntities();
            //entities.Configuration.ProxyCreationEnabled = false;
        }

        public IQueryable<FPMachine> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<FPMachine> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public FPMachine GetObjectById(int Id)
        {
            FPMachine fpMachine = FindAll(x => x.Id == Id && !x.IsDeleted).Include(x => x.CompanyInfo).FirstOrDefault();
            if (fpMachine != null) { fpMachine.Errors = new Dictionary<string, string>(); }
            return fpMachine;
        }

        public FPMachine CreateObject(FPMachine fpMachine)
        {
            fpMachine.IsDeleted = false;
            fpMachine.CreatedAt = DateTime.Now;
            return Create(fpMachine);
        }

        public FPMachine UpdateObject(FPMachine fpMachine)
        {
            fpMachine.UpdatedAt = DateTime.Now;
            Update(fpMachine);
            return fpMachine;
        }

        public FPMachine SoftDeleteObject(FPMachine fpMachine)
        {
            fpMachine.IsDeleted = true;
            fpMachine.DeletedAt = DateTime.Now;
            Update(fpMachine);
            return fpMachine;
        }

        public bool DeleteObject(int Id)
        {
            FPMachine fpMachine = Find(x => x.Id == Id);
            return (Delete(fpMachine) == 1) ? true : false;
        }

        
    }
}