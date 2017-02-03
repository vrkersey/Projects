using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS3
{
    class Program
    {

        static void Main(string[] args)
        {
            int x, y;

            string[] input = Console.ReadLine().Split(' ');

            long distanceSQRD = (long)Math.Pow(int.Parse(input[0]),2);
            int k = int.Parse(input[1]);

            HashSet<galaxy> possible = new HashSet<galaxy>();
            HashSet<star> allstars = new HashSet<star>();

            for (int i = 1; i <= k/2; i++)
            {
                input = Console.ReadLine().Split(' ');
                x = int.Parse(input[0]);
                y = int.Parse(input[1]);
                star s1 = new star(x, y);

                input = Console.ReadLine().Split(' ');
                x = int.Parse(input[0]);
                y = int.Parse(input[1]);
                star s2 = new star(x, y);
                allstars.Add(s1);
                allstars.Add(s2);

                if (s1.distanceSQRD(s2) <= distanceSQRD)
                {
                    galaxy g = new galaxy(s1, distanceSQRD);
                    g.addStar(s2);
                    possible.Add(g);
                }
            }
            if (k%2 == 1)
            {
                input = Console.ReadLine().Split(' ');
                x = int.Parse(input[0]);
                y = int.Parse(input[1]);
                star s1 = new star(x, y);
                allstars.Add(s1);
                possible.Add(new galaxy(s1, distanceSQRD));
            }

            Console.WriteLine(majorElement(possible, allstars));
            Console.Read();
        }

        private static string majorElement(HashSet<galaxy> g, HashSet<star> stars)
        {
            int k = stars.Count;
            if (g.Count == 0)
            {
                return "No";
            }
            else
            {
                foreach (galaxy gel in g)
                {
                    foreach (star sel in stars)
                    {
                        if (gel.testStar(sel))
                        {
                            gel.addStar(sel);
                        }
                    }

                    if (gel.count > k / 2)
                    {
                        return gel.count.ToString();
                    }
                }
            }
            return "No";
        }

        private class galaxy
        { 
            private long distanceSQRD;
            star firststar;
            public int count;
            HashSet<star> stars = new HashSet<star>();

            public galaxy(star firststar, long distanceSQRD)
            {
                count = 1;
                this.firststar = firststar;
                stars.Add(firststar);
                this.distanceSQRD = distanceSQRD;
            }

            public void addStar(star s)
            {
                stars.Add(s);
                count++;
            }

            public bool testStar(star s)
            {
                return firststar.distanceSQRD(s) <= distanceSQRD;
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

