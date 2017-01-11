using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;
using System.Xml;

namespace console_tester
{
    class Program
    {
        static void Main(string[] args)
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("sum1", "= a1 + a2");
            int i;
            int depth = 100;
            for (i = 1; i <= depth * 2; i += 2)
            {
                s.SetContentsOfCell("a" + i, "= a" + (i + 2) + " + a" + (i + 3));
                s.SetContentsOfCell("a" + (i + 1), "= a" + (i + 2) + "+ a" + (i + 3));
            }
            Console.WriteLine("done");
            Console.Read();
        }

        static void test1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A2", "3");
            s.SetContentsOfCell("A3", "=A4");
            s.SetContentsOfCell("A4", "6");
            Console.WriteLine(s.GetCellValue("A1"));
        }
        static void test2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=a2");
            s.SetContentsOfCell("a2", "=a3");
            s.SetContentsOfCell("a3", "=a4");
            s.SetContentsOfCell("a4", "=a5");
            s.SetContentsOfCell("a5", "=a6");
            s.SetContentsOfCell("a6", "=a7");
            s.SetContentsOfCell("a7", "=a8 + a8");
            s.SetContentsOfCell("a8", "5");
            Console.WriteLine(s.GetCellValue("a1"));
        }

        static void test3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("a2", "2");
            s.SetContentsOfCell("a3", "3");
            s.SetContentsOfCell("a4", "4");
            s.SetContentsOfCell("a5", "=a1*a2");
            s.SetContentsOfCell("a6", "=a3*a4");
            s.SetContentsOfCell("a7", "=a5*a6");
            Console.WriteLine(s.GetCellValue("a6"));
        }
    }
}
