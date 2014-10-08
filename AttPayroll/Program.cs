using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.ReadLine();
        }
    }
}
