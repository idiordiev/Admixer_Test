using System;
using Admixer_Test.Events;
using Admixer_Test.Interfaces;
using Admixer_Test.Services;

namespace Admixer_Test
{
    /*
    Завдання 1. Аналог три в ряд
    Матриця 9х9 випадково заповнюється числами від 0 до 3 включно.
    Якщо по горизонталі/вертикалі збігаються 3 або більше чисел, видаляємо їх, 
    зсунувши всі елементи матриці зверху донизу заповнивши порожній простір. 
    Порожні елементи, що залишилися, заповнюємо випадковими числами від 0 до 3 включно.
    Повторюємо процедуру доти не буде збігів. Результат виконання вивести/залогувати
    */
    
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                IRandomService randomService = new RandomService();
                IMatrixService service = new MatrixService(randomService);
            
                service.MatrixEvent += ShowMatrixEvent;

                Console.WriteLine("Game started.");
                var matrix = service.GenerateMatrix();
                while (true)
                {
                    var isSequencesInRows = service.CheckForSequencesInRows(matrix);
                    if (isSequencesInRows)
                        service.RemoveSequencesInRows(matrix);
                
                    var isSequencesInColumns = service.CheckForSequencesInColumns(matrix);
                    if (isSequencesInColumns)
                        service.RemoveSequencesInColumns(matrix);
                
                    if (!isSequencesInColumns && !isSequencesInRows)
                        break;
                
                    service.ShiftEmptyValues(matrix);
                    service.FillEmptySpaces(matrix);
                }

                Console.WriteLine("Game over.");
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }

        private static void ShowMatrixEvent(object sender, MatrixEventArgs eventArgs)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(eventArgs.Message);
            Console.WriteLine(eventArgs.Matrix);
            Console.ForegroundColor = currentColor;
        }

        private static void ShowException(Exception ex)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = currentColor;
        }
    }
}