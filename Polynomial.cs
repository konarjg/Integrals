using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PolynomialUtils
{
    public static Complex ModifierOfElement(string element)
    {
        if (element.Contains('x'))
        {
            if (element.Contains('^'))
            {
                var n = PowerOfElement(element);

                if (element == string.Format("x^{0}", n))
                    return new Complex(1, 0);
                else if (element == string.Format("-x^{0}", n))
                    return new Complex(-1, 0);

                return Complex.Parse(element.Split('x')[0]);
            }

            if (element == "x")
                return new Complex(1, 0);
            else if (element == "-x")
                return new Complex(-1, 0);

            return Complex.Parse(element.Split('x')[0]);
        }

        return Complex.Parse(element);
    }

    public static string ParseElement(Complex a, double n)
    {
        if (n == 0)
            return a.ToString();
        else if (n == 1)
        {
            if (a.Real == 1 && a.Imaginal == 0)
                return "x";
            else if (a.Real == -1 && a.Imaginal == 0)
                return "-x";

            return string.Format("{0}x", a);
        }

        if (a.Real == 1 && a.Imaginal == 0)
            return string.Format("x^{0}", n);
        else if (a.Real == -1 && a.Imaginal == 0)
            return string.Format("-x^{0}", n);

        return string.Format("{0}x^{1}", a, n);
    }

    public static double PowerOfElement(string element)
    {
        if (element.Contains('x'))
        {
            if (element.Contains('^'))
                return double.Parse(element.Split('^')[1]);

            return 1;
        }

        return 0;
    }

    public static void AddPluses(ref string formula)
    {
        var s = "";

        for (int i = 0; i < formula.Length; ++i)
        {
            if (formula[i] == '-' && i - 1 >= 0 && formula[i - 1] != '<' && formula[i - 1] != ';' && formula[i - 1] != 'E')
                s += "+" + formula[i];
            else
                s += formula[i];
        }

        formula = s;
    }

    public static void AddSpaces(ref string formula)
    {
        var s = "";

        for (int i = 0; i < formula.Length; ++i)
        {
            if (formula[i] == '+')
                s += " " + formula[i] + " ";
            else if (formula[i] == ';' && formula[i + 1] != ' ')
                s += formula[i] + " ";
            else
                s += formula[i];
        }

        formula = s;
    }

    public static int CompareElements(string a, string b)
    {
        if (PowerOfElement(a) < PowerOfElement(b))
            return 1;
        else if (PowerOfElement(a) > PowerOfElement(b))
            return -1;

        return 0;
    }

    public static string[] PrepareForHorner(Polynomial a)
    {
        var tokens = a.Elements.ToList();

        int k = 0;

        for (double n = a.MaxPower; n >= 2; --n)
        {
            if (tokens.Find(x => PowerOfElement(x) == n) == null)
                tokens.Insert(k, string.Format("0x^{0}", n));

            ++k;
        }

        if (tokens.Find(x => PowerOfElement(x) == 1) == null)
        {
            tokens.Insert(k, "0x");
            ++k;
        }

        if (tokens.Find(x => PowerOfElement(x) == 0) == null)
            tokens.Insert(k, "0");

        return tokens.ToArray();
    }

    public static Polynomial Derivative(Polynomial a)
    {
        var modifiers = new List<Complex>();
        var powers = new List<double>();
        var modifier = new Complex(1, 0);
        var power = 0d;
        var formula = "";

        for (int i = 0; i < a.Elements.Length; ++i)
        {
            power = a.Powers[i];
            modifier = a.Modifiers[i];

            if (power == 0)
                continue;

            modifier *= power;
            --power;

            powers.Add(power);
            modifiers.Add(modifier);
        }

        for (int i = 0; i < powers.Count; ++i)
            formula += ParseElement(modifiers[i], powers[i]) + "+";

        formula = formula.Remove(formula.Length - 1);
        AddSpaces(ref formula);

        return new Polynomial(formula);
    }

    public static Polynomial Integral(Polynomial a)
    {
        var modifiers = new List<Complex>();
        var powers = new List<double>();
        var modifier = new Complex(1, 0);
        var power = 0d;
        var formula = "";

        for (int i = 0; i < a.Elements.Length; ++i)
        {
            power = a.Powers[i];
            modifier = a.Modifiers[i];

            ++power;
            modifier /= power;

            powers.Add(power);
            modifiers.Add(modifier);
        }

        for (int i = 0; i < powers.Count; ++i)
            formula += ParseElement(modifiers[i], powers[i]) + "+";

        formula = formula.Remove(formula.Length - 1);
        AddSpaces(ref formula);

        return new Polynomial(formula);
    }

    public static Polynomial Horner(Polynomial a, Polynomial b)
    {
        var result = new List<Complex>();
        var formula = "";
        var x0 = -ModifierOfElement(b.Elements[1]);
        var elements = PrepareForHorner(a);

        result.Add(ModifierOfElement(a.Elements[0]));
        var k = 1;
        var n = a.MaxPower - 1; 

        for (int i = 0; i < elements.Length; ++i)
        {
            var z = x0 * result[k - 1] + ModifierOfElement(elements[i]);

            if (n == 0)
                break;

            result.Add(z);
            ++k;
            --n;
        }

        n = a.MaxPower - 1;

        for (int i = 0; i < result.Count; ++i)
        {
            if (Math.Round(result[i].Real) == 0 && Math.Round(result[i].Imaginal) == 0)
                continue;

            formula += ParseElement(result[i], n) + "+";
            --n;
        }

        formula = formula.Remove(formula.Length - 1);
        AddSpaces(ref formula);

        return new Polynomial(formula);
    }

    public static string DivideElements(string x, string y)
    {
        var a1 = ModifierOfElement(x);
        var a2 = ModifierOfElement(y);
        var n1 = PowerOfElement(x);
        var n2 = PowerOfElement(y);

        a1 /= a2;
        n1 -= n2;

        return ParseElement(a1, n1);
    }

    public static string MultiplyElements(string x, string y)
    {
        var a1 = ModifierOfElement(x);
        var a2 = ModifierOfElement(y);
        var n1 = PowerOfElement(x);
        var n2 = PowerOfElement(y);

        a1 *= a2;
        n1 += n2;

        return ParseElement(a1, n1);
    }

    public static string[] Multiply(string x, string[] y)
    {
        var a1 = ModifierOfElement(x);
        var n1 = PowerOfElement(x);
        
        for (int i = 0; i < y.Length; ++i)
        {
            var a = ModifierOfElement(y[i]);
            var n = PowerOfElement(y[i]);

            a *= a1;
            n += n1;

            y[i] = ParseElement(-a, n);
        }

        return y;
    }

    public static string[] Combine(string[] x, string[] y)
    {
        var formula = "";
        Polynomial result;
        var usedJ = new List<int>();
        var usedI = new List<int>();

        for (int j = 0; j < y.Length; ++j)
        {
            for (int i = 0; i < x.Length; ++i)
            {
                if (usedJ.Contains(j) || usedI.Contains(i))
                    continue;

                if (PowerOfElement(x[i]) == PowerOfElement(y[j]))
                {
                    var a = ModifierOfElement(x[i]) + ModifierOfElement(y[j]);
                    var n = PowerOfElement(x[i]);
                    formula += ParseElement(a, n) + "+";
                    usedJ.Add(j);
                    usedI.Add(i);
                }
                else
                {
                    formula += y[j] + "+" + x[i] + "+";
                    usedJ.Add(j);
                    usedI.Add(i);
                }
            }
        }

        formula = formula.Remove(formula.Length - 1);

        result = new(formula);

        return result.Elements;
    }

    public static string SumElements(string[] x, double power)
    {
        var a = new Complex(0, 0);
        var n = power;

        for (int i = 0; i < x.Length; ++i)
        {
            if (PowerOfElement(x[i]) == power)
                a += ModifierOfElement(x[i]);
        }

        return ParseElement(a, n);
    }

    public static string[] Simplify(string[] x, double maxPower)
    {
        var formula = "";
        Polynomial result;

        for (var i = maxPower; i >= 0; --i)
            formula += SumElements(x, i) + "+";

        formula = formula.Remove(formula.Length - 1);
        result = new(formula);

        return result.Elements;
    }
}

