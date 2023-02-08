Console.Write("Podaj wzór funkcji f(x) = ");
var formula = Console.ReadLine();

Console.Clear();
Console.WriteLine("f(x) = {0}", formula);
Console.WriteLine("f'(x) = {0}", Integrals.Derivative(formula));
Console.WriteLine("|f(x) dx = {0}", Integrals.Integral(formula));