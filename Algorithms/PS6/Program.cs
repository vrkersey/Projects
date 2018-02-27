using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS6
{
    class Program
    {
        static void Main(string[] args)
        {
            int n, m;

            string[] tokens = Console.ReadLine().Split(' ');
            n = int.Parse(tokens[0]);
            m = int.Parse(tokens[1]);

            while (n != 0 && m != 0)
            {
                map map = new map(n);
                for (int i = 0; i < m; i++)
                {
                    tokens = Console.ReadLine().Split(' ');
                    int u = int.Parse(tokens[0]);
                    int v = int.Parse(tokens[1]);
                    double w = double.Parse(tokens[2]);
                    map.addLink(u, v, w);
                }
                Console.WriteLine(map.escape().ToString("N4"));

                tokens = Console.ReadLine().Split(' ');
                n = int.Parse(tokens[0]);
                m = int.Parse(tokens[1]);
            }

            Console.Read();
        }
        
        private class link
        {
            public int to;
            public double weight;
            
            public link(int to, double weight)
            {
                this.to = to;
                this.weight = weight;
            }
        }

        private class map
        {
            int V;
            PQ pq;
            List<link>[] adjList;
            public map(int V)
            {
                this.V = V;
                pq = new PQ(V);
                adjList = new List<link>[V];
                for (int i = 0; i < V; i++)
                    adjList[i] = new List<link>();
            }

            public void addLink(int from, int to, double weight)
            {
                adjList[from].Add(new link(to, weight));
                adjList[to].Add(new link(from, weight));
            }
            
            public double escape()
            {
                int starting = 0;

                double[] dist = new double[V];
                int[] prev = new int[V];

                for (int i = 0; i < V; i++)
                {
                    dist[i] = starting;
                    prev[i] = -1;
                }

                pq.add(0, 1);
                dist[0] = 1;

                while (!pq.isEmpty())
                {
                    KeyValuePair<int, double> current = pq.removeMax();
                    foreach (link el in adjList[current.Key])
                    {
                        if (dist[el.to] < dist[current.Key] * el.weight)
                        {
                            dist[el.to] = dist[current.Key] * el.weight;
                            prev[el.to] = current.Key;
                            pq.add(el.to, dist[el.to]);
                        }
                    }
                }
                return dist[V - 1];
            }
        }

        private class PQ
        {
            KeyValuePair<int, double>[] pq;
            int index;
            public PQ(int V)
            {
                pq = new KeyValuePair<int, double>[V];
                index = 0;
            }

            public void add(int i, double d)
            {
                KeyValuePair<int, double> element = new KeyValuePair<int, double>(i, d);
                pq[i] = element;
                index++;
            }

            public KeyValuePair<int, double> removeMax()
            {

                int i = 0;
                double d = pq[0].Value;
                for (int j = 0; j < pq.Length; j++)
                {
                    if (d < pq[j].Value)
                    {
                        d = pq[j].Value;
                        i = j;
                    }
                }

                KeyValuePair<int, double> result = pq[i];
                pq[i] = new KeyValuePair<int, double>(0,0);
                index--;
                return result;
            }

            public bool isEmpty()
            {
                return index == 0;
            }
        }
    }
}
