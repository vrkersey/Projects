﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PS2
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] s = Console.ReadLine().Split(' ');
            int n = int.Parse(s[0]);
            int k = int.Parse(s[1]);

            HashSet<string> BinaryTrees = new HashSet<string>();

            for (int i = 0; i < n; i++)
            {
                string[] tokens = Console.ReadLine().Split();
                BTree t = new BTree(k);
                for (int j = 0; j < k; j++)
                {
                    t.addNode(int.Parse(tokens[j]));
                }
                BinaryTrees.Add(t.ToString());
                Console.WriteLine(t.ToString());
                Console.WriteLine(BinaryTrees.Count);
            }
            Console.WriteLine(BinaryTrees.Count);
            Console.Read();
        }

        private class BTree
        {
            int[] tree;
            int shape;
            List<int> nodes;

            public BTree(int size)
            {
                nodes = new List<int>();
                int tsize = (int)Math.Pow(2, size) - 1;
                tree = new int[tsize];
            }

            public void addNode(int number)
            {
                int i = 0;
                sink(number, i);
            }

            private void sink(int number, int i)
            {
                if (tree[i] == 0)
                {
                    tree[i] = number;
                    shape = shape + (int)Math.Pow(2, i);
                    nodes.Add(i);
                }
                else if (number > tree[i])
                {
                    i = 2 * i + 2;
                    sink(number, i);
                }
                else
                {
                    i = 2 * i + 1;
                    sink(number, i);
                }
            }

            public override string ToString()
            {
                //return shape;
                nodes.Sort();
                StringBuilder sb = new StringBuilder();
                foreach (int el in nodes)
                    sb.Append(el);
                return sb.ToString();
            }

        }
    }
}
