using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;

namespace Service.Service
{
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeRepository _repository;
        private IEmployeeValidator _validator;
        public EmployeeService(IEmployeeRepository _employeeRepository, IEmployeeValidator _employeeValidator)
        {
            _repository = _employeeRepository;
            _validator = _employeeValidator;
        }

        public IEmployeeValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Employee> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Employee> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<Employee> GetObjectsByDivisionId(int DivisionId)
        {
            return _repository.FindAll(x => x.DivisionId == DivisionId && !x.IsDeleted).ToList();
        }

        public IList<Employee> GetObjectsByTitleInfoId(int TitleInfoId)
        {
            return _repository.FindAll(x => x.TitleInfoId == TitleInfoId && !x.IsDeleted).ToList();
        }

        //public IList<Employee> GetObjectsByEmployeeWorkingTimeId(int EmployeeWorkingTimeId)
        //{
        //    return _repository.FindAll(x => x.EmployeeWorkingTimeId == EmployeeWorkingTimeId && !x.IsDeleted).ToList();
        //}

        public Employee GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Employee GetObjectByNIK(string NIK)
        {
            return _repository.FindAll(x => x.NIK == NIK && !x.IsDeleted).FirstOrDefault();
        }

        public Employee CreateObject(int divisionId, int titleInfoId, int employeeEducationId, string NIK, string Name, string Address, string PhoneNumber, string Email, string NPWP,
                                     string PlaceOfBirth, DateTime BirthDate, int Sex, int MaritalStatus, int Children, int Religion,
                                     IDivisionService _divisionService, ITitleInfoService _titleInfoService)
        {
            Employee employee = new Employee
            {
                DivisionId = divisionId,
                TitleInfoId = titleInfoId,
                EmployeeEducationId = employeeEducationId,
                NIK = NIK,
                Name = Name,
                Address = Address,
                PhoneNumber = PhoneNumber,
                Email = Email,
                NPWP = NPWP,
                PlaceOfBirth = PlaceOfBirth,
                BirthDate = BirthDate,
                Sex = Sex,
                MaritalStatus = MaritalStatus,
                Children = Children,
                Religion = Religion,
            };
            return this.CreateObject(employee, _divisionService, _titleInfoService);
        }

        public Employee FindOrCreateObject(int divisionId, int titleInfoId, int employeeEducationId, string NIK, string Name, string Address, string PhoneNumber, string Email, string NPWP,
                                     string PlaceOfBirth, DateTime BirthDate, int Sex, int MaritalStatus, int Children, int Religion,
                                     IDivisionService _divisionService, ITitleInfoService _titleInfoService)
        {
            Employee employee = GetObjectByNIK(NIK);
            if (employee != null)
            {
                employee.Errors = new Dictionary<String, String>();
                return employee;
            }
            employee = new Employee
            {
                DivisionId = divisionId,
                TitleInfoId = titleInfoId,
                EmployeeEducationId = employeeEducationId,
                NIK = NIK,
                Name = Name,
                Address = Address,
                PhoneNumber = PhoneNumber,
                Email = Email,
                NPWP = NPWP,
                PlaceOfBirth = PlaceOfBirth,
                BirthDate = BirthDate,
                Sex = Sex,
                MaritalStatus = MaritalStatus,
                Children = Children,
                Religion = Religion,
            };
            return this.CreateObject(employee, _divisionService, _titleInfoService);
        }

        public Employee FindOrCreateObject(Employee employee, IDivisionService _divisionService, ITitleInfoService _titleInfoService)
        {
            employee.Errors = new Dictionary<String, String>();
            Employee obj = GetObjectByNIK(employee.NIK);
            if (obj != null)
            {
                obj.Errors = new Dictionary<String, String>();
                return obj;
            }
            if (_validator.ValidCreateObject(employee, this, _divisionService, _titleInfoService))
            {
                employee.PTKPCode = GetPTKPCode(employee.MaritalStatus != (int)Constant.MaritalStatus.Married, employee.Children);
                _repository.CreateObject(employee);
            }
            return employee;
        }

        public Employee CreateObject(Employee employee, IDivisionService _divisionService, ITitleInfoService _titleInfoService)
        {
            employee.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(employee, this, _divisionService, _titleInfoService)){
                employee.PTKPCode = GetPTKPCode(employee.MaritalStatus != (int)Constant.MaritalStatus.Married, employee.Children);
                _repository.CreateObject(employee);
            }
            return employee;
        }

        public Employee UpdateObject(Employee employee, IDivisionService _divisionService, ITitleInfoService _titleInfoService)
        {
            return (employee = _validator.ValidUpdateObject(employee, this, _divisionService, _titleInfoService) ? _repository.UpdateObject(employee) : employee);
        }

        public Employee SoftDeleteObject(Employee employee)
        {
            return (employee = _validator.ValidDeleteObject(employee) ?
                    _repository.SoftDeleteObject(employee) : employee);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNIKDuplicated(Employee employee)
        {
            IQueryable<Employee> employees = _repository.FindAll(x => x.NIK == employee.NIK && !x.IsDeleted && x.Id != employee.Id);
            return (employees.Count() > 0 ? true : false);
        }

        public string GetPTKPCode(bool Single, int NumberOfChildren)
        {
            string code = null;
            if (Single)
            {
                code = "TK";
                if (NumberOfChildren > 0)
                {
                    code += "_" + NumberOfChildren.ToString("D2");
                }
            }
            else
            {
                code = "KW_" + NumberOfChildren.ToString("D2");
            }
            return code;
        }

    }
}