public class Polynomial
{
    public string Formula { get; private set; }
    public double MaxPower { get; private set; }
    public string[] Elements
    {
        get
        {
            var elements = Formula.Replace(" ", "").Split('+', StringSplitOptions.RemoveEmptyEntries);
            var result = new List<string>();

            for (int i = 0; i < elements.Length; ++i)
            {
                var element = "";

                if (elements[i].Last() == 'E' && i + 1 < elements.Length)
                    element = elements[i] + "+" + elements[i + 1];
                else
                    element = elements[i];

                if (element == "")
                    continue;

                if (PolynomialUtils.ModifierOfElement(element).Module == 0 && PolynomialUtils.PowerOfElement(element) != 0)
                    continue;

                result.Add(element);
            }

            return result.ToArray();
        }
    }

    public Complex[] Modifiers
    {
        get
        {
            var result = new Complex[Elements.Length];

            for (int i = 0; i < result.Length; ++i)
                result[i] = PolynomialUtils.ModifierOfElement(Elements[i]);

            return result;
        }
    }

    public double[] Powers
    {
        get
        {
            var result = new double[Elements.Length];

            for (int i = 0; i < result.Length; ++i)
                result[i] = PolynomialUtils.PowerOfElement(Elements[i]);

            return result;
        }
    }

