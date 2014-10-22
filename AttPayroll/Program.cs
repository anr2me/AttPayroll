using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context;
using TestValidation;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal Op1 = 2;
            decimal Op2 = 3;
            string Op = "+";

            string vs = Op1.ToString() + Op + Op2.ToString();

            DataTable dt = new DataTable();
            decimal val = decimal.Parse(dt.Compute(vs, string.Empty).ToString());

            Console.WriteLine(vs + " = " + val.ToString());
            Console.WriteLine("2-3 = " + decimal.Parse(dt.Compute("2-3", string.Empty).ToString()));
            Console.WriteLine("2/3 = " + decimal.Parse(dt.Compute("2/3", string.Empty).ToString()));
            Console.WriteLine("2*3 = " + decimal.Parse(dt.Compute("2*3", string.Empty).ToString()));
            Console.WriteLine("2%3 = " + decimal.Parse(dt.Compute("2%3", string.Empty).ToString()));
            Console.WriteLine("3%2 = " + decimal.Parse(dt.Compute("3%2", string.Empty).ToString()));
            //Console.WriteLine("Floor(12663216.000000 / 50000000.0) = " + decimal.Parse(dt.Compute("Floor(12663216.000000 / 50000000.0)", string.Empty).ToString()));
            //Console.ReadLine();

            var db = new AttPayrollEntities();
            using (db)
            {
                db.DeleteAllTables();

                DataBuilder d = new DataBuilder();

                DataFunction(d);
            }
            
        }

        public static void DataFunction(DataBuilder d)
        {
            d.PopulateData();

            d._salaryProcessService.ProcessEmployee(d.emp1.Id, new DateTime(2012, 1, 1));
            // End of Test
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }


    }
}
