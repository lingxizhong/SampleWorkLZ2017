using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS1
{
    class Palindrome
    {
        static void Main(string[] args)
        {
            String var;
            while ((var = Console.ReadLine()) != null)
            {
                char[] original = var.ToCharArray();
                char[] temp = var.ToCharArray();
                Array.Reverse(temp);
                Boolean flagCheck = true;
                for (int index = 0; index < original.Length; index++)
                {
                    if (flagCheck == true)
                    {
                        if (!original[index].Equals(temp[index]))
                        {
                            flagCheck = false;
                        }
                    }
                }
                if (flagCheck == true)
                {
                    Console.WriteLine(var);
                }

            }
        }
    }
}
