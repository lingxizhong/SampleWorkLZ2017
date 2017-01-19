using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS1
{
    class Format
    {
        static void Main(string[] args)
        {
            string temp = args[0];
            int patternLine;
            Boolean cont = int.TryParse(temp, out patternLine);
            if (cont == false)
            {
                patternLine = 1;
            }
            string sFormat;
            while ((sFormat = Console.ReadLine()) != null)
            {
                string[] original = sFormat.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < original.Length; i++)
                {
                    for (int column = 0; column < patternLine; column++)
                    {

                    }
                }
            }
        }
    }
}
