using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Tetris.Resources;
using Tetris.GameObjects;

namespace Tetris
{
    public partial class MainForm : Form
    {
        const int CupWidth = 10, CupHeight = 20, RectSize = 20, LittleRectSize = RectSize / 8;

        private Game game = new Game(CupWidth, CupHeight);
        
        private Bitmap bitmap = new Bitmap(CupWidth * RectSize, CupHeight * RectSize, PixelFormat.Format32bppArgb);
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {   
            e.Graphics.DrawImageUnscaled(bitmap, e.ClipRectangle);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {   
            int BorderWidth = (this.Width - this.ClientSize.Width) / 2;
            int TitlebarHeight = this.Height - this.ClientSize.Height - 2 * BorderWidth;

            this.Width = bitmap.Width + 2 * BorderWidth;
            this.Height = bitmap.Height + TitlebarHeight + 2 * BorderWidth + statusStrip.Height;

            game.GameOver += game_GameOver;
            game.Draw += game_Draw;

            game.New();
        }

        private void game_Draw(object sender, DrawEventArgs args)
        {
            DrawField(bitmap, args.Cup,args.Shape, args.ShapePoint);

            timer.Interval = 1000 - args.Level * 100;

            tssl_Level.Text = string.Format(Strings.Level, args.Level);
            tssl_Score.Text = string.Format(Strings.Score, args.Score);

            Invalidate();
        }

        private void game_GameOver(object sender, GameOverEventArgs args)
        {
            timer.Enabled = false;

            MessageBox.Show(string.Format(Strings.Score, args.Score), Strings.GameOver, 
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            timer.Enabled = true;     
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            game.NextTick();
        }

        private void DrawField(Image image, Cup cup, Shape shape, Point shapePoint)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                g.Clear(Color.Black);

                for (int i = 0; i < cup.Width; i++)
                {
                    for (int j = 0; j < cup.Height; j++)
                    {
                        if (cup[i, j] == Cell.Occupied)
                        {
                            DrawCell(g, i, j, cup[i, j], Color.Green);
                        }
                        else
                        {
                            DrawCell(g, i, j, cup[i, j], Color.Gray);
                        }
                    }
                }

                if (shape != null)
                {
                    for (int i = 0; i < shape.Width; i++)
                    {
                        for (int j = 0; j < shape.Height; j++)
                        {
                            if (shape.Cells[i, j] == Cell.Occupied)
                            {
                                Rectangle rect = new Rectangle((shapePoint.X + i) * RectSize, (shapePoint.Y + j)* RectSize, RectSize,RectSize);
                                g.FillRectangle(Brushes.Black,rect);
                                DrawCell(g, shapePoint.X + i, shapePoint.Y + j, Cell.Occupied, Color.Red);
                            }
                        }
                    }
                }
            }
        }

        private void DrawCell(Graphics g, int i, int j, Cell cell, Color color)
        {
            Rectangle rect;

            if (cell == Cell.Free)
            {
                rect = new Rectangle(i * RectSize + RectSize / 2 - LittleRectSize / 2, j * RectSize + RectSize / 2 - LittleRectSize / 2, LittleRectSize, LittleRectSize);
            }
            else
            {
                rect = new Rectangle(i * RectSize , j * RectSize, RectSize, RectSize);
            }

            using (Pen pen = new Pen(color, LittleRectSize))
            {
                g.DrawRectangle(pen, rect);
            }
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '5':
                    game.DropShape();
                    e.Handled = true;
                    break;
                case '7':
                    game.MoveShapeLeft();
                    e.Handled = true;
                    break;
                case '8':
                    game.RotateShape();
                    e.Handled = true;
                    break;
                case '9':
                    game.MoveShapeRight();
                    e.Handled = true;
                    break;
            }
        }
        
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    game.DropShape();
                    e.Handled = true;
                    break;
                case Keys.Left:
                    game.MoveShapeLeft();
                    e.Handled = true;
                    break;
                case Keys.Up:
                    game.RotateShape();
                    e.Handled = true;
                    break;
                case Keys.Right:
                    game.MoveShapeRight();
                    e.Handled = true;
                    break;
            }
        }
    }
}
