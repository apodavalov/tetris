using System;
using System.Drawing;

namespace Tetris.GameObjects
{
    class Game
    {
        public Game(int cupWidth, int cupHeight)
        {
            _Cup = new Cup(cupWidth, cupHeight);
        }

        public void New()
        {
            _Cup.Clear();
            _Level = 0;
            _Score = 0;
            _DroppedLinesCount = 0;

            NewShape();

            OnDraw();
        }


        public void NextTick()
        {
            Point newShapePoint = new Point(_ShapePoint.X, _ShapePoint.Y + 1);

            if (_Cup.CanShapeBePlaced(newShapePoint, _Shape))
            {
                _ShapePoint = newShapePoint;
                OnDraw();
            }
            else
            {
                _Score += 10;
                int droppedLinesCount = _Cup.MergeShape(_Shape,_ShapePoint);
                _Score += droppedLinesCount * 100;
                _DroppedLinesCount += droppedLinesCount;
                _Level = Math.Min(9, Math.Max(0, _DroppedLinesCount / 10));

                NewShape();

                if (!_Cup.CanShapeBePlaced(_ShapePoint, _Shape))
                {
                    OnGameOver();
                    New();
                }
                else
                {
                    OnDraw();
                }
            }
        }

        public void DropShape()
        {
            Point point = _ShapePoint;
            Point prevPoint;

            do
            {
                prevPoint = point;
                point = new Point(prevPoint.X, prevPoint.Y + 1);
            } while (_Cup.CanShapeBePlaced(point, _Shape));

            _ShapePoint = prevPoint;

            OnDraw();
        }

        public void MoveShapeLeft()
        {
            Point point = new Point(_ShapePoint.X - 1, _ShapePoint.Y);

            if (_Cup.CanShapeBePlaced(point, _Shape))
            {
                _ShapePoint = point;
                OnDraw();
            }
        }

        public void MoveShapeRight()
        {
            Point point = new Point(_ShapePoint.X + 1, _ShapePoint.Y);

            if (_Cup.CanShapeBePlaced(point, _Shape))
            {
                _ShapePoint = point;
                OnDraw();
            }
        }

        public void RotateShape()
        {
            Shape newShape = _Shape.Rotate();
            int x = _ShapePoint.X + _Shape.Width / 2 - newShape.Width / 2;

            if (x < 0)
            {
                x = 0;
            }
            else if (x + newShape.Width >= _Cup.Width)
            {
                x = _Cup.Width - newShape.Width;
            }

            Point point = new Point(x, _ShapePoint.Y);

            if (_Cup.CanShapeBePlaced(point, newShape))
            {
                _ShapePoint = point;
                _Shape = newShape;
                OnDraw();
            }
        }

        private void NewShape()
        {
            _Shape = RandomShape();
            _ShapePoint = new Point(_Cup.Width / 2 - _Shape.Width / 2, 0);
        }

        private Shape RandomShape()
        {
            return _Shapes[_Random.Next(_Shapes.Length)];
        }

        private event GameOverEventHandler _GameOver;
        public event GameOverEventHandler GameOver
        {
            add
            {
                _GameOver += value;
            }
            remove
            {
                _GameOver -= value;
            }
        }

        private void OnDraw()
        {
            if (_Draw != null)
            {
                _Draw(this,new DrawEventArgs(_Cup, _Shape, _ShapePoint, _Level, _Score));
            }
        }

        private void OnGameOver()
        {
            if (_GameOver != null)
            {
                _GameOver(this, new GameOverEventArgs(_Level, _Score));
            }
        }

        private event DrawEventHandler _Draw;
        public event DrawEventHandler Draw
        {
            add
            {
                _Draw += value;
            }
            remove
            {
                _Draw -= value;
            }
        }

        private Cup _Cup;
        private Shape _Shape;
        private Point _ShapePoint;
        private int _Level;
        private int _Score;
        private int _DroppedLinesCount;
        private Random _Random = new Random();

        private static readonly Shape[] _Shapes = new Shape[]
        {
            // ****
            new Shape(new Cell[,] { 
               { Cell.Occupied }, 
               { Cell.Occupied }, 
               { Cell.Occupied }, 
               { Cell.Occupied } 
            } ), 

            //  **
            // **
            new Shape(new Cell[,] { 
                { Cell.Occupied, Cell.Free }, 
                { Cell.Occupied, Cell.Occupied }, 
                { Cell.Free, Cell.Occupied} 
            } ), 

            // **
            //  **
            new Shape(new Cell[,] { 
                { Cell.Free, Cell.Occupied }, 
                { Cell.Occupied, Cell.Occupied }, 
                { Cell.Occupied, Cell.Free} 
            } ), 

            // **
            // **
            new Shape(new Cell[,] { 
                { Cell.Occupied, Cell.Occupied }, 
                { Cell.Occupied, Cell.Occupied } 
            } ),

            // ***
            // *
            new Shape(new Cell[,] { 
                { Cell.Occupied, Cell.Occupied },
                { Cell.Free, Cell.Occupied },
                { Cell.Free, Cell.Occupied }
            } ), 

            // ***
            //   *
            new Shape(new Cell[,] { 
                { Cell.Free, Cell.Occupied },
                { Cell.Free, Cell.Occupied },
                { Cell.Occupied, Cell.Occupied }
            } ), 

            // ***
            //  *
            new Shape(new Cell[,] { 
                { Cell.Free, Cell.Occupied },
                { Cell.Occupied, Cell.Occupied },
                { Cell.Free, Cell.Occupied }
            } )
        };
    }
}
