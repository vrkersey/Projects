using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS9
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine()) +1;
            int[] values = new int[n];
            for (int i = 0; i < n; i++)
                values[i] = int.Parse(Console.ReadLine());
            Console.WriteLine(travel(values));
            Console.ReadLine();
        }

        static int travel(int[] values)
        {
            return travel(values, 0, new Dictionary<int, int>());
        }

        static int travel(int[] values, int n, Dictionary<int, int> cache)
        {
            int result;
            if (cache.TryGetValue(n, out result))
                return result;

            if (n == values.Count()-1)
                return 0;

            int minP = singlePenalty(values[n], values[values.Count()-1]);
            for (int i = n; i < values.Count()-1; i++)
            {
                int p = travel(values, i+1, cache) + singlePenalty(values[n], values[i+1]);
                minP = Math.Min(p, minP);
            }
            cache[n] = minP;
            return minP;
        }

        static int singlePenalty(int here, int there)
        {
            int d = 400 - (there - here);
            return d * d;
        }
    }
}
