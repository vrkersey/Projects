using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS10
{
    class Program
    {
        static int N;
        static void Main(string[] args)
        {
            string[] tokens = Console.ReadLine().Split(' ');
            N = int.Parse(tokens[0]);
            int k = int.Parse(tokens[1]);

            int[,] values = new int[N,2];
            for (int i = 0; i < N; i++)
            {
                tokens = Console.ReadLine().Split(' ');
                values[i,0] = int.Parse(tokens[0]);
                values[i, 1] = int.Parse(tokens[1]);
            }
            Console.WriteLine(maxValue(values, 0, -1, k, new Dictionary<string, int>()));
            Console.Read();
        }

        static int maxValue(int[,] values, int r, int uncloseableRoom, int k, Dictionary<string, int> cache)
        {
            if (r == N)
            {
                return 0;
            }

            int maxV = 0;

            if (cache.TryGetValue(r.ToString() + "," + uncloseableRoom.ToString() + "," + k.ToString(), out maxV))
            {
                return maxV;
            }

            if (k < N - r)
            {
                switch (uncloseableRoom)
                {
                    case -1:
                        {
                            int v1 = values[r, 0] + values[r, 1] + maxValue(values, r + 1, -1, k, cache);
                            int v2 = values[r, 0] + maxValue(values, r + 1, 0, k - 1, cache);
                            int v3 = values[r, 1] + maxValue(values, r + 1, 1, k - 1, cache);
                            maxV = Math.Max(v1, Math.Max(v2,v3));
                        }
                        break;
                    case 0:
                        {
                            int v1 = values[r, 0] + maxValue(values, r + 1, 0, k - 1, cache);
                            int v2 = values[r, 0] + values[r,1] + maxValue(values, r + 1, -1, k, cache);
                            maxV = Math.Max(v1,v2);
                        }
                        break;
                    case 1:
                        {
                            int v1 = values[r, 1] + maxValue(values, r + 1, 1, k - 1, cache);
                            int v2 = values[r,0] + values[r, 1] + maxValue(values, r + 1, -1, k, cache);
                            maxV = Math.Max(v1,v2);
                        }
                        break;
                }
            }
            else if (k == N - r)
            {
                switch (uncloseableRoom)
                {
                    case -1:
                        {
                            int v1 = values[r, 0] + maxValue(values, r + 1, 0, k - 1, cache);
                            int v2 = values[r, 1] + maxValue(values, r + 1, 1, k - 1, cache);
                            maxV = Math.Max(v1,v2);
                        }
                        break;
                    case 0:
                        maxV = values[r, 0] + maxValue(values, r + 1, 0, k - 1, cache);
                        break;
                    case 1:
                        maxV = values[r, 1] + maxValue(values, r + 1, 1, k - 1, cache);
                        break;
                }
            }
            cache.Add(r.ToString() + "," + uncloseableRoom.ToString() + "," + k, maxV);
            return maxV;
        }
    }
}
