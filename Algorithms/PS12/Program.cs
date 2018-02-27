using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS12
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            map map = new map(n);

            for (int i = 0; i < n; i++)
            {
                string[] tokens = Console.ReadLine().Split(' ');
                map.addRow(tokens, i);
            }

            Console.WriteLine(map.travel());
            Console.ReadLine();
        }
    }

    class map
    {
        int[,] adj;
        int n;
        public map(int n)
        {
            adj = new int[n, n];
            this.n = n;
        }

        public void addRow(string[] row, int r)
        {
            for (int i = 0; i < n; i++)
                adj[r, i] = int.Parse(row[i]);
        }

        bool[] visited;
        int bestYet;
        public int travel()
        {
            bestYet = int.MaxValue;
            visited = new bool[n];
            for (int i = 0; i < n; i++)
                visited[i] = false;
            visited[0] = true;

            travel(0, 0, 0);
            return bestYet;
        }

        private void travel(int cur, int d, int nodesVisited)
        {
            if (nodesVisited == n)
                bestYet = bestYet > d ? d : bestYet;

            if (nodesVisited == n - 1)
                visited[0] = false;

            for(int i = 0; i < n; i++)
            {
                if (!visited[i])
                {
                    visited[i] = true;
                    travel(i, d + adj[cur, i], nodesVisited + 1);
                }
            }
            
        }
    }
}
