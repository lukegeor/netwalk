using System;

namespace NetwalkLib
{
    public class GameConfig
    {
        private int _width = 3;
        private int _height = 3;
        private int? _randomSeed;
        
        public int Width
        {
            get => _width;
            set
            {
                if (value % 2 == 0)
                {
                    throw new ArgumentException($"{nameof(Width)} must be odd.", nameof(Width));
                }
                else if (value < 3)
                {
                    throw new ArgumentException($"{nameof(Width)} must be >= 3.", nameof(Width));
                }

                _width = value;
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value % 2 == 0)
                {
                    throw new ArgumentException($"{nameof(Height)} must be odd.", nameof(Height));
                }
                else if (value < 3)
                {
                    throw new ArgumentException($"{nameof(Height)} must be >= 3.", nameof(Height));
                }

                _height = value;
            }
        }

        public int? RandomSeed
        {
            get => _randomSeed;
            set => _randomSeed = value;
        }
    }
}