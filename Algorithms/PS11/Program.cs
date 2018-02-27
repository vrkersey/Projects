using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS11
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            for (int n = 0; n < N; n++)
            {
                int M = int.Parse(Console.ReadLine());
                int[] tokens = new int[M];
                int i = 0;
                foreach (string t in Console.ReadLine().Split(' '))
                    tokens[i++] = int.Parse(t);

                string output="";
                if (workout(tokens, 0, 0, new Dictionary<string, bool>()))
                {
                    int minBudget = 0;
                    while (true)
                    {
                        output = findWorkout(tokens, ++minBudget);
                        if (output != "IMPOSSIBLE")
                        {
                            Console.WriteLine(output);
                            break;
                        }
                    }
                }
                else
                    Console.WriteLine("IMPOSSIBLE");
            }
            Console.ReadLine();

        }

        private static string findWorkout(int[] tokens, int budget)
        {
            String route = "";
            Dictionary<string, bool> cache = new Dictionary<string, bool>();
            if (workout(tokens, budget, 0, 0, cache))
            {
                int height = 0;
                for (int i = 0; i < tokens.Length; i++)
                {
                    string up = (i + 1).ToString() + "-" + (height + tokens[i]);
                    string down = (i + 1).ToString() + "-" + (height - tokens[i]);
                    bool d;
                    if (cache.TryGetValue(down, out d) && d)
                    {
                        height -= tokens[i];
                        route += 'D';
                    }
                    else if (cache.TryGetValue(up, out d) && d)
                    {
                        height += tokens[i];
                        route += 'U';
                    }
                    else
                    {
                        route = "IMPOSSIBLE";
                        break;
                    }
                }
            }
            else
            {
                route = "IMPOSSIBLE";
            }
            return route;
        }

        private static Boolean workout(int[] tokens, int budget, int pos, int height, Dictionary<string, bool> cache)
        {
            string key = pos.ToString() + "-" + height.ToString();
            bool r;
            if (cache.TryGetValue(key, out r))
                return r;

            if (pos == tokens.Length)
            {
                if (height == 0)
                {
                    cache.Add(key, true);
                    return true;
                }
                else
                {
                    cache.Add(key, false);
                    return false;
                }
            }

            bool d = false;

            if (height >= tokens[pos] && height + tokens[pos] <= budget)
            {
                d = workout(tokens, budget, pos + 1, height + tokens[pos], cache) ||
                    workout(tokens, budget, pos + 1, height - tokens[pos], cache);
            }
            else if (height < tokens[pos] && height + tokens[pos] <= budget)
            {
                d = workout(tokens, budget, pos + 1, height + tokens[pos], cache);
            }
            else if (height >= tokens[pos] && height + tokens[pos] > budget)
            {
                d = workout(tokens, budget, pos + 1, height - tokens[pos], cache);
            }
            else if (height < tokens[pos] && height + tokens[pos] > budget)
            {
                d = false;
            }
            cache.Add(key, d);
            return d;
        }

        private static Boolean workout(int[] tokens, int pos, int height, Dictionary<string, bool> cache)
        {
            string key = pos.ToString() + "-" + height.ToString();

            bool r;
            if (cache.TryGetValue(key, out r))
                return r;

            if (pos == tokens.Length)
            {
                if (height == 0)
                {
                    cache.Add(key, true);
                    return true;
                }
                else
                {
                    cache.Add(key, false);
                    return false;
                }
            }

            if (height - tokens[pos] < 0)
            {
                r = workout(tokens, pos + 1, height + tokens[pos], cache);
                cache.Add(key, r);
                return r;
            }
            else
            {
                bool r1 = workout(tokens, pos + 1, height - tokens[pos], cache);
                if (r1)
                    cache.Add(key, r1);

                bool r2 = workout(tokens, pos + 1, height + tokens[pos], cache);
                if (r2 && !r1)
                    cache.Add(key, r1);
                
                return r1 || r2;

            }
        }
    }
}
