using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS1
{
    class Program
    {
        public static List<string> words;

        static void Main(string[] args)
        {
            words = sortwords(args.ToList());
            //words = new List<string>();
            //words.Add("tape");
            //words.Add("rate");
            //words.Add("seat");
            //words.Add("pate");
            //words.Add("east");
            //words.Add("pest");

            List<string> sorted_words = sortwords(words);

            HashSet<string> accept = new HashSet<string>();
            HashSet<string> reject = new HashSet<string>();

            foreach (string el in sorted_words)
            {
                if (!reject.Contains(el))
                {
                    if (accept.Contains(el))
                    {
                        accept.Remove(el);
                        reject.Add(el);
                    }
                    else
                        accept.Add(el);
                }
            }
            Console.WriteLine(accept.Count);
            Console.Read();
        }

        private static List<string> sortwords(List<string> list)
        {
            List<string> dict = new List<string>();
            foreach (string word in list)
            {
                dict.Add(String.Concat(word.OrderBy(c => c)));
            }

            return dict;
        }
    }
}
