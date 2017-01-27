using System;
using System.Collections.Generic;
using System.Linq;

namespace PS1
{
    class Program
    {

        static void Main(string[] args)
        {
            HashSet<string> accept = new HashSet<string>();
            HashSet<string> reject = new HashSet<string>();

            string s;
            s = Console.ReadLine();
            int n;

            int.TryParse(s.Substring(0, s.IndexOf(" ")), out n);
            //int.TryParse(s.Substring(s.IndexOf(" ") + 1), out k);
            
            for (int i = 0; i < n; i++)
            {
                s = sortword(Console.ReadLine());
                if (!reject.Contains(s))
                {
                    if (accept.Contains(s))
                    {
                        accept.Remove(s);
                        reject.Add(s);
                    }
                    else
                        accept.Add(s);
                }
            }

            Console.WriteLine(accept.Count);
            Console.Read();
        }

        private static string sortword(string word)
        {
            return String.Concat(word.OrderBy(c => c));
        }
    }
}
