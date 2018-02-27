using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS8
{
    class Program
    {
        static void Main(string[] args)
        {
            int N, T, ci, ti;
            string[] tokens = Console.ReadLine().Split(' ');
            N = int.Parse(tokens[0]);
            T = int.Parse(tokens[1]);

            PQ pq = new PQ(N);
            for (int i = 0; i < N; i++)
            {
                tokens = Console.ReadLine().Split(' ');
                ci = int.Parse(tokens[0]);
                ti = int.Parse(tokens[1]);

                pq.add(ci,ti);
            }

            bool[] timeBlocks = new bool[T];
            for (int i = 0; i < T; i++)
                timeBlocks[i] = false;

            int best = 0;
            while(pq.count != 0)
            {
                KeyValuePair<int, int> person = pq.removeMax();

                for(int i = person.Value; i >= 0; i--)
                {
                    if (!timeBlocks[i])
                    {
                        timeBlocks[i] = true;
                        best += person.Key;
                        break;
                    }
                }
            }

            Console.WriteLine(best);
            Console.Read();
        }
    }

    public class PQ
    {
        KeyValuePair<int, int>[] queue;
        public int count;
        public PQ(int size)
        {
            queue = new KeyValuePair<int, int>[size];
            count = 0;
        }

        public void add(int money, int minutes)
        {
            queue[count] = new KeyValuePair<int, int>(money, minutes);
            count++;
        }

        public KeyValuePair<int,int> removeMax()
        {
            KeyValuePair<int,int> max = new KeyValuePair<int, int>();
            int index = -1;
            for(int i = 0; i < count; i++)
            {
                if (queue[i].Key > max.Key)
                {
                    max = queue[i];
                    index = i;
                }
            }

            queue[index] = queue[count-1];
            queue[count-1] = new KeyValuePair<int, int>();
            count--;

            return max;
        }
    }
}
