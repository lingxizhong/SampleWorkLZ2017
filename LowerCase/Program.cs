using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS1
{
    class LowerCase
    {
        static void Main(string[] args)
        {
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                string[] original = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < original.Length; i++)
                {
                    original[i] = original[i].ToLower();
                    Console.WriteLine(original[i]);
                }
                                             
            }
        }
    }
}
