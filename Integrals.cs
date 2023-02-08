using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Integrals
{
    private static List<string> Functions = new() { "ln", "e^", "sin", "cos", "tg", "ctg", "arcsin", "arccos", "arctg", "arcctg" };

    private static string[] SimplifyComplexFunction(string function)
    {
        function = function.Replace("(", ";").Replace(")", "");

        return function.Split(';').Reverse().ToArray();
    }

    private static void AddPluses(ref string formula)
    {
        var s = "";

        for (int i = 0; i < formula.Length; ++i)
        {
            if (formula[i] == '-')
                s += "+" + formula[i];
            else
                s += formula[i];
        }

        formula = s;
    }

    private static void AddSpaces(ref string formula)
    {
        var s = "";

        for (int i = 0; i < formula.Length; ++i)
        {
            if (formula[i] == '*')
                s += formula[i] + " ";
            else
                s += formula[i];
        }

        formula = s;
    }

    private static Complex Value(string formula, Complex x, long k, long n)
    {
        try
        {
            return new Polynomial(formula).Value(x);
        }
        catch (Exception)
        {
            var function = Taylor.FromTaylor(Taylor.ToTaylor(formula, k, n), k, n);
            return Taylor.ToTaylor(function, k, n).Value(x);
        }
    }

    public static string Derivative(string formula, long precision = 4)
    {
        AddPluses(ref formula);
        var tokens = formula.Split('+', StringSplitOptions.RemoveEmptyEntries);
        var result = "";

        for (int i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];

            var functions = SimplifyComplexFunction(token);
            var derivatives = new string[functions.Length];

            for (int j = 0; j < functions.Length; ++j)
            {
                var function = functions[j];

                if (!function.Contains('x'))
                    function += " x";

                try
                {
                    derivatives[j] = PolynomialUtils.Derivative(new Polynomial(function)).Formula;
                }
                catch (Exception)
                {
                    var taylor = Taylor.ToTaylor(function, 0, precision);
                    taylor = PolynomialUtils.Derivative(taylor);
                    derivatives[j] = Taylor.FromTaylor(taylor, 0, precision);
                }

                if (j > 0)
                    derivatives[j] = derivatives[j].Replace("x", "(" + functions[j - 1] + ")");

                result += derivatives[j] + " *";
            }

            result = result.Remove(result.Length - 1) + "+ ";
        }

        result = result.Remove(result.Length - 2, 2);
        AddSpaces(ref result);

        return result;
    }

    public static Complex Derivative(string formula, Complex x, long precision = 4)
    {
        AddPluses(ref formula);
        var tokens = formula.Split('+', StringSplitOptions.RemoveEmptyEntries);
        var result = new Complex(0, 0);

        for (int i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];

            var functions = SimplifyComplexFunction(token);
            var derivatives = new Polynomial[functions.Length];

            for (int j = 0; j < functions.Length; ++j)
            {
                var function = functions[j];

                if (!function.Contains('x'))
                    function += " x";

                try
                {
                    derivatives[j] = PolynomialUtils.Derivative(new Polynomial(function));
                }
                catch (Exception)
                {
                    var taylor = Taylor.ToTaylor(function, 0, precision);
                    derivatives[j] = PolynomialUtils.Derivative(taylor);
                }

                if (j > 0)
                {
                    var t = new Complex(0, 0);
                    
                    try
                    {
                        t = new Polynomial(functions[j - 1]).Value(x);
                    }
                    catch(Exception)
                    {
                        var taylor = Taylor.ToTaylor(functions[j - 1], 0, precision);
                        t = Value(taylor.Formula, x, 0, precision);
                    }

                    derivatives[j] = new Polynomial(Value(derivatives[j].Formula, t, 0, precision).ToString());
                }

                if (j > 0)
                    result += Value(derivatives[j].Formula, x, 0, precision) * Value(derivatives[j - j].Formula, x, 0, precision);
            }
        }

        return result;
    }

    public static Complex Integral (string formula, Complex x, long precision = 4)
    {
        AddPluses(ref formula);
        var tokens = formula.Split('+', StringSplitOptions.RemoveEmptyEntries);
        var result = new Complex(0, 0);

        for (int i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];

            var functions = SimplifyComplexFunction(token);
            var integral = new Complex(0, 0);

            var function = functions[functions.Length - 1];

            if (!function.Contains('x'))
                function += " x";

            var taylor = new Polynomial("0");

            try
            {
                integral = PolynomialUtils.Integral(new Polynomial(function)).Value(x);
                result += integral;
            }
            catch (Exception)
            {
                taylor = Taylor.ToTaylor(function, 0, precision);
                taylor = PolynomialUtils.Integral(taylor);
                integral = Value(Taylor.FromTaylor(taylor, 0, precision), x, 0, precision);

                if (functions.Length == 1)
                    result += integral;
                else
                {
                    var t = "";

                    for (int j = functions.Length - 2; j >= 1; --j)
                        t += functions[j] + "(";

                    t += functions[0];

                    for (int j = 0; j < functions.Length - 1; ++j)
                        t += ")";

                    var dt = new Complex(0, 0);
                    var r = new Complex(0, 0);

                    try
                    {
                        dt = !PolynomialUtils.Derivative(new Polynomial(t)).Value(x);
                        r = new Polynomial(t).Value(x);
                    }
                    catch (Exception)
                    {
                        dt = !Derivative(t, x, precision);

                        try
                        {
                            r = new Polynomial(functions[0]).Value(x);

                            for (int j = 1; j < functions.Length; ++j)
                            {
                                try
                                {
                                    r = new Polynomial(functions[j]).Value(r);
                                }
                                catch (Exception)
                                {
                                    r = Value(Taylor.ToTaylor(functions[j], 0, precision).Formula, r, 0, precision);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            r = Value(Taylor.ToTaylor(functions[0], 0, precision).Formula, x, 0, precision);

                            for (int j = 1; j < functions.Length; ++j)
                            {
                                try
                                {
                                    r = new Polynomial(functions[j]).Value(r);
                                }
                                catch (Exception)
                                {
                                    r = Value(Taylor.ToTaylor(functions[j], 0, precision).Formula, r, 0, precision);
                                }
                            }
                        }
                    }

                    taylor = Taylor.ToTaylor(function, 0, precision);
                    taylor = PolynomialUtils.Integral(taylor);
                    integral = Value(taylor.Formula, r, 0, precision);

                    result += dt * integral;
                }
            }
        }

        return result;
    }

    public static string Integral(string formula, long precision = 4)
    {
        AddPluses(ref formula);
        var tokens = formula.Split('+', StringSplitOptions.RemoveEmptyEntries);
        var result = "";

        for (int i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];

            var functions = SimplifyComplexFunction(token);
            var integral = "";

            var function = functions[functions.Length - 1];

            if (!function.Contains('x'))
                function += " x";

            var taylor = new Polynomial("0");

            try
            {
                integral = PolynomialUtils.Integral(new Polynomial(function)).Formula;
                result += " " + integral + " +";
            }
            catch (Exception)
            {
                taylor = Taylor.ToTaylor(function, 0, precision);
                taylor = PolynomialUtils.Integral(taylor);
                integral = Taylor.FromTaylor(taylor, 0, precision);

                if (functions.Length == 1)
                    result += " " + integral + " +";
                else
                {
                    var t = "";

                    for (int j = functions.Length - 2; j >= 1; --j)
                        t += functions[j] + "(";

                    t += functions[0];

                    for (int j = 0; j < functions.Length - 1; ++j)
                        t += ")";

                    integral = integral.Replace("x", "(" + t);
                    var dt = "";

                    try
                    {
                        dt = " 1 / " + PolynomialUtils.Derivative(new Polynomial(t)).Formula + "";
                    }
                    catch (Exception)
                    {
                        dt = " 1 / " + Derivative(t, precision);
                    }

                    result += dt + " * " + integral + " +";
                }
            }
        }

        result = result.Remove(result.Length - 1).Remove(0, 1);
        AddSpaces(ref result);

        return result;
    }

    public static Complex Integral(string formula, Complex a, Complex b, long precision = 0)
    {
        return Integral(formula, b, precision) - Integral(formula, a, precision);
    }
}
