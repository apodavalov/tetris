using System;

namespace Tetris.GameObjects
{
    class Shape
    {
        public Cell[,] Cells
        {
            get;
            private set;
        }

        public int Width
        {
            get
            {
                return Cells.GetLength(0);
            }
        }

        public int Height
        {
            get
            {
                return Cells.GetLength(1);
            }
        }

        public Shape(Cell[,] cells)
        {
            if (cells == null)
            {
                throw new ArgumentNullException("cells");
            }

            Cells = new Cell[cells.GetLength(0),cells.GetLength(1)];

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    Cells[i, j] = cells[i, j];
                }
            }
        }

        private Shape()
        {

        }

        public Shape Rotate()
        {
            Cell[,] cells = new Cell[Cells.GetLength(1), Cells.GetLength(0)];

            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    cells[j, i] = Cells[i, Cells.GetLength(1) - 1 - j];
                }
            }

            Shape shape = new Shape();
            shape.Cells = cells;

            return shape;
        }
    }
}
