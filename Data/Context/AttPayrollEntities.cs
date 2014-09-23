using Core.DomainModel;
using Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using Data.Migrations;
using System.Threading.Tasks;

namespace Data.Context
{
    public class AttPayrollEntities : DbContext
    {
        public AttPayrollEntities() : base("AttPayroll")
        {
            //Database.SetInitializer<AttPayrollEntities>(new DropCreateDatabaseIfModelChanges<AttPayrollEntities>());
            Database.SetInitializer<AttPayrollEntities>(new MigrateDatabaseToLatestVersion<AttPayrollEntities, Configuration>()); // Is this suppose to be inside OnModelCreating ?
       
        }

        public void DeleteAllTables()
        {
            IList<String> tableNames = new List<String>();

            IList<String> utilityNames = new List<String>() 
                                        { "EmployeeLeave", "GeneralLeave", "EmployeeLoanDetail", "EmployeeLoan", 
                                          "PensionCompensation", "ManualAttendance"};

            IList<String> masterNames = new List<String>() 
                                        { "SalarySlipDetail", "SalarySlip", "SalaryEmployeeDetail", "SalaryEmployee",
                                          "SalaryStandardDetail", "SalaryStandard", "OtherExpense", "OtherIncome", "Formula",
                                          "SalaryItem", "PTKP", "PPH21SPT", "WorkingStatus", "LastEducation", "LastEmployment",
                                          "TitleInfo", "Employee", "Division", "Department", "CompanyInfo"};

            IList<String> userRoleNames = new List<String>()
                                        { "UserAccount" };

            utilityNames.ToList().ForEach(x => tableNames.Add(x));
            masterNames.ToList().ForEach(x => tableNames.Add(x));
            userRoleNames.ToList().ForEach(x => tableNames.Add(x));

            foreach (var tableName in tableNames)
            {
                Database.ExecuteSqlCommand(string.Format("DELETE FROM {0}", tableName));
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new SalarySlipDetailMapping());
            modelBuilder.Configurations.Add(new SalarySlipMapping());
            modelBuilder.Configurations.Add(new SalaryEmployeeDetailMapping());
            modelBuilder.Configurations.Add(new SalaryEmployeeMapping());
            modelBuilder.Configurations.Add(new SalaryStandardDetailMapping());
            modelBuilder.Configurations.Add(new SalaryStandardMapping());
            modelBuilder.Configurations.Add(new OtherExpenseMapping());
            modelBuilder.Configurations.Add(new OtherIncomeMapping());
            modelBuilder.Configurations.Add(new SalaryItemMapping());
            modelBuilder.Configurations.Add(new FormulaMapping());
            modelBuilder.Configurations.Add(new EmployeeMapping());
            modelBuilder.Configurations.Add(new LastEmploymentMapping());
            modelBuilder.Configurations.Add(new LastEducationMapping());
            modelBuilder.Configurations.Add(new TitleInfoMapping());
            modelBuilder.Configurations.Add(new DivisionMapping());
            modelBuilder.Configurations.Add(new DepartmentMapping());
            modelBuilder.Configurations.Add(new CompanyInfoMapping());
            modelBuilder.Configurations.Add(new PTKPMapping());
            modelBuilder.Configurations.Add(new PPH21SPTMapping());
            modelBuilder.Configurations.Add(new EmployeeLoanDetailMapping());
            modelBuilder.Configurations.Add(new EmployeeLoanMapping());
            modelBuilder.Configurations.Add(new EmployeeLeaveMapping());
            modelBuilder.Configurations.Add(new GeneralLeaveMapping());
            modelBuilder.Configurations.Add(new ManualAttendanceMapping());
            modelBuilder.Configurations.Add(new PensionCompensationMapping());
            modelBuilder.Configurations.Add(new UserAccountMapping());
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<SalarySlipDetail> SalarySlipDetails { get; set; }
        public DbSet<SalarySlip> SalarySlips { get; set; }
        public DbSet<SalaryEmployeeDetail> SalaryEmployeeDetails { get; set; }
        public DbSet<SalaryEmployee> SalaryEmployees { get; set; }
        public DbSet<SalaryStandardDetail> SalaryStandardDetails { get; set; }
        public DbSet<SalaryStandard> SalaryStandards { get; set; }
        public DbSet<OtherExpense> OtherExpenses { get; set; }
        public DbSet<OtherIncome> OtherIncomes { get; set; }
        public DbSet<SalaryItem> SalaryItems { get; set; }
        public DbSet<Formula> Formulas { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LastEmployment> LastEmployments { get; set; }
        public DbSet<LastEducation> LastEducations { get; set; }
        public DbSet<TitleInfo> TitleInfos { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<CompanyInfo> CompanyInfos { get; set; }
        public DbSet<PTKP> PTKPs { get; set; }
        public DbSet<PPH21SPT> PPH21SPTs { get; set; }
        public DbSet<EmployeeLoanDetail> EmployeeLoanDetails { get; set; }
        public DbSet<EmployeeLoan> EmployeeLoans { get; set; }
        public DbSet<EmployeeLeave> EmployeeLeaves { get; set; }
        public DbSet<GeneralLeave> GeneralLeaves { get; set; }
        public DbSet<ManualAttendance> ManualAttendances { get; set; }
        public DbSet<PensionCompensation> PensionCompensations { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}
