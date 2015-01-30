using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Data.Context;
using Core.Constants;
using Core.DomainModel;
using Core.Interface.Service;
using Service.Service;
using Data.Repository;
using Validation.Validation;
using System.Text.RegularExpressions;

namespace WebView
{
    // connectionString info at http://connectionstrings.com/excel-2007
    public class ConversionFunction
    {
        public ICompanyInfoService _companyInfoService = new CompanyInfoService(new CompanyInfoRepository(), new CompanyInfoValidator());
        public IBranchOfficeService _branchOfficeService = new BranchOfficeService(new BranchOfficeRepository(), new BranchOfficeValidator());
        public IDepartmentService _departmentService = new DepartmentService(new DepartmentRepository(), new DepartmentValidator());
        public IDivisionService _divisionService = new DivisionService(new DivisionRepository(), new DivisionValidator());
        public IEmployeeService _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
        public ITitleInfoService _titleInfoService = new TitleInfoService(new TitleInfoRepository(), new TitleInfoValidator());
        public ISalaryStandardService _salaryStandardService = new SalaryStandardService(new SalaryStandardRepository(), new SalaryStandardValidator());
        public ISalaryStandardDetailService _salaryStandardDetailService = new SalaryStandardDetailService(new SalaryStandardDetailRepository(), new SalaryStandardDetailValidator());
        public ISalaryEmployeeService _salaryEmployeeService = new SalaryEmployeeService(new SalaryEmployeeRepository(), new SalaryEmployeeValidator());
        public ISalaryEmployeeDetailService _salaryEmployeeDetailService = new SalaryEmployeeDetailService(new SalaryEmployeeDetailRepository(), new SalaryEmployeeDetailValidator());

        public ConversionFunction()
        {
            //_companyInfoService = new CompanyInfoService(new CompanyInfoRepository(), new CompanyInfoValidator());
            //_branchOfficeService = new BranchOfficeService(new BranchOfficeRepository(), new BranchOfficeValidator());
            //_departmentService = new DepartmentService(new DepartmentRepository(), new DepartmentValidator());
            //_divisionService = new DivisionService(new DivisionRepository(), new DivisionValidator());
            //_employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
            //_titleInfoService = new TitleInfoService(new TitleInfoRepository(), new TitleInfoValidator());
        }

        public void Log(dynamic model, string sheet, int row)
        {
            foreach (var error in model.Errors)
            {
                Console.WriteLine(model.GetType().Name + "[ID:" + model.Id + ", SHEET:" + sheet + ", ROW:" + row + "] : " + error.Key + " " + error.Value);
            }
        }

        public string GetValidNumber(string str)
        {
            Regex re = new Regex(@"([\d|\.]+)");
            Match result = re.Match(str);
            string ret = result.Groups[1].Value;
            if (ret == null || ret.Trim() == "") ret = "0";
            return ret;
        }

        public int DoBranchOffice(OleDbDataReader dr, DbContext db)
        {
            int count = 0;
            while (dr.Read()) // read per record/row from a table/sheet
            {
                BranchOffice obj = new BranchOffice
                {
                    Code = dr.GetString(0),
                    Name = dr.GetString(1),
                };
                if (!_branchOfficeService.CreateObject(obj, _companyInfoService).Errors.Any())
                {
                    count++;
                };
            }
            return count;
        }

        public int DoDepartment(OleDbDataReader dr, DbContext db)
        {
            int count = 0;
            while (dr.Read()) // read per record/row from a table/sheet
            {
                BranchOffice b = _branchOfficeService.GetObjectByName(dr.GetString(2)); // column 2 = branch name
                Department obj = new Department
                {
                    Code = dr.GetString(0),
                    Name = dr.GetString(1),
                    BranchOfficeId = b.Id,
                };
                if (!_departmentService.CreateObject(obj, _branchOfficeService).Errors.Any())
                {
                    count++;
                    Division d = new Division
                    {
                        Code = obj.Code,
                        Name = obj.Name,
                        DepartmentId = obj.Id,
                    };
                    _divisionService.CreateObject(d, _departmentService);
                };
            }
            return count;
        }

        public int DoEmployee(OleDbDataReader dr, DbContext db)
        {
            int count = 0;
            while (dr.Read()) // read per record/row from a table/sheet
            {

            }
            return count;
        }

