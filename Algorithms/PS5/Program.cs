using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS5
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, node> students = new Dictionary<string, node>();

            string[] line = Console.ReadLine().Split(' ');

            int s = int.Parse(line[0]);
            map map = new map(s);

            for (int i = 0; i < s; i++)
            {
                line = Console.ReadLine().Split(' ');
                string studentName = line[0];
                node student = new node(studentName, i);
                map.addStudent(student);
                students.Add(studentName, student);
            }

            line = Console.ReadLine().Split(' ');
            int friendships = int.Parse(line[0]);
            for (int i = 0; i < friendships; i++)
            {
                line = Console.ReadLine().Split(' ');
                string friend1 = line[0];
                string friend2 = line[1];

                // add a link from friend1 to friend2
                map.addLink(students[friend1], students[friend2]);
            }

            map.sort();

            line = Console.ReadLine().Split(' ');
            int r = int.Parse(line[0]);
            string[] starters = new string[r];
            for (int i = 0; i < r; i++)
            {
                line = Console.ReadLine().Split(' ');
                string starter = line[0];

                starters[i] = starter;

                foreach (string el in map.betterspread(students[starter]))
                    Console.Write(el + " ");
                Console.WriteLine();
            }
            Console.Read();
        }

        private class node
        {
            public string name;
            public int index;

            public node(string n, int i)
            {
                name = n;
                index = i;
            }
        }

        private class map
        {
            public int V;
            List<node>[] adjList;
            List<node> students;

            public map(int v)
            {
                V = v;
                adjList = new List<node>[v];
                for (int i = 0; i < v; i++)
                    adjList[i] = new List<node>();

                students = new List<node>();
            }

            public void addStudent(node s)
            {
                students.Add(s);
            }

            public void addLink(node s1, node s2)
            {
                adjList[s1.index].Add(s2);
                adjList[s2.index].Add(s1);
            }

            public void sort()
            {
                students.Sort((node1, node2) => node1.name.CompareTo(node2.name));
                for (int i = 0; i < V; i++)
                    adjList[i].Sort((node1, node2) => node1.name.CompareTo(node2.name));
            }

            public Queue<string> spread(node start)
            {
                Queue<string> results = new Queue<string>();
                bool[] visited = new bool[V];
                for (int i = 0; i < V; i++)
                    visited[i] = false;

                Queue<node> neighbors = new Queue<node>();
                neighbors.Enqueue(start);
                visited[start.index] = true;
                
                while (neighbors.Count != 0)
                {
                    node currentNode = neighbors.Dequeue();
                    results.Enqueue(currentNode.name);
                    foreach(node el in adjList[currentNode.index])
                    {
                        if (!visited[el.index])
                        {
                            visited[el.index] = true;
                            neighbors.Enqueue(el);
                        }
                    }
                }

                foreach (node el in students)
                    if (!results.Contains(el.name))
                        results.Enqueue(el.name);

                return results;
            }

            public Queue<string> betterspread(node start)
            {

                Queue<string> results = new Queue<string>();
                List<node> l = new List<node>(students);

                bool[] visited = new bool[V];
                for (int i = 0; i < V; i++)
                    visited[i] = false;

                Queue<node> neighbors = new Queue<node>();
                neighbors.Enqueue(start);

                visited[start.index] = true;

                while (true)
                {
                    List<node> children = new List<node>();
                    while (neighbors.Count != 0)
                    {
                        node current = neighbors.Dequeue();
                        results.Enqueue(current.name);
                        l.Remove(current);
                        foreach (node el in adjList[current.index])
                        {
                            if (!visited[el.index])
                            {
                                children.Add(el);
                                visited[el.index] = true;
                            }
                        }
                    }
                    if (children.Count == 0)
                        break;
                    children.Sort((node1, node2) => node1.name.CompareTo(node2.name));
                    foreach (node el in children)
                        neighbors.Enqueue(el);

                }

                foreach (node el in l)
                    results.Enqueue(el.name);
                return results;
            }
        }
    }
}
