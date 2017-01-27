using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PS1
{
    class Program
    {

        static void Main(string[] args)
        {
            int size = 8;


            //Console.WriteLine(timeAnagram(size,5).ToString("G3"));
            //Console.Read();


            Console.WriteLine("\nSize\tTime (msec)\tDelta (msec)");
            double previousTime = 0;
            for (int i = 0; i <= 8; i++)
            {
                size = size * 2;
                double currentTime = timeAnagram(2000, size);
                Console.Write((size) + "\t" + currentTime.ToString("G3"));
                if (i > 0)
                {
                    Console.WriteLine("   \t" + (currentTime - previousTime).ToString("G3"));
                }
                else
                {
                    Console.WriteLine();
                }
                previousTime = currentTime;
            }
            Console.Read();
        }

        private static string sortword(string word)
        {
            return String.Concat(word.OrderBy(c => c));
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

        private static int anagram(List<string> dictionary)
        {
            HashSet<string> accept = new HashSet<string>();
            HashSet<string> reject = new HashSet<string>();

            int count = 0;

            foreach (string s in sortwords(dictionary))
            {
                if (!reject.Contains(s))
                {
                    if (accept.Contains(s))
                    {
                        accept.Remove(s);
                        reject.Add(s);
                        count--;
                    }
                    else
                    {
                        accept.Add(s);
                        count++;
                    }
                }
            }
            return count;
        }

        private static double timeAnagram(int nSize, int kSize)
        {
            Random r = new Random();
            List<string> dictonary = new List<string>();
            int DURATION = 1000;

            // builds the dictionary of random words
            for (int i = 0; i < nSize; i++)
            {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < kSize; j++)
                    sb.Append((char)('a' + r.Next(0, 26)));
                dictonary.Add(sb.ToString());
                sb.Clear();

            }

            Stopwatch sw = new Stopwatch();

            double elapsed = 0;
            long repetitions = 50;

            for (int i = 0; i < repetitions; i++)
            {
                sw.Restart();
                anagram(dictonary);
                sw.Stop();
                elapsed = msecs(sw);
            }

            return elapsed / repetitions /*/ nSize*/;

            //do
            //{
            //    repetitions *= 2;
            //    sw.Restart();
            //    for (int i = 0; i < repetitions; i++)
            //    {
            //        anagram(dictonary);
            //    }
            //    sw.Stop();
            //    elapsed = msecs(sw);
            //} while (elapsed < DURATION);
            //double totalAverage = elapsed / repetitions /*/ nSize*/;

            //// Create a stopwatch
            //sw = new Stopwatch();

            //// Keep increasing the number of repetitions until one second elapses.
            //elapsed = 0;
            //repetitions = 1;
            //do
            //{
            //    repetitions *= 2;
            //    sw.Restart();
            //    for (int i = 0; i < repetitions; i++)
            //    {
            //        for (int d = 0; d < nSize; d++)
            //        {
            //            //BinarySearch(data, d);
            //        }
            //    }
            //    sw.Stop();
            //    elapsed = msecs(sw);
            //} while (elapsed < DURATION);
            //double overheadAverage = elapsed / repetitions /*/ nSize*/;

            //// Return the difference
            //return totalAverage - overheadAverage;
        }

        private static double msecs(Stopwatch sw)
        {
            return (((double)sw.ElapsedTicks) / Stopwatch.Frequency) * 1000;
        }
        /*
public static double TimeBinarySearch5(int size)
{
   // Construct a sorted array
   int[] data = new int[size];
   for (int i = 0; i < size; i++)
   {
       data[i] = i;
   }

   // Create a stopwatch
   Stopwatch sw = new Stopwatch();

   // Keep increasing the number of repetitions until one second elapses.
   double elapsed = 0;
   long repetitions = 1;
   do
   {
       repetitions *= 2;
       sw.Restart();
       for (int i = 0; i < repetitions; i++)
       {
           for (int d = 0; d < size; d++)
           {
               BinarySearch(data, d);
           }
       }
       sw.Stop();
       elapsed = msecs(sw);
   } while (elapsed < DURATION);
   double totalAverage = elapsed / repetitions / size;

   // Create a stopwatch
   sw = new Stopwatch();

   // Keep increasing the number of repetitions until one second elapses.
   elapsed = 0;
   repetitions = 1;
   do
   {
       repetitions *= 2;
       sw.Restart();
       for (int i = 0; i < repetitions; i++)
       {
           for (int d = 0; d < size; d++)
           {
               //BinarySearch(data, d);
           }
       }
       sw.Stop();
       elapsed = msecs(sw);
   } while (elapsed < DURATION);
   double overheadAverage = elapsed / repetitions / size;

   // Return the difference
   return totalAverage - overheadAverage;
}
*/
    }
}