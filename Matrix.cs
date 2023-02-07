using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Matrix
{
    public string Name { get; private set; }
    public int Rows { get; private set; }
    public int Columns { get; private set; }
    private Complex[,] Values;
    
    #region Class Declaration

    public Matrix(Complex[,] values)
    {
        Name = "A";
        Values = values;
        Rows = values.GetLength(0);
        Columns = values.GetLength(1);
    }

    public Matrix(int rows, int columns, Complex[,] values = null)
    {
        Name = "A";

        if (values != null)
        {
            if (values.GetLength(0) != rows || values.GetLength(1) != columns)
                throw new ArgumentException();

            Rows = rows;
            Columns = columns;
            Values = values;
        }
        else
        {
            Rows = rows;
            Columns = columns;
            Values = new Complex[rows, columns];
        }
    }

    public Matrix(string name, int rows, int columns, Complex[,] values = null)
    {
        Name = name;

        if (values != null)
        {
            if (values.GetLength(0) != rows || values.GetLength(1) != columns)
                throw new ArgumentException();

            Rows = rows;
            Columns = columns;
            Values = values;
        }
        else
        {
            Rows = rows;
            Columns = columns;
            Values = new Complex[rows, columns];
        }
    }

    public string PrintRow(int row)
    {
        var output = "";

        for (int y = 0; y < Columns; ++y)
            output += Values[row, y] + " ";

        return output.Remove(output.Length - 1);
    }

    public override string ToString()
    {
        var output = "";

        for (int x = 0; x < Columns / 2; ++x)
            output += "  ";

        output += Name;

        for (int x = 0; x < Columns / 2; ++x)
            output += "  ";

        output += "\n";

        for (int x = 0; x < Rows; ++x)
        {
            for (int y = 0; y < Columns; ++y)
            {
                if (y == 0)
                    output += "|";

                var a = Values[x, y].Round(1).ToString();

                if (a == "-0")
                    a = "0";

                output += a + " ";

                if (y == Columns - 1)
                {
                    output = output.Remove(output.Length - 1);
                    output += "|";
                }
            }

            output += "\n";
        }

        return output;
    }
    public Complex this[int row, int column]
    {
        get => Values[row, column];
        set => Values[row, column] = value;
    }

    #endregion

    #region Advanced Operations
    public Matrix Minor(int x, int y)
    {
        if (Rows != Columns)
            throw new Exception("Non-Quadratic Matrix Exception");

        if (Rows == 1)
            return new Matrix(1, 1, new Complex[1, 1] { { new Complex(1, 0) } });

        Matrix B = RemoveRow(x).RemoveColumn(y);

        return B;
    }

    public Matrix Transpose()
    {
        Matrix B = new(Columns, Rows);
        int x0 = 0;
        int y0 = 0;

        for (int x = 0; x < Rows; ++x)
        {
            for (int y = 0; y < Columns; ++y)
            {
                B[x0, y0] = Values[x, y];
                ++x0;
            }

            x0 = 0;
            ++y0;
        }

        return B;
    }

    #endregion

    #region Row/Column Operations

    public Matrix RemoveRow(int row)
    {
        var x0 = 0;

        Matrix B = new(Rows - 1, Columns);

        for (int x = 0; x < Rows; ++x)
        {
            if (x == row)
                continue;

            for (int y = 0; y < Columns; ++y)
                B[x0, y] = Values[x, y];

            ++x0;
        }

        return B;
    }

    public Matrix RemoveColumn(int column)
    {
        var y0 = 0;

        Matrix B = new(Rows, Columns - 1);

        for (int y = 0; y < Columns; ++y)
        {
            if (y == column)
                continue;

            for (int x = 0; x < Rows; ++x)
                B[x, y0] = Values[x, y];

            ++y0;
        }

        return B;
    }

    public Complex[] GetRow(int row)
    {
        var output = new Complex[Columns];
        int i = 0;

        for (int y = 0; y < Columns; ++y)
        {
            output[i] = Values[row, y];
            ++i;
        }

        return output;
    }

    public Complex[] GetColumn(int column)
    {
        var output = new Complex[Rows];
        int i = 0;

        for (int x = 0; x < Rows; ++x)
        {
            output[i] = Values[x, column];
            ++i;
        }

        return output;
    }

    public void SetRow(int row, params Complex[] values)
    {
        if (values.Length != Columns)
            throw new ArgumentException();

        for (int y = 0; y < Columns; ++y)
            Values[row, y] = values[y];
    }

    public void SetColumn(int column, params Complex[] values)
    {
        if (values.Length != Rows)
            throw new ArgumentException();

        for (int x = 0; x < Rows; ++x)
            Values[x, column] = values[x];
    }

    #endregion;

    #region Elementary Operations

    public static Matrix UnitMatrix(int n)
    {
        Matrix A = new("I", n, n);
        int oneLocation = 0;

        for (int x = 0; x < n; ++x)
        {
            for (int y = 0; y < n; ++y)
            {
                if (y == oneLocation)
                    A[x, y] = new Complex(1, 0);
                else
                    A[x, y] = new Complex(0, 0);
            }

            ++oneLocation;
        }

        return A;
    }

    public void ToUnitMatrix()
    {
        var oneLocation = 0;
        Name = "I";

        for (int x = 0; x < Rows; ++x)
        {
            for (int x1 = (x - oneLocation >= 0 ? x - oneLocation : x + 1); x1 < Rows; ++x1)
            {
                if (GetRow(x)[oneLocation].Module == 0)
                    AddMultiplied(x, x1, new Complex(1, 0));
                else if (GetRow(x)[oneLocation] != new Complex(1, 0))
                    DivideRow(x, GetRow(x)[oneLocation]);

                if (x1 == x)
                    continue;

                if (GetRow(x1)[oneLocation].Module != 0)
                    AddMultiplied(x1, x, -GetRow(x1)[oneLocation]);
            }

            ++oneLocation;
        }
    }

    public void ToUnitMatrix(out List<string> operations)
    {
        var oneLocation = 0;
        operations = new List<string>();

        for (int x = 0; x < Rows; ++x)
        {
            if (GetRow(x)[oneLocation] != new Complex(1, 0))
            {
                operations.Add(string.Format("{0} /= {1}", x, GetRow(x)[oneLocation]));
                DivideRow(x, GetRow(x)[oneLocation]);
            }

            for (int x1 = (x - oneLocation >= 0 ? x - oneLocation : x + 1); x1 < Rows; ++x1)
            {
                if (x1 == x)
                    continue;

                if (GetRow(x1)[oneLocation].Module != 0)
                {
                    operations.Add(string.Format("{0} += {1} * {2}", x, x1, -GetRow(x1)[oneLocation]));
                    AddMultiplied(x1, x, -GetRow(x1)[oneLocation]);
                }
            }

            ++oneLocation;
        }
    }

    private void ExecuteOperation(string operation)
    {
        var tokens = operation.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var x = 0;
        var x1 = 0;

        if (tokens[1] == "/=")
        {
            x = int.Parse(tokens[0]);

            if (tokens[2].Contains(';'))
                DivideRow(x, Complex.Parse(tokens[2] + tokens[3]));
            else
                DivideRow(x, Complex.Parse(tokens[2]));
        }
        else if (tokens[1] == "+=")
        {
            x = int.Parse(tokens[0]);
            x1 = int.Parse(tokens[2]);

            if (tokens[4].Contains(';'))
                AddMultiplied(x, x1, Complex.Parse(tokens[4] + tokens[5]));
            else
                AddMultiplied(x, x1, Complex.Parse(tokens[4]));
        }
    }

    public static Matrix operator !(Matrix A)
    {
        var start = new Complex[A.Rows, A.Columns];

        Matrix B = UnitMatrix(A.Rows);
        B.Name = A.Name + "^-1";
        List<string> operations = new();

        A.ToUnitMatrix(out operations);

        for (int i = 0; i < operations.Count; ++i)
            B.ExecuteOperation(operations[i]);

        return B;
    }

    public void ToTriagonalMatrix()
    {
        var location = 0;

        for (int x = 0; x < Rows; ++x)
        {
            for (int x1 = location + 1; x1 < Rows; ++x1)
            {
                if (Values[x, location].Module == 0)
                    AddMultiplied(x, x1, new Complex(1, 0));

                if (Values[x1, location].Module != 0)
                    AddMultiplied(x1, x, -Values[x1, location] / Values[x, location]);
            }

            ++location;
        }
    }

    private static void CopyTo<T>(ref T[,] A, ref T[,] array)
    {
        for (int y = 0; y < A.GetLength(1); ++y)
        {
            for (int x = 0; x < A.GetLength(1); ++x)
                array[x, y] = A[x, y];
        }
    }

    public static Complex operator ~(Matrix A)
    {
        var location = 0;
        var determinant = new Complex(1, 0);

        var start = new Complex[A.Rows, A.Columns];

        CopyTo(ref A.Values, ref start);
        A.ToTriagonalMatrix();

        for (int x = 0; x < A.Rows; ++x)
        {
            determinant *= A[x, location];
            ++location;
        }

        if (determinant.Real == double.NaN)
            return new Complex(0, 0);

        CopyTo(ref start, ref A.Values);
        return determinant;
    }

    public void MultiplyRow(int row, double a)
    {
        var A = GetRow(row);
        var result = new Complex[A.Length];

        for (int i = 0; i < A.Length; ++i)
            result[i] = A[i] * a;

        SetRow(row, result);
    }

    public Complex[] Multiply(int row, Complex a)
    {
        var A = GetRow(row);
        var result = new Complex[A.Length];

        for (int i = 0; i < A.Length; ++i)
            result[i] = A[i] * a;

        return result;
    }

    public void DivideRow(int row, Complex a)
    {
        var A = GetRow(row);
        var result = new Complex[A.Length];

        for (int i = 0; i < A.Length; ++i)
            result[i] = A[i] / a;

        SetRow(row, result);
    }

    public Complex[] Divide(int row, Complex a)
    {
        var A = GetRow(row);
        var result = new Complex[A.Length];

        for (int i = 0; i < A.Length; ++i)
            result[i] = A[i] / a;

        return result;
    }

    public void AddMultiplied(int row, int row1, Complex a)
    {
        var A = GetRow(row);
        var B = Multiply(row1, a);

        if (A.Length != B.Length)
            throw new ArgumentException();

        for (int i = 0; i < A.Length; ++i)
            A[i] += B[i];

        SetRow(row, A);
    }

    private bool AllZeros(Complex[] A)
    {
        for (int i = 0; i < A.Length; ++i)
        {
            if (A[i].Module != 0f)
                return false;
        }

        return true;
    }

    public void RemoveAllZeros()
    {
        Matrix B = new(Rows, Columns, Values);
        var rowsToRemove = new List<int>();

        if (Rows != 1)
        {
            for (int x = 0; x < Rows; ++x)
            {
                if (AllZeros(GetRow(x)))
                    rowsToRemove.Add(x);
            }
        }

        for (int i = rowsToRemove.Count - 1; i >= 0; --i)
            B = B.RemoveRow(rowsToRemove[i]);

        Values = B.Values;
        Rows = B.Rows;
    }

    private bool Identical(Complex[] A, Complex[] B)
    {
        if (A.Length != B.Length)
            throw new ArgumentException();

        for (int i = 0; i < A.Length; ++i)
        {
            if (A[i] != B[i])
                return false;
        }

        return true;
    }

    public void RemoveIdentical()
    {
        Matrix B = new(Rows, Columns, Values);
        var rowsToRemove = new List<int>();
        var rowsRemoved = new List<Complex[]>();

        if (Rows != 1)
        {
            for (int x = 0; x < Rows; ++x)
            {
                for (int x1 = 1; x1 < Rows; ++x1)
                {
                    if (Identical(GetRow(x), GetRow(x1)) && rowsRemoved.Find(row => Identical(row, GetRow(x1))) == null)
                    {
                        rowsToRemove.Add(x);
                        rowsRemoved.Add(GetRow(x));
                    }
                }
            }
        }

        for (int i = rowsToRemove.Count - 1; i >= 0; --i)
            B = B.RemoveRow(rowsToRemove[i]);

        Values = B.Values;
        Rows = B.Rows;
    }

    private static Complex Multiply(Complex[] A, Complex[] B)
    {
        if (A.Length != B.Length)
            throw new ArgumentException();

        var result = new Complex(0, 0);

        for (int i = 0; i < A.Length; ++i)
            result += A[i] * B[i];

        return result;
    }

    #endregion

    #region Mathematical Operations

    public static Matrix operator +(Matrix A, Matrix B)
    {
        if (A.Rows != B.Rows || A.Columns != B.Columns)
            throw new Exception("Non-Identical Matrix Size Exception");

        Matrix C = new(A.Rows, A.Columns);

        for (int y = 0; y < A.Columns; ++y)
        {
            for (int x = 0; x < A.Rows; ++x)
                C[x, y] = A[x, y] + B[x, y];
        }

        return C;
    }

    public static Matrix operator -(Matrix A, Matrix B)
    {
        if (A.Rows != B.Rows || A.Columns != B.Columns)
            throw new Exception("Non-Identical Matrix Size Exception");

        Matrix C = new(A.Rows, A.Columns);

        for (int y = 0; y < A.Columns; ++y)
        {
            for (int x = 0; x < A.Rows; ++x)
                C[x, y] = A[x, y] - B[x, y];
        }

        return C;
    }

    public static Matrix operator *(Matrix A, Matrix B)
    {
        if (A.Columns != B.Rows)
            throw new Exception("Incorrect Matrix Size Exception");

        Matrix C = new(A.Rows, B.Columns);

        for (int y = 0; y < B.Columns; ++y)
        {
            for (int x = 0; x < A.Rows; ++x)
                C[x, y] = Multiply(A.GetRow(x), B.GetColumn(y));
        }

        return C;
    }

    public static Matrix operator /(Matrix A, Matrix B)
    {
        return A * !B;
    }

    public static Matrix operator *(Matrix A, double a)
    {
        Matrix C = new(A.Rows, A.Columns);

        for (int y = 0; y < A.Columns; ++y)
        {
            for (int x = 0; x < A.Rows; ++x)
                C[x, y] = A[x, y] * a;
        }

        return C;
    }

    public static Matrix operator /(Matrix A, double a)
    {
        Matrix C = new(A.Rows, A.Columns);

        for (int y = 0; y < A.Columns; ++y)
        {
            for (int x = 0; x < A.Rows; ++x)
                C[x, y] = A[x, y] / a;
        }

        return C;
    }

    #endregion
}