using System;
using System.Collections.Generic;

namespace PS4
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] line = Console.ReadLine().Split(' ');
            int V = int.Parse(line[0]);
            country map = new country(V);
            Dictionary<string, city> dict = new Dictionary<string, city>();
            for(int i = 0; i < V; i++)
            {
                line = Console.ReadLine().Split(' ');
                city c = new city(i, int.Parse(line[1]));
                dict.Add(line[0], c);
            }

            line = Console.ReadLine().Split(' ');
            int E = int.Parse(line[0]);
            for(int i = 0; i < E; i++)
            {
                line = Console.ReadLine().Split(' ');
                map.addRoad(dict[line[0]].v, dict[line[1]].v, dict[line[1]].cost);
            }

            line = Console.ReadLine().Split(' ');
            int t = int.Parse(line[0]);
            string[] results = new string[t];
            for (int i = 0; i < t; i++)
            {
                line = Console.ReadLine().Split(' ');
                int cost = map.sortestPath(dict[line[0]].v, dict[line[1]].v);
                if (cost == 20000001)
                    results[i] = "NO";
                else
                    results[i] = cost.ToString();
                    //Console.WriteLine(cost);
            }
            
            for (int i = 0; i < t; i++)
                Console.WriteLine(results[i]);
            Console.Read();
        }

        private class city
        {
            public int v;
            public int cost;

            public city (int v, int cost)
            {
                this.v = v;
                this.cost = cost;
            }
        }

        private class country
        {
            public int V;
            LinkedList<city>[] cities;

            public country(int v)
            {
                V = v;
                cities = new LinkedList<city>[v];
                for (int i = 0; i < v; i++)
                    cities[i] = new LinkedList<city>();
            }

            public void addRoad(int u, int v, int cost)
            {
                cities[u].AddLast(new city(v, cost));
            }

            private void topSort(int v, bool[] visited, Stack<int> stack)
            {
                visited[v] = true;

                var it = cities[v].GetEnumerator();
                while (it.MoveNext())
                {
                    if (!visited[it.Current.v])
                        topSort(it.Current.v, visited, stack);
                }
                stack.Push(v);
            }

            Stack<int> stack;
            bool[] visited;

            public int sortestPath(int s, int e)
            {
               
                int[] dist = new int[V];

                stack = new Stack<int>();
                visited = new bool[V];
                for (int i = 0; i < V; i++)
                    visited[i] = false;

                for (int i = 0; i < V; i++)
                    if (!visited[i])
                        topSort(i, visited, stack);

                for (int i = 0; i < V; i++)
                    dist[i] = 20000001;
                dist[s] = 0;

                while (stack.Count != 0)
                {
                    int u = (int)stack.Pop();

                    if (dist[u] != 20000001)
                    {
                        var it = cities[u].GetEnumerator();
                        
                        while (it.MoveNext())
                        {
                            if (dist[it.Current.v] > dist[u] + it.Current.cost)
                                dist[it.Current.v] = dist[u] + it.Current.cost;
                        }
                    }
                }
                return dist[e];
            }
        }
    }
}
