using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class BranchOfficeRepository : EfRepository<BranchOffice>, IBranchOfficeRepository
    {
        private AttPayrollEntities entities;
        public BranchOfficeRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<BranchOffice> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<BranchOffice> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public BranchOffice GetObjectById(int Id)
        {
            BranchOffice branchOffice = Find(x => x.Id == Id && !x.IsDeleted);
            if (branchOffice != null) { branchOffice.Errors = new Dictionary<string, string>(); }
            return branchOffice;
        }

        public BranchOffice GetObjectByName(string Name)
        {
            return FindAll(x => x.Name == Name && !x.IsDeleted).FirstOrDefault();
        }

        public BranchOffice CreateObject(BranchOffice branchOffice)
        {
            branchOffice.IsDeleted = false;
            branchOffice.CreatedAt = DateTime.Now;
            return Create(branchOffice);
        }

        public BranchOffice UpdateObject(BranchOffice branchOffice)
        {
            branchOffice.UpdatedAt = DateTime.Now;
            Update(branchOffice);
            return branchOffice;
        }

        public BranchOffice SoftDeleteObject(BranchOffice branchOffice)
        {
            branchOffice.IsDeleted = true;
            branchOffice.DeletedAt = DateTime.Now;
            Update(branchOffice);
            return branchOffice;
        }

        public bool DeleteObject(int Id)
        {
            BranchOffice branchOffice = Find(x => x.Id == Id);
            return (Delete(branchOffice) == 1) ? true : false;
        }

        public bool IsNameDuplicated(BranchOffice branchOffice)
        {
            IQueryable<BranchOffice> branchOffices = FindAll(x => x.Name == branchOffice.Name && !x.IsDeleted && x.Id != branchOffice.Id);
            return (branchOffices.Count() > 0 ? true : false);
        }

    }
}