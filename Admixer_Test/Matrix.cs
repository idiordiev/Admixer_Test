using System;
using System.Text;

namespace Admixer_Test
{
    public class Matrix
    {
        private int[,] _values;

        public Matrix(int rows, int columns)
        {
            _values = new int[rows, columns];
        }

        public Matrix(int[,] values)
        {
            _values = values;
        }

        public int Rows
        {
            get
            {
                return _values.GetLength(0);
            }
        }

        public int Columns
        {
            get
            {
                return _values.GetLength(1);
            }
        }

        public int this[int row, int column]
        {
            get
            {
                if (row < 0 || row > Rows - 1 || column < 0 || column > Columns - 1)
                {
                    throw new IndexOutOfRangeException("Invalid index.");
                }
                
                return _values[row, column];
            }
            set
            {
                if (row < 0 || row > Rows - 1 || column < 0 || column > Columns - 1)
                {
                    throw new IndexOutOfRangeException("Invalid index.");
                }
                
                _values[row, column] = value;
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                var rowStringBuilder = new StringBuilder();
                for (int j = 0; j < Columns; j++)
                {
                    rowStringBuilder.Append(_values[i, j]);
                    rowStringBuilder.Append(' ');
                }

                stringBuilder.AppendLine(rowStringBuilder.ToString());
            }

            return stringBuilder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix other)
            {
                if (other.Rows != Rows || other.Columns != Columns)
                {
                    return false;
                }

                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if (this[i, j] != other[i, j])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;

        }

        public override int GetHashCode()
        {
            return _values != null ? _values.GetHashCode() : 0;
        }
    }
}