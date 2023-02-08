using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Integrals
{

    public static string Integral(string formula, ulong precision = 4)
    {
        var tokens = formula.Replace(" ", "").Split('+');

        for (int i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];


        }
    }
}
