using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS7
{
    class Program
    {
        static void Main(string[] args)
        {

            int counter = 561;
            for (int i = 1; i < 561; i++)
            {
                if (i % 3 == 0)
                    counter--;
                else if (i % 11 == 0)
                    counter--;
                else if (i % 13 == 0)
                    counter--;
            }

            string line;
            while((line = Console.ReadLine()) != null)
            {
                string[] tokens = line.Split(null);
                switch (tokens[0])
                {
                    case "gcd":
                        Console.WriteLine(gcd(int.Parse(tokens[1]), int.Parse(tokens[2])));
                        break;
                    case "exp":
                        Console.WriteLine(exp(long.Parse(tokens[1]), long.Parse(tokens[2]), long.Parse(tokens[3])));
                        break;
                    case "inverse":
                        Console.WriteLine(inverse(int.Parse(tokens[1]), int.Parse(tokens[2])));
                        break;
                    case "isprime":
                        Console.WriteLine(isprime(int.Parse(tokens[1])));
                        break;
                    case "key":
                        Console.WriteLine(key(int.Parse(tokens[1]), int.Parse(tokens[2])));
                        break;
                }
            }
        }

        private static string key(long p, long q)
        {
            long N = p * q;
            long g = (p - 1) * (q - 1);
            long e = 1;
            bool stay = true;
            while (stay)
                if (gcd(++e, g) == 1)
                    stay = false;
            long d = long.Parse(inverse(e, g));
            return N.ToString() + " " + e.ToString() + " " + d.ToString();
        }

        private static string isprime(long p)
        {
            int a1,a2,a3;

            a1 = 2;
            a2 = 3;
            a3 = 5;
            if (exp(a1,p-1,p) != 1)
                return "no";
            if (exp(a2, p - 1, p) != 1)
                return "no";
            if (exp(a3, p - 1, p) != 1)
                return "no";
            return "yes";
        }

        private static string inverse(long a, long N)
        {
            
            if (gcd(a, N) != 1)
                return "none";
            long i = N, v = 0, d = 1;
            while (a > 0)
            {
                long t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= N;
            if (v < 0)
                v = (v + N) % N;
            return v.ToString();
        }

        private static long exp(long x, long y, long n)
        {
            if (y == 1)
                return x;

            long prev = exp(x, y / 2, n);

            if (y % 2 == 0)
                return ((prev % n) * (prev % n)) % n;
            else
                return (((((x % n) * (prev % n)) % n) * (prev % n)) % n) % n;
        }

        private static long gcd(long a, long b)
        {

            if (b == 0)
                return a;
            else
                return gcd(b, a % b);
        }
    }
}
