using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Taylor
{
    private static double[] B = new double[] { 1, 1/2d, 1/6d, 0, -1/30d, 0, 1/42d, 0, -1/30d, 0,
        5/66d, 0, -691/2730d, 0, 7/6d, 0, -3617/510d, 0, 43867/798d, 0, -174611/330d };

    private static string[] ElementaryFunctions = new string[] { "exp", "ln", "sin", "cos", "tg", "ctg" };

    private static string AddSpaces(string s)
    {
        var result = "";

        for (int i = 0; i < s.Length; ++i)
        {
            if (s[i] == '+')
                result += " " + s[i] + " ";
            else
                result += s[i];
        }

        return result;
    }

    private static long Factorial(long n)
    {
        if (n == 0 || n == 1)
            return 1;

        return n * Factorial(n - 1);
    }

    private static double Newton(long n, long k)
    {
        if (k == 0 || k == n)
            return 1;

        return Newton(n - 1, k - 1) + Newton(n - 1, k);
    }

    private static Polynomial Binomial(string a, double b, long n)
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (long k = 0; k <= n; ++k)
        {
            modifier = Newton(n, k) * Math.Pow(b, k);
            power = n - k;

            result += string.Format("{0}{1}^{2}+", modifier, a, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Exp(long startIndex, long count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (long i = startIndex; i < count; ++i)
        {
            modifier = 1d / Factorial(i);
            power = i;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Ln(long startIndex, long count, string argument = "x")
    {
        var elements = new List<Polynomial>();
        var modifier = 0d;

        for (long i = startIndex + 1; i < count + 1; ++i)
        {
            modifier = Math.Pow(-1d, i - 1) / i;
            elements.Add(new Polynomial("" + modifier) * Binomial(argument, -1, i));
        }

        var result = elements[0];

        for (int i = 1; i < elements.Count; ++i)
            result += elements[i];

        return result;
    }

    public static Polynomial Sin(long startIndex, long count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (long i = startIndex; i < count; ++i)
        {
            modifier = Math.Pow(-1d, i) / Factorial(2 * i + 1);
            power = 2d * i + 1;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Cos(long startIndex, long count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (long i = startIndex; i < count; ++i)
        {
            modifier = Math.Pow(-1d, i) / Factorial(2 * i);
            power = 2d * i;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Tg(long startIndex, long count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (long i = startIndex + 1; i < count + 1; ++i)
        {
            modifier = B[2 * i] * Math.Pow(-4d, i) * (1 - Math.Pow(4d, i)) / Factorial(2 * i);
            power = 2d * i - 1;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Ctg(long startIndex, long count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (long i = startIndex + 1; i < count + 1; ++i)
        {
            modifier = -Math.Pow(4, i) * B[2 * i] / Factorial(2 * i);
            power = 2d * i - 1;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Arcsin(long startIndex, long count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (long i = startIndex; i < count; ++i)
        {
            modifier = Factorial(2 * i) / (Math.Pow(4d, i) * Math.Pow(Factorial(i), 2) * (2 * i + 1));
            power = 2d * i + 1;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Arccos(long startIndex, long count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (long i = startIndex; i < count; ++i)
        {
            modifier = Factorial(2 * i) / (Math.Pow(4d, i) * Math.Pow(Factorial(i), 2) * (2 * i + 1));
            power = 2d * i + 1;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial("" + Math.PI / 2) - new Polynomial(result);
    }

    public static Polynomial Arctg(long startIndex, long count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (long i = startIndex; i < count; ++i)
        {
            modifier = Math.Pow(-1d, i) / (2d * i + 1);
            power = 2d * i + 1;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Arcctg(long startIndex, long count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (long i = startIndex; i < count; ++i)
        {
            modifier = Math.Pow(-1d, i) / (2d * i + 1);
            power = 2d * i + 1;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial("" + Math.PI / 2) - new Polynomial(result);
    }

    public static Polynomial ToTaylor(string formula, long k = 0, long n = 4)
    {
        formula = formula.Replace(" ", "");

        switch (formula)
        {
            case "lnx":
                return Ln(k, n);

            case "e^x":
                return Exp(k, n);

            case "sinx":
                return Sin(k, n);

            case "cosx":
                return Cos(k, n);

            case "tgx":
                return Tg(k, n);

            case "ctgx":
                return Ctg(k, n);

            case "arcsinx":
                return Arcsin(k, n);

            case "arccosx":
                return Arccos(k, n);

            case "arctgx":
                return Arctg(k, n);

            case "arcctgx":
                return Arcctg(k, n);
        }

        return new Polynomial(formula);
    }

    public static string FromTaylor(Polynomial taylor, long k = 0, long n = 4)
    {
        var elements = taylor.Elements.ToList();

        for (long j = -1; j <= 1; ++j)
        {
            for (long i = 0; i <= 1; ++i)
            {
                switch (elements[0])
                {
                    case string x when x == Ln(k + i, n + j).Elements[0]:
                        return "ln x";

                    case string x when x == Exp(k + i, n + j).Elements[0]:
                        return "e^x";

                    case string x when x == Sin(k + i, n + j).Elements[0]:
                        return "sin x";

                    case string x when x == Cos(k + i, n + j).Elements[0]:
                        return "cos x";

                    case string x when x == Tg(k + i, n + j).Elements[0]:
                        return "tg x";

                    case string x when x == Ctg(k + i, n + j).Elements[0]:
                        return "ctg x";

                    case string x when x == Arcsin(k + i, n + j).Elements[0]:
                        return "arcsin x";

                    case string x when x == Arccos(k + i, n + j).Elements[0]:
                        return "arccos x";

                    case string x when x == Arctg(k + i, n + j).Elements[0]:
                        return "arctg x";

                    case string x when x == Arcctg(k + i, n + j).Elements[0]:
                        return "arcctg x";
                }
            }
        }

        var a = PolynomialUtils.ModifierOfElement(elements[0]).Real;
        var b = PolynomialUtils.PowerOfElement(elements[0]);

        for (long j = -1; j <= 1; ++j)
        {
            for (long i = 0; i <= 1; ++i)
            {
                switch (elements[0])
                {
                    case string x when (a % Ln(k + i, n + j).Modifiers[0].Real) == 0 && b % Ln(k + i, n + j).Powers[0] == 0:
                        return a / Ln(k + i, n + j).Modifiers[0].Real + " * ln x";

                    case string x when (a % Exp(k + i, n + j).Modifiers[0].Real) == 0 && b % Exp(k + i, n + j).Powers[0] == 0:
                        return a / Exp(k + i, n + j).Modifiers[0].Real + " * e^x";

                    case string x when (a % Sin(k + i, n + j).Modifiers[0].Real) == 0 && b % Sin(k + i, n + j).Powers[0] == 0:
                        return a / Sin(k + i, n + j).Modifiers[0].Real + " * sin x";

                    case string x when (a % Cos(k + i, n + j).Modifiers[0].Real) == 0 && b % Cos(k + i, n + j).Powers[0] == 0:
                        return a / Cos(k + i, n + j).Modifiers[0].Real + " * cos x";

                    case string x when (a % Tg(k + i, n + j).Modifiers[0].Real) == 0 && b % Tg(k + i, n + j).Powers[0] == 0:
                        return a / Tg(k + i, n + j).Modifiers[0].Real + " * tg x";

                    case string x when (a % Ctg(k + i, n + j).Modifiers[0].Real) == 0 && b % Ctg(k + i, n + j).Powers[0] == 0:
                        return a / Ctg(k + i, n + j).Modifiers[0].Real + " * ctg x";

                    case string x when (a % Arcsin(k + i, n + j).Modifiers[0].Real) == 0 && b % Arcsin(k + i, n + j).Powers[0] == 0:
                        return a / Arcsin(k + i, n + j).Modifiers[0].Real + " * arcsin x";

                    case string x when (a % Arccos(k + i, n + j).Modifiers[0].Real) == 0 && b % Arccos(k + i, n + j).Powers[0] == 0:
                        return a / Arccos(k + i, n + j).Modifiers[0].Real + " * arccos x";

                    case string x when (a % Arctg(k + i, n + j).Modifiers[0].Real) == 0 && b % Arctg(k + i, n + j).Powers[0] == 0:
                        return a / Arctg(k + i, n + j).Modifiers[0].Real + " * arctg x";

                    case string x when (a % Arcctg(k + i, n + j).Modifiers[0].Real) == 0 && b % Arcctg(k + i, n + j).Powers[0] == 0:
                        return a / Arcctg(k + i, n + j).Modifiers[0].Real + " * arcctg x";
                }
            }
        }

        return taylor.Formula;
    }
}
