using System;
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
    }
}