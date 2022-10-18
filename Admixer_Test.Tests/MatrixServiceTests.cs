using Admixer_Test.Interfaces;
using Admixer_Test.Services;
using Moq;
using NUnit.Framework;

namespace Admixer_Test.Tests
{
    public class MatrixServiceTests
    {
        private Mock<IRandomService> _randomService;
        private IMatrixService _service;
        
        [SetUp]
        public void Setup()
        {
            _randomService = new Mock<IRandomService>();
        }

        [Test]
        public void GenerateMatrix_RandomizerAlwaysReturnsZero_ReturnsMatrix9x9WithZeros()
        {
            // Arrange
            _randomService.Setup(x => x.GetRandomInt(0, 3)).Returns(0);
            _service = new MatrixService(_randomService.Object);
            
            // Act
            var matrix = _service.GenerateMatrix();

            // Assert
            Assert.AreEqual(9, matrix.Rows, "The matrix rows count is not equal to 9.");
            Assert.AreEqual(9, matrix.Columns, "The matrix columns count is not equal to 9.");
        }

        [Test]
        public void CheckForSequencesInColumns_NoSequences_ReturnsFalse()
        {
            // Arrange
            var matrix = new Matrix(new[,] { { 0, 1, 2, 3 }, { 1, 2, 3, 0 }, { 2, 3, 0, 1 }, { 3, 0, 1, 2 } });
            _service = new MatrixService(_randomService.Object);
            
            // Act
            var result = _service.CheckForSequencesInColumns(matrix);

            // Assert
            Assert.IsFalse(result, "Method returned wrong result.");
        }

        [Test]
        public void CheckForSequencesInColumns_WithSequences_ReturnsTrue()
        {
            // Arrange
            var matrix = new Matrix(new[,] { { 0, 1, 2, 3 }, { 0, 2, 3, 0 }, { 0, 3, 0, 1 }, { 3, 0, 1, 2 } });
            _service = new MatrixService(_randomService.Object);
            
            // Act
            var result = _service.CheckForSequencesInColumns(matrix);

            // Assert
            Assert.IsTrue(result, "Method returned wrong result.");
        }