    public Polynomial(string formula)
    {
        MaxPower = 0;

        formula = formula.Replace(" ", "");
        PolynomialUtils.AddPluses(ref formula);
        Formula = formula;

        for (int i = 0; i < Elements.Length; ++i)
        {
            if (PolynomialUtils.PowerOfElement(Elements[i]) > MaxPower)
                MaxPower = PolynomialUtils.PowerOfElement(Elements[i]);
        }

        var list = Elements.ToList();
        list.Sort((x, y) => PolynomialUtils.CompareElements(x, y));

        formula = "";

        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i] == "" || list[i] == " ")
                continue;

            if (PolynomialUtils.ModifierOfElement(list[i]).Module == 0 && Powers[i] != 0)
                continue;

            var a = PolynomialUtils.ModifierOfElement(list[i]);
            var n = PolynomialUtils.PowerOfElement(list[i]);
            formula += PolynomialUtils.ParseElement(a, n) + "+";
        }

        if (formula.Length != 0)
            formula = formula.Remove(formula.Length - 1);

        PolynomialUtils.AddSpaces(ref formula);
        Formula = formula;
    }

    public Complex Value(Complex x)
    {
        Complex r = new(0, 0);

        for (int i = 0; i < Modifiers.Length; ++i)
            r += Modifiers[i] * (x ^ Powers[i]);

        return r;
    }

    public string ExcludeFactor(int index, string a)
    {
        return PolynomialUtils.DivideElements(Elements[index], a);
    }

    public Complex Laguerre()
    {
        var n = new Complex(MaxPower, 0);
        var z = PolynomialUtils.ModifierOfElement(Elements[0]).Factors()[0];
        Complex a = new(0, 0);
        Complex x = new(z.Real, z.Imaginal);
        Complex y = new(0, 0);

        for (int i = 0; i < 1e12; ++i)
        {
            y = Value(x);

            if (Math.Round(y.Module) <= double.Epsilon)
                return x;

            var df = PolynomialUtils.Derivative(this);
            var G = df.Value(x) / Value(x);
            var H = (G ^ 2) - (PolynomialUtils.Derivative(df).Value(x) / Value(x));

            var d1 = G + (((H * n - (G^2)) * (n - 1))^0.5d);
            var d2 = G - (((H * n - (G^2)) * (n - 1))^0.5d);

            if (d1 > d2)
                a = n / d1;
            else
                a = n / d2;

            x = x - a;
            y = Value(x);

            if (Math.Round(y.Module) <= double.Epsilon)
                return x;
        }

        return x;
    }

    public Complex[] Roots()
    {
        Complex a, b, c;
        var elements = PolynomialUtils.PrepareForHorner(this);

        if (MaxPower == 1)
        {
            a = PolynomialUtils.ModifierOfElement(elements[0]);
            b = PolynomialUtils.ModifierOfElement(elements[1]);

            return new Complex[] { -b / a };
        }
        else if (MaxPower == 2)
        {
            a = PolynomialUtils.ModifierOfElement(elements[0]);
            b = PolynomialUtils.ModifierOfElement(elements[1]);
            c = PolynomialUtils.ModifierOfElement(elements[2]);

            var delta = (b ^ 2) - a * c * 4;

            if (Math.Round(delta.Module) == 0)
                return new Complex[] { -b / (a * 2) };

            var root = delta ^ 0.5d;

            return new Complex[] { (-b + root) / (a * 2), (-b - root) / (a * 2) };
        }

        var result = new List<Complex>();
        var x0 = new Complex(0, 0);
        var freeFactors = Modifiers[Modifiers.Length - 1].Factors();
        var maxPowerFactors = Modifiers[0].Factors();
        var w = new Polynomial("0");
        var roots = new Complex[0];

        for (int i = 0; i < freeFactors.Length; ++i)
        {
            for (int j = 0; j < maxPowerFactors.Length; ++j)
            {
                x0 = freeFactors[i] / maxPowerFactors[j];
                var y = Value(x0);

                if (y.Real <= double.Epsilon && y.Imaginal <= double.Epsilon)
                {
                    w = new Polynomial(string.Format("x + {0}", -x0));
                    roots = PolynomialUtils.Horner(this, w).Roots();

                    result.Add(x0);
                    result.AddRange(roots);

                    return result.ToArray();
                }
            }
        }

        x0 = -Laguerre();

        w = new Polynomial(string.Format("x + {0}", x0));
        roots = PolynomialUtils.Horner(this, w).Roots();

        result.Add(-x0);
        result.AddRange(roots);

        return result.ToArray();
    }

    public static Polynomial operator %(Polynomial a, Polynomial b)
    {
        if (a.MaxPower < b.MaxPower)
            return new Polynomial("" + double.MaxValue);

        return a - (a / b) * b;
    }

    public static Polynomial operator /(Polynomial a, Polynomial b)
    {
        if (a.MaxPower < b.MaxPower)
            throw new ArgumentException();

        var elements = a.Elements;
        var x = b.Elements[0];
        var result = "";

        while (elements.Length > 0)
        {
            if (PolynomialUtils.PowerOfElement(elements[0]) < PolynomialUtils.PowerOfElement(x))
                break;

            var y = PolynomialUtils.DivideElements(elements[0], x);
            result += y + "+";

            elements = PolynomialUtils.Combine(elements, PolynomialUtils.Multiply(y, b.Elements));
        }

        return new Polynomial(result);
    }

    public static Polynomial operator *(Polynomial a, Polynomial b)
    {
        var result = new List<string>();

        for (int j = 0; j < a.Elements.Length; ++j)
        {
            for (int i = 0; i < b.Elements.Length; ++i)
            {
                var a1 = a.Modifiers[j];
                var a2 = b.Modifiers[i];
                var n1 = a.Powers[j];
                var n2 = b.Powers[i];

                a1 *= a2;
                n1 += n2;

                result.Add(PolynomialUtils.ParseElement(a1, n1));
            }
        }
        var formula = "";

        result = PolynomialUtils.Simplify(result.ToArray(), a.MaxPower + b.MaxPower).ToList();

        for (int i = 0; i < result.Count; ++i)
            formula += result[i] + "+";

        formula = formula.Remove(formula.Length - 1);

        return new Polynomial(formula);
    }

    public static Polynomial operator +(Polynomial a, Polynomial b)
    {
        var result = new List<string>();

        result.AddRange(a.Elements);
        result.AddRange(b.Elements);

        var max = (a.MaxPower > b.MaxPower ? a.MaxPower : b.MaxPower);
        result = PolynomialUtils.Simplify(result.ToArray(), max).ToList();

        var formula = "";

        for (int i = 0; i < result.Count; ++i)
            formula += result[i] + "+";

        formula = formula.Remove(formula.Length - 1);

        return new Polynomial(formula);
    }

    public static Polynomial operator -(Polynomial a, Polynomial b)
    {
        var result = new List<string>();

        result.AddRange(a.Elements);

        for (int i = 0; i < b.Elements.Length; ++i)
            result.Add(PolynomialUtils.ParseElement(-b.Modifiers[i], b.Powers[i]));

        var max = (a.MaxPower > b.MaxPower ? a.MaxPower : b.MaxPower);
        result = PolynomialUtils.Simplify(result.ToArray(), max).ToList();

        var formula = "";

        for (int i = 0; i < result.Count; ++i)
            formula += result[i] + "+";

        formula = formula.Remove(formula.Length - 1);
        return new Polynomial(formula);
    }

    public string[] Factors()
    {
        var roots = Roots();
        var result = new string[roots.Length];

        for (int i = 0; i < roots.Length; ++i)
            result[i] = string.Format("x - {0}", roots[i]);

        return result;
    }

    public override string ToString()
    {
        var formula = "";

        for (int i = 0; i < Elements.Length; ++i)
        {
            var element = "";

            if (i - 1 >= 0 && Modifiers[i].Real < 0 && Modifiers[i].Imaginal == 0)
                element = PolynomialUtils.ParseElement(-Modifiers[i], Powers[i]);
            else
                element = Elements[i];

            if (i + 1 < Elements.Length && Modifiers[i + 1].Real < 0 && Modifiers[i + 1].Imaginal == 0)
                formula += element + " - ";
            else
                formula += element + " + ";
        }

        formula = formula.Remove(formula.Length - 3, 3);
        formula = formula.Replace(";", "; ");

        return formula;
    }
}