        public void DoCustomer(DataRow row, int rowidx, string sheetname)
        {
            //var desc = row[0].ToString();
            //var name = row[1].ToString();
            //var addr = row[2].ToString();
            //var defaddr = row[3].ToString();
            //var defterm = Convert.ToInt32(GetValidNumber(row[4].ToString())); //int.Parse(row[4].ToString());
            //var phone = row[5].ToString();
            //var npwp = row[7].ToString();
            //var tax = row[8].ToString();
            //var email = row[10].ToString();
            //var status = row[15].ToString();
            //var obj = _contactService.GetQueryable().Where(x => x.Name == name /*&& x.Description == desc && x.NPWP == npwp*/ && !x.IsDeleted).FirstOrDefault();
            //if (obj == null)
            //{
            //    obj = new Contact()
            //    {
            //        Name = name, //(desc.Length > name.Length) ? desc : name,
            //        Description = desc, //(desc.Length > name.Length) ? name : desc,
            //        Address = (addr == null || addr.Trim() == "") ? "-" : addr,
            //        DeliveryAddress = defaddr,
            //        DefaultPaymentTerm = defterm,
            //        ContactNo = (phone == null || phone.Trim() == "") ? "-" : phone,
            //        NPWP = npwp,
            //        TaxCode = (tax == null || tax.Trim() == "") ? "01" : tax,
            //        IsTaxable = (tax != null && tax.Trim() != ""),
            //        Email = email,
            //        ContactType = "CUSTOMER", //status,

            //    };
            //    obj = _contactService.CreateObject(obj);
            //    Log(obj, sheetname + "Create", rowidx + 2);
            //}
            //else
            //{
            //    obj.ContactType = "CUSTOMER";
            //    obj.Errors = new Dictionary<string, string>();
            //    obj = _contactService.UpdateObject(obj);
            //    Log(obj, sheetname + "Update", rowidx + 2);
            //}
        }

