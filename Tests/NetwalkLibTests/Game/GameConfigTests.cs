using System;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NetwalkLib;
using Xunit;
using Xunit.Abstractions;

namespace NetwalkLibTests
{
    public class GameConfigTests
    {
        private readonly GameConfig _gameConfig;
        
        public GameConfigTests()
        {
            _gameConfig = new GameConfig();
        }
        
        #region Constructor
        [Fact]
        public void GameConfig_Constructor_Nominal()
        {
        }
        #endregion
        
        #region Height
        [Fact]
        public void GameConfig_Height_Nominal()
        {
            // Act
            _gameConfig.Height = 3;
            
            // Assert
            Assert.Equal(3, _gameConfig.Height);
        }

        [Fact]
        public void GameConfig_Height_Even()
        {
            Assert.Throws<ArgumentException>(() => _gameConfig.Height = 2);
        }

        [Fact]
        public void GameConfig_Height_TooSmall()
        {
            Assert.Throws<ArgumentException>(() => _gameConfig.Height = 1);
        }
        #endregion
        
        #region Width
        [Fact]
        public void GameConfig_Width_Nominal()
        {
            // Act
            _gameConfig.Width = 3;
            
            // Assert
            Assert.Equal(3, _gameConfig.Width);
        }

        [Fact]
        public void GameConfig_Width_Even()
        {
            Assert.Throws<ArgumentException>(() => _gameConfig.Width = 2);
        }

        [Fact]
        public void GameConfig_Width_TooSmall()
        {
            Assert.Throws<ArgumentException>(() => _gameConfig.Width = 1);
        }
        #endregion

        #region RandomSeed
        [Fact]
        public void GameConfig_RandomSeed_Nominal()
        {
            // Act
            _gameConfig.RandomSeed = 3;
            
            // Assert
            Assert.Equal(3, _gameConfig.RandomSeed);
        }
        #endregion
    }
}
