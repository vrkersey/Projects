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

        }

        static int dec2bin(string x)
        {
            int n = x.Length;
            if (n == 1)
                return binary(int.Parse(x));
            else
            {
                return dec2bin(x.Substring(0, n / 2)) * pow2bin(n / 2) + dec2bin(x.Substring(n / 2));
            }
            //return 0;
        }

        static int binary(int x)
        {
            switch (x)
            {
                case 0:
                    return 0000;
                case 1:
                    return 0001;
                case 2:
                    return 0010;
                case 3:
                    return 0011;
                case 4:
                    return 0100;
                case 5:
                    return 0101;
                case 6:
                    return 0110;
                case 7:
                    return 0111;
                case 8:
                    return 1000;
                case 9:
                    return 1001;
            }
            return 0;
        }
//        function dec2bin(x)
//        if n = 1: return binary[x]
//        else:
//        split x into two decimal numbers xL, xR with n=2 digits each
//        return ???
    }
}
