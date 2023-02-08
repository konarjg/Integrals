Console.Write("Podaj wzór funkcji f(x) = ");
var formula = Console.ReadLine();

Console.Clear();
Console.Write("Podaj górne ograniczenie całki: b = ");
var b = Complex.Parse(Console.ReadLine());

Console.Clear();

Console.Write("Podaj górne ograniczenie całki: a = ");
var a = Complex.Parse(Console.ReadLine());

Console.Clear();

Console.WriteLine("{0}              {0}   ", b);
Console.WriteLine("|{0} dx = {1}| = {2}", formula, Integrals.Integral(formula, 9), Integrals.Integral(formula, a, b, 9));
Console.WriteLine("{0}              {0}   ", a);