using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NetwalkLib;
using Xunit;
using Xunit.Abstractions;

namespace NetwalkLibTests
{
    public class BoardTests
    {
        private readonly Board _board;

        public BoardTests()
        {
            _board = new Board(5, 3, new[,] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}, {10, 11, 12}, {13, 14, 15}});
        }

        #region Constructor

        [Fact]
        public void Board_Constructor_Nominal()
        {
        }

        public static IEnumerable<object[]> BadHeightData => new List<object[]>
        {
            new object[] {4, 3, new int[,] {{3, 3, 3}, {3, 3, 3}, {3, 3, 3}, {3, 3, 3}}},
            new object[] {5, 3, new int[,] {{3, 3, 3}, {3, 3, 3}, {3, 3, 3}}},
            new object[] {-5, 3, new int[,] {{3, 3, 3}, {3, 3, 3}, {3, 3, 3}}},
        };
        
        [Theory]
        [MemberData(nameof(BadHeightData))]
        public void Board_Constructor_BadHeight(int height, int width, int[,] cells)
        {
            // Assert
            var ex = Assert.Throws<ArgumentException>(() => new Board(height, width, cells));
            Assert.Equal("height", ex.ParamName);
        }

        public static IEnumerable<object[]> BadWidthData => new List<object[]>
        {
            new object[] {3, 4, new int[,] {{3, 3, 3, 3}, {3, 3, 3, 3}, {3, 3, 3, 3}}},
            new object[] {3, 5, new int[,] {{3, 3, 3}, {3, 3, 3}, {3, 3, 3}}},
            new object[] {3, -5, new int[,] {{3, 3, 3}, {3, 3, 3}, {3, 3, 3}}},
        };
        
        [Theory]
        [MemberData(nameof(BadWidthData))]
        public void Board_Constructor_BadWidth(int height, int width, int[,] cells)
        {
            // Assert
            var ex = Assert.Throws<ArgumentException>(() => new Board(height, width, cells));
            Assert.Equal("width", ex.ParamName);
        }
        #endregion
        
        #region ToString
        [Fact]
        public void Board_ToString_Nominal()
        {
            // Act
            var toString = _board.ToString();

            // Assert
            var chars = toString.ToCharArray();
            for (var row = 0; row < _board.Height; row++)
            {
                for (var col = 0; col < _board.Width; col++)
                {
                    Assert.Equal(Board.Glyphs[row * _board.Width + col + 1], chars[row * (_board.Width + Environment.NewLine.Length) + col]);
                }
            }
        }
        #endregion
        
        #region Equals
        [Fact]
        public void Board_Equals_True()
        {
            // Assert
            // ReSharper disable once EqualExpressionComparison
            Assert.True(_board.Equals(_board));
        }

        [Fact]
        public void Board_Equals_OtherType()
        {
            // Assert
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.False(_board.Equals(5));
        }

        [Fact]
        public void Board_Equals_DifferentWidth()
        {
            // Arrange
            var other = new Board(5, 5, new[,] {{1, 2, 3, 0, 0}, {4, 5, 6, 0, 0}, {7, 8, 9, 0, 0}, {10, 11, 12, 0, 0}, {13, 14, 15, 0, 0}});
            
            // Assert
            Assert.NotEqual(_board, other);
        }

        [Fact]
        public void Board_Equals_DifferentHeight()
        {
            // Arrange
            var other = new Board(3, 3, new[,] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}});
            
            // Assert
            Assert.NotEqual(_board, other);
        }

        [Fact]
        public void Board_Equals_DifferentValues()
        {
            // Arrange
            var other = new Board(5, 3, new[,] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}, {10, 11, 12}, {13, 14, 14}});
            
            // Assert
            Assert.NotEqual(_board, other);
        }
        #endregion
        
        #region GetHashCode
        [Fact]
        public void Board_GetHashCode_NotEqual()
        {
            // Arrange
            var other = new Board(5, 3, new[,] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}, {10, 11, 12}, {13, 14, 16}});

            // Act
            var boardHash = _board.GetHashCode();
            var otherHash = other.GetHashCode();
            
            // Assert
            Assert.NotEqual(boardHash, otherHash);
        }
        #endregion
    }
}
