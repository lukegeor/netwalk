using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NetwalkLib;
using Xunit;

namespace NetwalkLibTests.Utils
{
    public class SetTests
    {
        private readonly Set<int> _set;
        private readonly int _representative;

        public SetTests()
        {
            _representative = 1;
            _set = new Set<int>(_representative);
        }
        
        #region Constructor
        [Fact]
        public void Set_Constructor_Nominal()
        {
            Assert.Equal(_representative, _set.Representative);
        }
        #endregion
        
        #region Add
        [Fact]
        public void Set_Add_Nominal()
        {
            // Arrange
            var secondNum = 5;
            
            // Act
            _set.Add(secondNum);
            
            // Assert
            Assert.Equal(2, _set.Count());
            Assert.Equal(1, _set.Representative);
            Assert.Contains(1, _set);
            Assert.Contains(5, _set);
        }

        [Fact]
        public void Set_Add_DuplicateItem()
        {
            // Arrange
            var secondNum = 1;
            
            // Act
            Assert.Throws<InvalidOperationException>(() => _set.Add(secondNum));
        }
        #endregion

        #region Merge
        [Fact]
        public void Set_Merge_Nominal()
        {
            // Arrange
            var secondSet = new Set<int>(5);
            secondSet.Add(6);
            
            // Act
            _set.Merge(secondSet);
            
            // Assert
            Assert.Equal(3, _set.Count());
            Assert.Equal(1, _set.Representative);
            Assert.Contains(1, _set);
            Assert.Contains(5, _set);
            Assert.Contains(6, _set);
        }

        [Fact]
        public void Set_Merge_DuplicateItem()
        {
            // Arrange
            var secondSet = new Set<int>(5);
            secondSet.Add(1);
            
            // Act
            Assert.Throws<InvalidOperationException>(() => _set.Merge(secondSet));
        }
        #endregion
        
        #region Equals
        [Fact]
        public void Set_Equals_True()
        {
            // Act
            // ReSharper disable once EqualExpressionComparison
            var equal = _set.Equals(_set);
            
            // Assert
            Assert.True(equal);
        }

        [Fact]
        public void Set_Equals_DifferentSetSameValuesTrue()
        {
            // Arrange
            var otherSet = new Set<int>(1);

            // Act
            var equal = _set.Equals(otherSet);
                
            // Assert
            Assert.True(equal);
        }

        [Fact]
        public void Set_Equals_False()
        {
            // Arrange
            var otherSet = new Set<int>(2);

            // Act
            var equal = _set.Equals(otherSet);
            
            // Assert
            Assert.False(equal);
        }

        [Fact]
        public void Set_Equals_DifferentTypeFalse()
        {
            // Arrange
            var otherSet = new Set<double>(2.0);

            // Act
            // ReSharper disable once SuspiciousTypeConversion.Global
            var equal = _set.Equals(otherSet);

            // Assert
            Assert.False(equal);
        }
        #endregion

        #region GetHashCode
        [Fact]
        public void Set_GetHashCode_Nominal()
        {
            // Act
            _ = _set.GetHashCode();
        }
        #endregion

        #region ToString
        [Fact]
        public void Set_GetHashCode_ToString()
        {
            // Act
            var toString = _set.ToString();
            
            // Assert
            Assert.Equal(_set.Representative.ToString(), toString);
        }
        #endregion

        #region GetEnumerator
        [Fact]
        public void Set_GetEnumeratorT_Nominal()
        {
            // Arrange
            var newSet = new List<int>();
            
            // Act
            foreach (var val in _set)
            {
                newSet.Add(val);
            }
            
            // Assert
            Assert.Collection(newSet, i => Assert.Equal(1, i));
        }
        
        [Fact]
        public void Set_GetEnumerator_Nominal()
        {
            // Arrange
            var newSet = new List<int>();
            
            // Act
            foreach (var val in (IEnumerable)_set)
            {
                newSet.Add((int) val);
            }
            
            // Assert
            Assert.Collection(newSet, i => Assert.Equal(1, i));
        }
        #endregion
    }
}