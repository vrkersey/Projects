using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS3
{
    class Program
    {
        static long distanceSQRD;

        static void Main(string[] args)
        {
            int x, y;

            string[] input = Console.ReadLine().Split(' ');

            distanceSQRD = (long)Math.Pow(int.Parse(input[0]),2);
            int k = int.Parse(input[1]);

            HashSet<star> allstars = new HashSet<star>();

            for (int i = 1; i <= k; i++)
            {
                input = Console.ReadLine().Split(' ');
                x = int.Parse(input[0]);
                y = int.Parse(input[1]);
                star s1 = new star(x, y);
                allstars.Add(s1);
            }

            //Console.WriteLine(majorElement(foo(allstars), allstars));
            Console.WriteLine(foo2(allstars));
            Console.Read();
        }

        private static string foo2(HashSet<star> stars)
        {
            star canidate = null;
            int c = 0;
            foreach (star el in stars)
            {
                if (c == 0)
                {
                    canidate = el;
                    c = 1;
                }
                else if (canidate.distanceSQRD(el) <= distanceSQRD)
                {
                    c++;
                }
                else
                {
                    c--;
                }
            }
            if (c == 0)
                return "NO";
            else
            {
                c = 0;
                foreach (star el in stars)
                    if (canidate.distanceSQRD(el) <= distanceSQRD)
                        c++;
                if (c > stars.Count / 2)
                    return c.ToString();
                else
                    return "NO";
            }
        }
        private static star foo(HashSet<star> stars)
        {
            if (stars.Count == 1)
                return stars.ElementAt(0);
            else if (stars.Count == 0)
                return null;
            else
            {
                HashSet<star> canidates = new HashSet<star>();
                if (stars.Count%2 == 1)
                {
                    canidates.Add(stars.Last());
                }
                for (int i = 1; i < stars.Count; i+=2)
                {
                    if (stars.ElementAt(i-1).distanceSQRD(stars.ElementAt(i)) <= distanceSQRD)
                    {
                        canidates.Add(stars.ElementAt(i - 1));
                    }
                }
                return foo(canidates);
            }
        }

        private static string majorElement(star canidate, HashSet<star> stars)
        {
            int i = 0;
            int c = 0;
            int k = stars.Count;
            if (canidate == null)
            {
                return "No";
            }
            else
            {
                foreach (star el in stars)
                {
                    if (canidate.distanceSQRD(el) <= distanceSQRD)
                        c++;
                    else
                        i++;
                }
                if (c > k / 2)
                    return c.ToString();
                else
                    return "No";
            }
        }

        private class star
        {
            public int x, y;
            public star(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public long distanceSQRD(star s)
            {
                return (long)Math.Pow(Math.Abs(x-s.x),2) + (long)Math.Pow(Math.Abs(y-s.y),2);
            }
        }
    }
}

#if false

        static void Main(string[] args)
        {
            int[] A = new int[] { 2, 5, 7, 22};
            int[] B = new int[] { 7, 8, 9, 14};
            int k = 0;
            Console.WriteLine(select(A,B,k));
            Console.Read();
        }

        static int select(int[] A, int [] B, int k)
        {
            return select(A, 0, A.Length - 1, B, 0, B.Length - 1, k);
        }


        static int select(int[] A, int loA, int hiA, int[] B, int loB, int hiB, int k)
        {
            // A[loA..hiA] is empty
            if (hiA < loA)
                return B[k - loA];

            // B[loB..hiB] is empty
            if (hiB < loB)
                return A[k - loB];

            // Get the midpoints of A[loA..hiA] and B[loB..hiB]
            int i = (loA + hiA) / 2;
            int j = (loB + hiB) / 2;

            // Figure out which one of four cases apply
            if (A[i] > B[j]) 
            {
                if (k <= i + j) 
                    return select(A, loA, i - 1, B, loB, hiB, k);

                else //correct
                    return select(A, loA, hiA, B, j + 1, hiB, k);

            }
            else 
            {
                if (k <= i + j)
                    return select(A, loA, hiA , B, loB, j - 1, k);

                else  //correct
                    return select(A, i + 1, hiA, B, loB, hiB, k);

            }
            
        }
#endif