        [Test]
        public void CheckForSequencesInRows_NoSequences_ReturnsFalse()
        {
            // Arrange
            var matrix = new Matrix(new[,]
            {
                { 0, 1, 2, 3 }, 
                { 1, 2, 3, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            _service = new MatrixService(_randomService.Object);
            
            // Act
            var result = _service.CheckForSequencesInRows(matrix);

            // Assert
            Assert.IsFalse(result, "Method returned wrong result.");
        }

        [Test]
        public void CheckForSequencesInRows_WithSequences_ReturnsTrue()
        {
            // Arrange
            var matrix = new Matrix(new[,]
            {
                { 0, 1, 2, 3 }, 
                { 1, 1, 1, 0 },
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            _service = new MatrixService(_randomService.Object);
            
            // Act
            var result = _service.CheckForSequencesInRows(matrix);

            // Assert
            Assert.IsTrue(result, "Method returned wrong result.");
        }

        [Test]
        public void RemoveSequencesInColumns_NoSequences_NoChanges()
        {
            // Arrange
            var expected = new Matrix(new[,]
            {
                { 0, 1, 2, 3 }, 
                { 1, 2, 3, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            var actual = new Matrix(new[,]
            {
                { 0, 1, 2, 3 }, 
                { 1, 2, 3, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            _service = new MatrixService(_randomService.Object);
            
            // Act
            _service.RemoveSequencesInColumns(actual);

            // Assert
            Assert.AreEqual(expected, actual, "The actual matrix is not equal to expected.");
        }

        [Test]
        public void RemoveSequencesInColumns_WithSequences_ReplacesConcurrentNumbersWithNegative()
        {
            // Arrange
            var expected = new Matrix(new[,]
            {
                { -1, 1, 2, 3 }, 
                { -1, 2, 3, 0 }, 
                { -1, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            var actual = new Matrix(new[,]
            {
                { 0, 1, 2, 3 }, 
                { 0, 2, 3, 0 }, 
                { 0, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            _service = new MatrixService(_randomService.Object);
            
            // Act
            _service.RemoveSequencesInColumns(actual);

            // Assert
            Assert.AreEqual(expected, actual, "The actual matrix is not equal to expected.");
        }
        
        [Test]
        public void RemoveSequencesInRows_NoSequences_NoChanges()
        {
            // Arrange
            var expected = new Matrix(new[,]
            {
                { 0, 1, 2, 3 }, 
                { 1, 2, 3, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            var actual = new Matrix(new[,]
            {
                { 0, 1, 2, 3 }, 
                { 1, 2, 3, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            _service = new MatrixService(_randomService.Object);
            
            // Act
            _service.RemoveSequencesInRows(actual);

            // Assert
            Assert.AreEqual(expected, actual, "The actual matrix is not equal to expected.");
        }

        [Test]
        public void RemoveSequencesInRows_WithSequences_ReplacesConcurrentNumbersWithNegative()
        {
            // Arrange
            var expected = new Matrix(new[,]
            {
                { 0, 1, 2, 3 }, 
                { -1, -1, -1, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            var actual = new Matrix(new[,]
            {
                { 0, 1, 2, 3 }, 
                { 1, 1, 1, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            _service = new MatrixService(_randomService.Object);

            // Act
            _service.RemoveSequencesInRows(actual);

            // Assert
            Assert.AreEqual(expected, actual, "The actual matrix is not equal to expected.");
        }

        [Test]
        public void ShiftEmptyValues_NothingToShift_NoChanges()
        {
            // Arrange
            var actual = new Matrix(new[,] 
            { 
                { 0, 1, 2, 3 }, 
                { 1, 2, 3, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 } 
            });
            var expected = new Matrix(new[,]
            {
                { 0, 1, 2, 3 }, 
                { 1, 2, 3, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            _service = new MatrixService(_randomService.Object);
            
            // Act
            _service.ShiftEmptyValues(actual);

            // Assert
            Assert.AreEqual(expected, actual, "The actual matrix is not equal to expected.");
        }

        [Test]
        public void ShiftEmptyValues_OneRowNeedToBeShifted_ShiftsValuesToTheUpperRow()
        {
            // Arrange
            var actual = new Matrix(new[,] 
            { 
                { 0, 1, 2, 3 }, 
                { -1, -1, -1, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 } 
            });
            var expected = new Matrix(new[,]
            {
                { -1, -1, -1, 3 }, 
                { 0, 1, 2, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            _service = new MatrixService(_randomService.Object);
            
            // Act
            _service.ShiftEmptyValues(actual);

            // Assert
            Assert.AreEqual(expected, actual, "The actual matrix is not equal to expected.");
        }

        [Test]
        public void ShiftEmptyValues_MultipleRowsNeedToBeShifted_ShiftsValuesToTheUpperRows()
        {
            // Arrange
            var actual = new Matrix(new[,] 
            { 
                { 0, 1, 2, 3 }, 
                { -1, -1, 1, 0 }, 
                { -1, -1, -1, 1 }, 
                { -1, 0, 1, 2 } 
            });
            var expected = new Matrix(new[,]
            {
                { -1, -1, -1, 3 }, 
                { -1, -1, 2, 0 }, 
                { -1, 1, 1, 1 }, 
                { 0, 0, 1, 2 }
            });
            _service = new MatrixService(_randomService.Object);
            
            // Act
            _service.ShiftEmptyValues(actual);

            // Assert
            Assert.AreEqual(expected, actual, "The actual matrix is not equal to expected.");
        }

        [Test]
        public void FillEmptySpaces_NothingToRefill_NoChanges()
        {
            // Arrange
            _randomService.Setup(x => x.GetRandomInt(0, 3)).Returns(3);
            var actual = new Matrix(new[,]
            {
                { 0, 1, 2, 3 }, 
                { 1, 1, 1, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            var expected = new Matrix(new[,]
            {
                { 0, 1, 2, 3 }, 
                { 1, 1, 1, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            _service = new MatrixService(_randomService.Object);
            
            // Act
            _service.FillEmptySpaces(actual);

            // Assert
            Assert.AreEqual(expected, actual, "The actual matrix is not equal to expected.");
        }

        [Test]
        public void FillEmptySpaces_MultipleValuesToRefill_ReplacesByNewValues()
        {
            // Arrange
            _randomService.Setup(x => x.GetRandomInt(0, 3)).Returns(3);
            var actual = new Matrix(new[,]
            {
                { 0, 1, 2, 3 }, 
                { -1, -1, -1, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            var expected = new Matrix(new[,]
            {
                { 0, 1, 2, 3 }, 
                { 3, 3, 3, 0 }, 
                { 2, 3, 0, 1 }, 
                { 3, 0, 1, 2 }
            });
            _service = new MatrixService(_randomService.Object);
            
            // Act
            _service.FillEmptySpaces(actual);

            // Assert
            Assert.AreEqual(expected, actual, "The actual matrix is not equal to expected.");
        }
    }
}