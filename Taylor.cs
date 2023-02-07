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

    private static ulong Factorial(ulong n)
    {
        if (n == 0 || n == 1)
            return 1;

        return n * Factorial(n - 1);
    }

    private static double Newton(ulong n, ulong k)
    {
        if (k == 0 || k == n)
            return 1;

        return Newton(n - 1, k - 1) + Newton(n - 1, k);
    }

    private static Polynomial Binomial(string a, double b, ulong n)
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (ulong k = 0; k <= n; ++k)
        {
            modifier = Newton(n, k) * Math.Pow(b, k);
            power = n - k;

            result += string.Format("{0}{1}^{2}+", modifier, a, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Exp(ulong startIndex, ulong count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (ulong i = startIndex; i < count; ++i)
        {
            modifier = 1d / Factorial(i);
            power = i;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Ln(ulong startIndex, ulong count, string argument = "x")
    {
        var elements = new List<Polynomial>();
        var modifier = 0d;

        for (ulong i = startIndex + 1; i < count + 1; ++i)
        {
            modifier = Math.Pow(-1d, i - 1) / i;
            elements.Add(new Polynomial("" + modifier) * Binomial(argument, -1, i));
        }

        var result = elements[0];

        for (int i = 1; i < elements.Count; ++i)
            result += elements[i];

        return result;
    }

    public static Polynomial Sin(ulong startIndex, ulong count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (ulong i = startIndex; i < count; ++i)
        {
            modifier = Math.Pow(-1d, i) / Factorial(2 * i + 1);
            power = 2d * i + 1;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Cos(ulong startIndex, ulong count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (ulong i = startIndex; i < count; ++i)
        {
            modifier = Math.Pow(-1d, i) / Factorial(2 * i);
            power = 2d * i;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Tg(ulong startIndex, ulong count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (ulong i = startIndex + 1; i < count + 1; ++i)
        {
            modifier = B[2 * i] * Math.Pow(-4d, i) * (1 - Math.Pow(4d, i)) / Factorial(2 * i);
            power = 2d * i - 1;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Ctg(ulong startIndex, ulong count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (ulong i = startIndex + 1; i < count + 1; ++i)
        {
            modifier = -Math.Pow(4, i) * B[2 * i] / Factorial(2 * i);
            power = 2d * i - 1;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Arcsin(ulong startIndex, ulong count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (ulong i = startIndex; i < count; ++i)
        {
            modifier = Factorial(2 * i) / (Math.Pow(4d, i) * Math.Pow(Factorial(i), 2) * (2 * i + 1));
            power = 2d * i + 1;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Arccos(ulong startIndex, ulong count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (ulong i = startIndex; i < count; ++i)
        {
            modifier = Factorial(2 * i) / (Math.Pow(4d, i) * Math.Pow(Factorial(i), 2) * (2 * i + 1));
            power = 2d * i + 1;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial("" + Math.PI / 2) - new Polynomial(result);
    }

    public static Polynomial Arctg(ulong startIndex, ulong count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (ulong i = startIndex; i < count; ++i)
        {
            modifier = Math.Pow(-1d, i) / (2d * i + 1);
            power = 2d * i + 1;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial(result);
    }

    public static Polynomial Arcctg(ulong startIndex, ulong count, string argument = "x")
    {
        var result = "";
        var modifier = 0d;
        var power = 0d;

        for (ulong i = startIndex; i < count; ++i)
        {
            modifier = Math.Pow(-1d, i) / (2d * i + 1);
            power = 2d * i + 1;

            result += string.Format("{0}{1}^{2}+", modifier, argument, power);
        }

        result = result.Remove(result.Length - 1);
        return new Polynomial("" + Math.PI / 2) - new Polynomial(result);
    }

    public static string FromTaylor(Polynomial taylor, ulong k = 0, ulong n = 4)
    {
        var elements = taylor.Elements.ToList();

        switch (elements[0])
        {
            case string x when x == Ln(k, n).Elements[0]:
                return "ln x";

            case string x when x == Exp(k, n).Elements[0]:
                return "exp x";

            case string x when x == Sin(k, n).Elements[0]:
                return "sin x";

            case string x when x == Cos(k, n).Elements[0]:
                return "cos x";

            case string x when x == Tg(k, n).Elements[0]:
                return "tg x";

            case string x when x == Ctg(k, n).Elements[0]:
                return "ctg x";

            case string x when x == Arcsin(k, n).Elements[0]:
                return "arcsin x";

            case string x when x == Arccos(k, n).Elements[0]:
                return "arccos x";

            case string x when x == Arctg(k, n).Elements[0]:
                return "arctg x";

            case string x when x == Arcctg(k, n).Elements[0]:
                return "arcctg x";
        }

        var a = PolynomialUtils.ModifierOfElement(elements[0]).Real;
        var b = PolynomialUtils.PowerOfElement(elements[0]);

        switch (elements[0])
        {
            case string x when (a % Ln(k, n - 1).Modifiers[0].Real) == 0 && b % Ln(k, n - 1).Powers[0] == 0:
                return a / Ln(k, n - 1).Modifiers[0].Real + " * ln x";

            case string x when (a % Exp(k, n - 1).Modifiers[0].Real) == 0 && b % Exp(k, n - 1).Powers[0] == 0:
                return a / Exp(k, n - 1).Modifiers[0].Real + " * exp x";

            case string x when (a % Sin(k, n - 1).Modifiers[0].Real) == 0 && b % Sin(k, n - 1).Powers[0] == 0:
                return a / Sin(k, n - 1).Modifiers[0].Real + " * sin x";

            case string x when (a % Cos(k, n - 1).Modifiers[0].Real) == 0 && b % Cos(k, n - 1).Powers[0] == 0:
                return a / Cos(k, n - 1).Modifiers[0].Real + " * cos x";

            case string x when (a % Tg(k, n - 1).Modifiers[0].Real) == 0 && b % Tg(k, n - 1).Powers[0] == 0:
                return a / Tg(k, n - 1).Modifiers[0].Real + " * tg x";

            case string x when (a % Ctg(k, n - 1).Modifiers[0].Real) == 0 && b % Ctg(k, n - 1).Powers[0] == 0:
                return a / Ctg(k, n - 1).Modifiers[0].Real + " * ctg x";

            case string x when (a % Arcsin(k, n - 1).Modifiers[0].Real) == 0 && b % Arcsin(k, n - 1).Powers[0] == 0:
                return a / Arcsin(k, n - 1).Modifiers[0].Real + " * arcsin x";

            case string x when (a % Arccos(k, n - 1).Modifiers[0].Real) == 0 && b % Arccos(k, n - 1).Powers[0] == 0:
                return a / Arccos(k, n - 1).Modifiers[0].Real + " * arccos x";

            case string x when (a % Arctg(k, n - 1).Modifiers[0].Real) == 0 && b % Arctg(k, n - 1).Powers[0] == 0:
                return a / Arctg(k, n - 1).Modifiers[0].Real + " * arctg x";

            case string x when (a % Arcctg(k, n - 1).Modifiers[0].Real) == 0 && b % Arcctg(k, n - 1).Powers[0] == 0:
                return a / Arcctg(k, n - 1).Modifiers[0].Real + " * arcctg x";
        }

        switch (elements[0])
        {
            case string x when (a % Ln(k + 1, n + 1).Modifiers[0].Real) == 0 && b % Ln(k + 1, n + 1).Powers[0] == 0:
                return a / Ln(k + 1, n + 1).Modifiers[0].Real + " * ln x";

            case string x when (a % Exp(k + 1, n + 1).Modifiers[0].Real) == 0 && b % Exp(k + 1, n + 1).Powers[0] == 0:
                return a / Exp(k + 1, n + 1).Modifiers[0].Real + " * exp x";

            case string x when (a % Sin(k + 1, n + 1).Modifiers[0].Real) == 0 && b % Sin(k + 1, n + 1).Powers[0] == 0:
                return a / Sin(k + 1, n + 1).Modifiers[0].Real + " * sin x";

            case string x when (a % Cos(k + 1, n + 1).Modifiers[0].Real) == 0 && b % Cos(k + 1, n + 1).Powers[0] == 0:
                return a / Cos(k + 1, n + 1).Modifiers[0].Real + " * cos x";

            case string x when (a % Tg(k + 1, n + 1).Modifiers[0].Real) == 0 && b % Tg(k + 1, n + 1).Powers[0] == 0:
                return a / Tg(k + 1, n + 1).Modifiers[0].Real + " * tg x";

            case string x when (a % Ctg(k + 1, n + 1).Modifiers[0].Real) == 0 && b % Ctg(k + 1, n + 1).Powers[0] == 0:
                return a / Ctg(k + 1, n + 1).Modifiers[0].Real + " * ctg x";

            case string x when (a % Arcsin(k + 1, n + 1).Modifiers[0].Real) == 0 && b % Arcsin(k + 1, n + 1).Powers[0] == 0:
                return a / Arcsin(k + 1, n + 1).Modifiers[0].Real + " * arcsin x";

            case string x when (a % Arccos(k + 1, n + 1).Modifiers[0].Real) == 0 && b % Arccos(k + 1, n + 1).Powers[0] == 0:
                return a / Arccos(k + 1, n + 1).Modifiers[0].Real + " * arccos x";

            case string x when (a % Arctg(k + 1, n + 1).Modifiers[0].Real) == 0 && b % Arctg(k + 1, n + 1).Powers[0] == 0:
                return a / Arctg(k + 1, n + 1).Modifiers[0].Real + " * arctg x";

            case string x when (a % Arcctg(k + 1, n + 1).Modifiers[0].Real) == 0 && b % Arcctg(k + 1, n + 1).Powers[0] == 0:
                return a / Arcctg(k + 1, n + 1).Modifiers[0].Real + " * arcctg x";
        }

        return taylor.Formula;
    }
}
