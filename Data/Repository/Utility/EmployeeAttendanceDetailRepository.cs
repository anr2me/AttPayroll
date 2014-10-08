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
    public class EmployeeAttendanceDetailRepository : EfRepository<EmployeeAttendanceDetail>, IEmployeeAttendanceDetailRepository
    {
        private AttPayrollEntities entities;
        public EmployeeAttendanceDetailRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<EmployeeAttendanceDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<EmployeeAttendanceDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public EmployeeAttendanceDetail GetObjectById(int Id)
        {
            EmployeeAttendanceDetail employeeAttendanceDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (employeeAttendanceDetail != null) { employeeAttendanceDetail.Errors = new Dictionary<string, string>(); }
            return employeeAttendanceDetail;
        }

        public EmployeeAttendanceDetail CreateObject(EmployeeAttendanceDetail employeeAttendanceDetail)
        {
            employeeAttendanceDetail.IsDeleted = false;
            employeeAttendanceDetail.CreatedAt = DateTime.Now;
            return Create(employeeAttendanceDetail);
        }

        public EmployeeAttendanceDetail UpdateObject(EmployeeAttendanceDetail employeeAttendanceDetail)
        {
            employeeAttendanceDetail.UpdatedAt = DateTime.Now;
            Update(employeeAttendanceDetail);
            return employeeAttendanceDetail;
        }

        public EmployeeAttendanceDetail SoftDeleteObject(EmployeeAttendanceDetail employeeAttendanceDetail)
        {
            employeeAttendanceDetail.IsDeleted = true;
            employeeAttendanceDetail.DeletedAt = DateTime.Now;
            Update(employeeAttendanceDetail);
            return employeeAttendanceDetail;
        }

        public bool DeleteObject(int Id)
        {
            EmployeeAttendanceDetail employeeAttendanceDetail = Find(x => x.Id == Id);
            return (Delete(employeeAttendanceDetail) == 1) ? true : false;
        }

        
    }
}