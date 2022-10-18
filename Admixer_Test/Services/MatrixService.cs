using System;
using Admixer_Test.Events;
using Admixer_Test.Interfaces;

namespace Admixer_Test.Services
{
    public class MatrixService : IMatrixService
    {
        private readonly IRandomService _randomService;

        public event EventHandler<MatrixEventArgs> MatrixEvent;

        public MatrixService(IRandomService randomService)
        {
            _randomService = randomService;
        }

        public Matrix GenerateMatrix()
        {
            var matrix = new Matrix(9, 9);
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    matrix[i, j] = _randomService.GetRandomInt(0, 3);
                }
            }

            MatrixEvent?.Invoke(this, new MatrixEventArgs { Message = "The new matrix has been generated.", Matrix = matrix });
            return matrix;
        }

        public bool CheckForSequencesInColumns(Matrix matrix)
        {
            for (int column = 0; column < matrix.Columns; column++)
            {
                if (GetIndexOfSequenceInColumn(matrix, column) >= 0)
                    return true;
            }

            return false;
        }

        private int GetIndexOfSequenceInColumn(Matrix matrix, int column)
        {
            for (int row = 0; row < matrix.Rows; row++)
            {
                var sequenceLength = GetSequenceLengthInColumn(matrix, row, column);

                if (sequenceLength >= 3)
                    return row;
            }

            return -1;
        }

        private int GetSequenceLengthInColumn(Matrix matrix, int rowStart, int column)
        {
            var sequenceLength = 1;
            for (int row = rowStart + 1; row < matrix.Rows; row++)
            {
                if (matrix[row, column] >= 0 && matrix[row, column] == matrix[row - 1, column])
                    sequenceLength++;
                else
                    break;
            }

            return sequenceLength;
        }

        public bool CheckForSequencesInRows(Matrix matrix)
        {
            for (int row = 0; row < matrix.Rows; row++)
            {
                if (GetIndexOfSequenceInRow(matrix, row) >= 0)
                    return true;
            }

            return false;
        }

        private int GetIndexOfSequenceInRow(Matrix matrix, int rowIndex)
        {
            for (int columnIndex = 0; columnIndex < matrix.Columns; columnIndex++)
            {
                var sequenceLength = GetSequenceLengthInRow(matrix, rowIndex, columnIndex);

                if (sequenceLength >= 3)
                    return columnIndex;
            }

            return -1;
        }

        private int GetSequenceLengthInRow(Matrix matrix, int row, int columnStart)
        {
            var sequenceLength = 1;
            for (int column = columnStart + 1; column < matrix.Columns; column++)
            {
                if (matrix[row, column] >= 0 && matrix[row, column] == matrix[row, column - 1])
                    sequenceLength++;
                else
                    break;
            }

            return sequenceLength;
        }

        public void RemoveSequencesInColumns(Matrix matrix)
        {
            for (int column = 0; column < matrix.Columns; column++)
            {
                var rowStart = GetIndexOfSequenceInColumn(matrix, column);
                if (rowStart < 0)
                    continue;

                var sequenceLength = GetSequenceLengthInColumn(matrix, rowStart, column);
                if (sequenceLength < 3)
                    continue;

                RemoveSequenceFromColumn(matrix, column, rowStart, sequenceLength);
                
                MatrixEvent?.Invoke(this, new MatrixEventArgs { Message = $"Concurrences in column {column} has been removed. ", Matrix = matrix });
            }
        }

        private void RemoveSequenceFromColumn(Matrix matrix, int column, int rowStart, int sequenceLength)
        {
            for (int row = rowStart; row < rowStart + sequenceLength; row++)
                matrix[row, column] = -1;
        }

        public void RemoveSequencesInRows(Matrix matrix)
        {
            for (int row = 0; row < matrix.Rows; row++)
            {
                var columnStart = GetIndexOfSequenceInRow(matrix, row);
                if (columnStart < 0)
                    continue;

                var sequenceLength = GetSequenceLengthInRow(matrix, row, columnStart);
                if (sequenceLength < 3)
                    continue;

                RemoveSequenceFromRow(matrix, row, columnStart, sequenceLength);
                
                MatrixEvent?.Invoke(this, new MatrixEventArgs { Message = $"Concurrences in row {row} has been removed. ", Matrix = matrix });
            }
        }

        private void RemoveSequenceFromRow(Matrix matrix, int row, int columnStart, int sequenceLength)
        {
            for (int column = columnStart; column < columnStart + sequenceLength; column++)
                matrix[row, column] = -1;
        }

        public void ShiftEmptyValues(Matrix matrix)
        {
            for (int iteration = 0; iteration < matrix.Rows; iteration++)
            {
                for (int row = 1; row < matrix.Rows; row++)
                {
                    for (int column = 0; column < matrix.Columns; column++)
                    {
                        if (matrix[row, column] < 0)
                            (matrix[row, column], matrix[row - 1, column]) = (matrix[row - 1, column], matrix[row, column]);
                    }
                }
            }

            MatrixEvent?.Invoke(this, new MatrixEventArgs { Message = "Matrix rows has been shifted.", Matrix = matrix });
        }

        public void FillEmptySpaces(Matrix matrix)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    if (matrix[i, j] < 0)
                        matrix[i, j] = _randomService.GetRandomInt(0, 3);
                }
            }

            MatrixEvent?.Invoke(this, new MatrixEventArgs { Message = "The matrix has been refilled.", Matrix = matrix });
        }
    }
}