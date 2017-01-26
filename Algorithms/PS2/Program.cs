using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS2
{
    class Program
    {
        static void Main(string[] args)
        {
            int n, k;
            string s = Console.ReadLine();
            int.TryParse(s.Substring(0, s.IndexOf(" ")), out n);
            int.TryParse(s.Substring(s.IndexOf(" " + 1)), out k);

            for (int i = 0; i < n; i++)
            {
                s = Console.ReadLine();

            }

        }
    }
}