        // Example 1
        public int ImporttoSQL(string sPath)
        {
            // Note : HDR=Yes indicates that the first row contains column names, not data. HDR=No indicates the opposite.
            // Connect to Excel 2007 earlier version
            //string sSourceConstr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=C:\AgentList.xls; Extended Properties=""Excel 8.0;HDR=YES;""";
            // Connect to Excel 2007 (and later) files with the Xlsx file extension 
            string sSourceConstr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=1;""", sPath); //IMEX=1

            //string sDestConstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; // "DBConnection"
            int count = 0;
            OleDbConnection sSourceConnection = new OleDbConnection(sSourceConstr);
            //DbContext db = new DbContext(sDestConstr);
            using (var db = new AttPayrollEntities())
            {
                using (sSourceConnection)
                {
                    sSourceConnection.Open();
                    // Get Sheets list
                    DataTable dt = sSourceConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    //String[] strExcelSheets = new String[dt.Rows.Count];
                    //int i = 0;
                    // Add the sheet name to the string array.
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        //strExcelSheets[i] = row["TABLE_NAME"].ToString();
                        //i++;

                        string sheetname = dtrow["TABLE_NAME"].ToString(); // string.Format("{0}", "Sheet1$");
                        string sql = string.Format("Select [Employee ID],[Employee Name],[Designation],[Posting],[Dept] FROM [{0}]", "Sheet1$");
                        OleDbCommand command = new OleDbCommand(sql, sSourceConnection);
                        sSourceConnection.Open();
                        using (OleDbDataReader dr = command.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                switch (sheetname)
                                {
                                    case Constant.SheetName.BranchOffice :
                                        count += DoBranchOffice(dr, db);
                                        break;
                                    case Constant.SheetName.Department:
                                        count += DoDepartment(dr, db);
                                        break;
                                }

                                //DataTable schemaTable = dr.GetSchemaTable(); // read the whole table/sheet
                                //foreach (DataRow row in schemaTable.Rows)
                                //{
                                //    foreach (DataColumn column in schemaTable.Columns)
                                //    {

                                //    }
                                //}

                                //using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sDestConstr)) // copy the whole table/sheet
                                //{
                                //    bulkCopy.DestinationTableName = "Employee";
                                //    //You can mannualy set the column mapping by the following way.
                                //    //bulkCopy.ColumnMappings.Add("Employee ID", "Employee Code");
                                //    bulkCopy.WriteToServer(dr);
                                //}
                            }
                            if (dr != null)
                            {
                                dr.Close();
                                dr.Dispose();
                            }
                        }
                    }
                    if (dt != null)
                    {
                        dt.Dispose();
                    }
                    //if (sSourceConnection != null)
                    //{
                    //    sSourceConnection.Close();
                    //}
                }
                if (db != null)
                {
                    db.Dispose();
                }
            }
            return count;
        }

        // Example 2
        public int ImportFromExcel(string fileName)
        {
            // Buat satu Service menggunakan nama ExcelEntryService
            // yang memiliki Repository, dimana GetContext dapat digunakan

            // membaca setiap nama sheet dan link dengan nama table di database / domain model
            // setiap table mengarah ke service terkait, menggunakan fungsi CreateObject
            // 
            int count = 0;
            //DbContext conLinq = new DbContext("Data Source=server name;Initial Catalog=Database Name;Integrated Security=true");
            using (var conLinq = new AttPayrollEntities())
            {
                try
                {
                    DataTable dtExcel = new DataTable();
                    // Note : HDR=Yes indicates that the first row contains column names, not data. HDR=No indicates the opposite.
                    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + fileName + "';Extended Properties= 'Excel 12.0;HDR=Yes;IMEX=1'";
                    OleDbConnection conn = new OleDbConnection(SourceConstr);
                    string query = "Select * from [Sheet1$]";
                    OleDbDataAdapter data = new OleDbDataAdapter(query, conn);
                    conn.Open();
                    data.Fill(dtExcel);
                    for (int i = 0; i < dtExcel.Rows.Count; i++)
                    {
                        try
                        {
                            count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                    if (count == dtExcel.Rows.Count)
                    {
                        //<--Success Message-->
                    }
                    else
                    {
                        //<--Failure Message-->
                    }
                    dtExcel.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conLinq.Dispose();
                }
            }
            return count;
        }

        public int ImportDataFromExcel(string fileName)
        {
            // Buat satu Service menggunakan nama ExcelEntryService
            // yang memiliki Repository, dimana GetContext dapat digunakan

            // membaca setiap nama sheet dan link dengan nama table di database / domain model
            // setiap table mengarah ke service terkait, menggunakan fungsi CreateObject
            // 
            int count = 0;
            //DbContext conLinq = new DbContext("Data Source=server name;Initial Catalog=Database Name;Integrated Security=true");
            using (var conLinq = new AttPayrollEntities())
            {
                //try
                {
                    DataSet dtExcel = new DataSet();
                    // Note : HDR=Yes indicates that the first row contains column names, not data. HDR=No indicates the opposite.
                    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + fileName + "';Extended Properties= 'Excel 12.0;HDR=Yes;IMEX=1'";
                    OleDbConnection conn = new OleDbConnection(SourceConstr);
                    conn.Open();
                    // Get Sheets list
                    DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    string sheetname = "";
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        sheetname = dtrow["TABLE_NAME"].ToString(); //.Trim(new[] { '$' }); // string.Format("{0}", "Sheet1$");
                        string query = string.Format("Select * from [{0}]", sheetname);
                        // Create OleDbCommand object and select data from worksheet Sheet1
                        OleDbCommand cmd = new OleDbCommand(query, conn);
                        // Create new OleDbDataAdapter
                        OleDbDataAdapter oleda = new OleDbDataAdapter();
                        oleda.SelectCommand = cmd;
                        // Fill the DataSet from the data extracted from the worksheet.
                        oleda.Fill(dtExcel, sheetname);
                        oleda.Dispose();
                        cmd.Dispose();
                    }

                    //sheetname = "Customer$";
                    //for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    //{
                    //    //try
                    //    {
                    //        //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                    //        // Find Or Create Object
                    //        var row = dtExcel.Tables[sheetname].Rows[i];
                    //        var tmp = row[0].ToString() + row[1].ToString();
                    //        if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                    //        DoCustomer(row, i, sheetname);
                    //    }
                    //    //catch (Exception ex)
                    //    //{
                    //    //    Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                    //    //    continue;
                    //    //}
                    //}

                    //oleda.Dispose();
                    //foreach (var x in data) x.Dispose();
                    //data.Clear();
                    dtExcel.Dispose();
                }
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
                //finally
                {
                    conLinq.Dispose();
                }
            }
            return count;
        }

        public int ImportEmployeeFromExcel(string fileName)
        {
            // Buat satu Service menggunakan nama ExcelEntryService
            // yang memiliki Repository, dimana GetContext dapat digunakan

            // membaca setiap nama sheet dan link dengan nama table di database / domain model
            // setiap table mengarah ke service terkait, menggunakan fungsi CreateObject
            // 
            int count = 0;
            //DbContext conLinq = new DbContext("Data Source=server name;Initial Catalog=Database Name;Integrated Security=true");
            using (var conLinq = new AttPayrollEntities())
            {
                try
                {
                    DataTable dtExcel = new DataTable();
                    // Note : HDR=Yes indicates that the first row contains column names, not data. HDR=No indicates the opposite.
                    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + fileName + "';Extended Properties= 'Excel 12.0;HDR=Yes;IMEX=1'";
                    OleDbConnection conn = new OleDbConnection(SourceConstr);
                    conn.Open();
                    // Get Sheets list
                    DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string sheetname = dt.Rows[0]["TABLE_NAME"].ToString(); // string.Format("{0}", "Sheet1$");
                    string query = string.Format("Select * from [{0}]", sheetname);
                    OleDbDataAdapter data = new OleDbDataAdapter(query, conn);
                    
                    data.Fill(dtExcel);
                    for (int i = 0; i < dtExcel.Rows.Count; i++)
                    {
                        try
                        {
                            //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                            // Find Or Create Branch
                            var branchcode = dtExcel.Rows[i][4].ToString();
                            BranchOffice branchOffice = _branchOfficeService.FindOrCreateObject(branchcode.Replace(" ", String.Empty), branchcode, "-", "-", "-", "-", "-", "-", _companyInfoService);

                            // Find Or Create Department & Division
                            var deptcode = dtExcel.Rows[i][5].ToString();
                            Department department = _departmentService.FindOrCreateObject(branchOffice.Id, deptcode.Replace(" ", String.Empty), deptcode, "", _branchOfficeService);
                            Division division = _divisionService.FindOrCreateObject(department.Id, deptcode.Replace(" ", String.Empty), deptcode, "", _departmentService);

                            // Find Or Create Title
                            var titlename = dtExcel.Rows[i][3].ToString();
                            TitleInfo titleInfo = _titleInfoService.FindOrCreateObject(titlename.Replace(" ", String.Empty), titlename, "", false);

                            // Find Or Create Employee
                            Employee employee = new Employee()
                            {
                                DivisionId = division.Id,
                                TitleInfoId = titleInfo.Id,
                                NIK = dtExcel.Rows[i][0].ToString(),
                                StartWorkingDate = DateTime.Parse(dtExcel.Rows[i][1].ToString()),
                                Name = dtExcel.Rows[i][2].ToString(),
                                BankAccount = dtExcel.Rows[i][6].ToString(),
                                Bank = dtExcel.Rows[i][7].ToString(),
                                BirthDate = DateTime.Parse(dtExcel.Rows[i][20].ToString()),
                                PlaceOfBirth = "-",
                                PTKPCode = dtExcel.Rows[i][22].ToString(),
                                NPWP = dtExcel.Rows[i][23].ToString(),
                                Religion = (int)Enum.Parse(typeof(Constant.Religion), dtExcel.Rows[i][26].ToString(), true),
                                Address = dtExcel.Rows[i][28].ToString(),
                                IDNumber = dtExcel.Rows[i][29].ToString(),
                                Sex = dtExcel.Rows[i][27].ToString().Substring(0,1) == "F" ? 1 : 0,
                                MaritalStatus = dtExcel.Rows[i][22].ToString().Substring(0,1) == "K" ? 1 : 0,
                                Children = int.Parse(dtExcel.Rows[i][22].ToString().Substring(dtExcel.Rows[i][22].ToString().IndexOf("/"))),
                                PhoneNumber = "-",
                            };
                            _employeeService.FindOrCreateObject(employee, _divisionService, _titleInfoService);

                            // Find Or Create SalaryStandard

                            // Find Or Create SalaryEmployee

                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                    if (count == dtExcel.Rows.Count)
                    {
                        //<--Success Message-->
                    }
                    else
                    {
                        //<--Failure Message-->
                    }
                    dtExcel.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conLinq.Dispose();
                }
            }
            return count;
        }
    }
}
