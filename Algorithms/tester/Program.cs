using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tester
{
    class Program
    {
        static void Main(string[] args)
        {
            DisjointSet s = new DisjointSet(10);
            int a = 0;
            int b = 1;
            int c = 2;
            int d = 3;
            int e = 4;
            int f = 5;
            int g = 6;
            int h = 7;
            int i = 8;
            int j = 9;
            s.union(a, b);
            s.union(c, d);
            s.union(a, d);
            s.union(e, f);
            s.union(g, h);
            s.union(f, h);
            s.union(a, e);
            s.union(i, j);
            s.union(a, j);

            Console.WriteLine("a-" + s.parent[a]);
            Console.WriteLine("b-" + s.parent[b]);
            Console.WriteLine("c-" + s.parent[c]);
            Console.WriteLine("d-" + s.parent[d]);
            Console.WriteLine("e-" + s.parent[e]);
            Console.WriteLine("f-" + s.parent[f]);
            Console.WriteLine("g-" + s.parent[g]);
            Console.WriteLine("h-" + s.parent[h]);
            Console.WriteLine("i-" + s.parent[i]);
            Console.WriteLine("j-" + s.parent[j]);
            //Console.WriteLine((int)a);
            Console.Read();
        }
        public class DisjointSet
        {
            public int[] parent;
            int[] rank;

            public DisjointSet(int n)
            {
                parent = new int[n];
                rank = new int[n];
                for (int i = 0; i < n; i++)
                {
                    parent[i] = i;
                    rank[i] = 0;
                }
            }

            public int find(int x)
            {
                if (x != parent[x])
                    parent[x] = find(parent[x]);
                return parent[x];
            }

            public void union(int x, int y)
            {
                int rx = find(x);
                int ry = find(y);

                if (rx == ry)
                    return;
                else if (rank[rx] > rank[ry])
                    parent[ry] = rx;
                else if (rank[rx] < rank[ry])
                    parent[rx] = ry;
                else
                {
                    if (rx > ry)
                    {
                        parent[ry] = rx;
                        rank[rx] = rank[rx] + 1;
                    }
                    else
                    {
                        parent[rx] = ry;
                        rank[ry] = rank[ry] + 1;
                    }
                }
            }
        }
    }
}
