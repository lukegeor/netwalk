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
    public class GameWonEventArgsTests
    {
        private readonly GameWonEventArgs _args;
        private readonly TimeSpan _timeSpan;
        
        public GameWonEventArgsTests()
        {
            _timeSpan = TimeSpan.FromSeconds(4);
            _args = new GameWonEventArgs(_timeSpan);
        }
        
        #region Constructor
        [Fact]
        public void GameWonEventArgs_Constructor_Nominal()
        {
            Assert.Equal(_timeSpan, _args.WinTime);
        }
        #endregion
    }
}
