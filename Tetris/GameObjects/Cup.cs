using System.Drawing;

namespace Tetris.GameObjects
{
    class Cup
    {
        public int Width
        {
            get
            {
                return _Cells.GetLength(0);
            }
        }

        public int Height
        {
            get
            {
                return _Cells.GetLength(1);
            }
        }

        public Cup(int width, int height)
        {
            _Cells = new Cell[width, height];
        }
        
        public Cell this[int row, int column]
        {
            get
            {
                return _Cells[row, column];
            }
        }

        public void Clear()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    _Cells[i, j] = Cell.Free;
                }
            }
        }

        public int MergeShape(Shape shape, Point shapePoint)
        {
            for (int i = 0; i < shape.Width; i++)
            {
                for (int j = 0; j < shape.Height; j++)
                {
                    if (shape.Cells[i, j] == Cell.Occupied && 
                        IsInternalPoint(shapePoint.X + i, shapePoint.Y + j))
                    {  
                        _Cells[shapePoint.X + i, shapePoint.Y + j] = Cell.Occupied;
                    }
                }
            }

            int droppedLinesCount = 0;

            for (int j = _Cells.GetLength(1) - 1; j >= 0; j--)
            {
                bool flag = true;

                for (int i = 0; i < _Cells.GetLength(0); i++)
                {
                    if (_Cells[i, j] == Cell.Free)
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    droppedLinesCount++;
                    DropLine(j++);
                }
            }

            return droppedLinesCount;
        }

        private void DropLine(int column)
        {
            for (int j = column; j > 0; j--)
            {
                for (int i = 0; i < Width; i++)
                {
                    _Cells[i, j] = _Cells[i, j - 1];
                }
            }
        }

        private bool IsInternalPoint(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        public bool CanShapeBePlaced(Point shapePoint, Shape shape)
        {
            if (shapePoint.X >= 0 && shapePoint.Y >= 0 &&
                shapePoint.X + shape.Width <= Width && 
                shapePoint.Y + shape.Height <= Height)
            {
                bool flag = true;

                for (int i = 0; i < shape.Width; i++)
                {
                    for (int j = 0; j < shape.Height; j++)
                    {
                        if (shape.Cells[i, j] == Cell.Occupied && 
                            _Cells[shapePoint.X + i, shapePoint.Y + j] == Cell.Occupied)
                        {
                            flag = false;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        break;
                    }
                }

                return flag;
            }
            else
            {
                return false;
            }
        }        

        private Cell[,] _Cells;
    }
}
