using System;
namespace Tetris.GameObjects
{
    class GameOverEventArgs : EventArgs
    {
        public int Level
        {
            get;
            private set;
        }

        public int Score
        {
            get;
            private set;
        }

        public GameOverEventArgs(int level, int score)
            : base()
        {
            Level = level;
            Score = score;
        }
    }
}
