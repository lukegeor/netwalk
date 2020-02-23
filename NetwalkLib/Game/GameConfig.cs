using System;

namespace NetwalkLib
{
    public class GameConfig
    {
        private int _width = 3;
        private int _height = 3;
        
        public int Width
        {
            get => _width;
            set
            {
                if (value % 2 == 0)
                {
                    throw new ArgumentException($"{nameof(Width)} must be odd.", nameof(Width));
                }

                _width = value;
            }
        }

        public int Height
        {
            get => _width;
            set
            {
                if (value % 2 == 0)
                {
                    throw new ArgumentException($"{nameof(Height)} must be odd.", nameof(Height));
                }

                _height = value;
            }
        }
    }
}