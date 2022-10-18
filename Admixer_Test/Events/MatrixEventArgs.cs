using System;

namespace Admixer_Test.Events
{
    public class MatrixEventArgs : EventArgs
    {
        public string Message { get; set; }
        public Matrix Matrix { get; set; }
    }
}