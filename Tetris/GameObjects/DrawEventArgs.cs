using System;
using System.Drawing;

namespace Tetris.GameObjects
{
    class DrawEventArgs : GameOverEventArgs
    {
        public Cup Cup
        {
            get;
            private set;
        }

        public Shape Shape
        {
            get;
            private set;
        }

        public Point ShapePoint
        {
            get;
            private set;
        }

        public DrawEventArgs(Cup cup, Shape shape, Point shapePoint, int level, int score) 
            : base(level, score)
        {
            if (cup == null)
            {
                throw new ArgumentNullException("cup");
            }

            if (shape == null)
            {
                throw new ArgumentNullException("shape");
            }

            if (shapePoint == null)
            {
                throw new ArgumentNullException("shapePoint");
            }

            Cup = cup;
            Shape = shape;
            ShapePoint = shapePoint;
        }
    }
}
