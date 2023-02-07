ulong p = 4;
var y = Taylor.Sin(0, p);
var dy = PolynomialUtils.Derivative(y);
var idy = PolynomialUtils.Integral(y);

Console.WriteLine("f(x) = {0}", Taylor.FromTaylor(y, 0, p));
Console.WriteLine("f'(x) = {0}", Taylor.FromTaylor(dy, 0, p));
Console.WriteLine("|f'(x) dx = {0} + C", Taylor.FromTaylor(idy, 0, p));