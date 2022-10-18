using System;
using Admixer_Test.Events;

namespace Admixer_Test.Interfaces
{
    public interface IMatrixService
    {
        event EventHandler<MatrixEventArgs> MatrixEvent;

        Matrix GenerateMatrix();
        bool CheckForSequencesInColumns(Matrix matrix);
        bool CheckForSequencesInRows(Matrix matrix);
        void RemoveSequencesInColumns(Matrix matrix);
        void RemoveSequencesInRows(Matrix matrix);
        void ShiftRows(Matrix matrix);
        void FillEmptySpaces(Matrix matrix);
    }
}