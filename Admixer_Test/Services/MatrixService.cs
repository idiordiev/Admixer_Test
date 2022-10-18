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
            for (var i = 0; i < matrix.Rows; i++)
            for (var j = 0; j < matrix.Columns; j++)
                matrix[i, j] = _randomService.GetRandomInt(0, 3);

            MatrixEvent?.Invoke(this, new MatrixEventArgs { Message = "The new matrix has been generated.", Matrix = matrix });
            return matrix;
        }

        public bool CheckForSequencesInColumns(Matrix matrix)
        {
            for (var column = 0; column < matrix.Columns; column++)
                if (IndexOfSequenceInColumn(matrix, column) >= 0)
                    return true;

            return false;
        }

        private int IndexOfSequenceInColumn(Matrix matrix, int columnIndex)
        {
            for (var rowIndex = 0; rowIndex < matrix.Rows; rowIndex++)
            {
                var sequenceLength = GetSequenceLengthInColumn(matrix, rowIndex, columnIndex);

                if (sequenceLength >= 3)
                    return rowIndex;
            }

            return -1;
        }

        private int GetSequenceLengthInColumn(Matrix matrix, int rowIndex, int columnIndex)
        {
            var sequenceLength = 1;
            for (var k = rowIndex + 1; k < matrix.Rows; k++)
            {
                if (matrix[k, columnIndex] >= 0 && matrix[k, columnIndex] == matrix[k - 1, columnIndex])
                    sequenceLength++;
                else
                    break;
            }

            return sequenceLength;
        }

        public bool CheckForSequencesInRows(Matrix matrix)
        {
            for (var row = 0; row < matrix.Rows; row++)
                if (IndexOfSequenceInRow(matrix, row) >= 0)
                    return true;

            return false;
        }

        private int IndexOfSequenceInRow(Matrix matrix, int rowIndex)
        {
            for (var columnIndex = 0; columnIndex < matrix.Columns; columnIndex++)
            {
                var sequenceLength = GetSequenceLengthInRow(matrix, rowIndex, columnIndex);

                if (sequenceLength >= 3)
                    return columnIndex;
            }

            return -1;
        }

        private static int GetSequenceLengthInRow(Matrix matrix, int rowIndex, int columnIndex)
        {
            var sequenceLength = 1;
            for (var k = columnIndex + 1; k < matrix.Columns; k++)
                if (matrix[rowIndex, k] >= 0 && matrix[rowIndex, k] == matrix[rowIndex, k - 1])
                    sequenceLength++;
                else
                    break;

            return sequenceLength;
        }

        public void RemoveSequencesInColumns(Matrix matrix)
        {
            for (var column = 0; column < matrix.Columns; column++)
            {
                var rowIndex = IndexOfSequenceInColumn(matrix, column);
                if (rowIndex < 0)
                    continue;

                var sequenceLength = GetSequenceLengthInColumn(matrix, rowIndex, column);
                if (sequenceLength < 3)
                    continue;

                for (int k = rowIndex; k < rowIndex + sequenceLength; k++)
                    matrix[k, column] = -1;
                
                MatrixEvent?.Invoke(this, new MatrixEventArgs { Message = $"Concurrences in column {column} has been removed. ", Matrix = matrix });
            }
        }

        public void RemoveSequencesInRows(Matrix matrix)
        {
            for (var row = 0; row < matrix.Rows; row++)
            {
                var columnIndex = IndexOfSequenceInRow(matrix, row);
                if (columnIndex < 0)
                    continue;

                var sequenceLength = GetSequenceLengthInRow(matrix, row, columnIndex);
                if (sequenceLength < 3)
                    continue;

                for (var k = columnIndex; k < columnIndex + sequenceLength; k++)
                    matrix[row, k] = -1;
                
                MatrixEvent?.Invoke(this, new MatrixEventArgs { Message = $"Concurrences in row {row} has been removed. ", Matrix = matrix });
            }
        }

        public void ShiftRows(Matrix matrix)
        {
            for (int iteration = 0; iteration < matrix.Rows; iteration++)
            {
                for (var row = 1; row < matrix.Rows; row++)
                {
                    for (var column = 0; column < matrix.Columns; column++)
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
            for (var i = 0; i < matrix.Rows; i++)
                for (var j = 0; j < matrix.Columns; j++)
                    if (matrix[i, j] < 0)
                        matrix[i, j] = _randomService.GetRandomInt(0, 3);

            MatrixEvent?.Invoke(this, new MatrixEventArgs { Message = "The matrix has been refilled.", Matrix = matrix });
        }
    }
}