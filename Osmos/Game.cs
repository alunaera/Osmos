using System;
using System.Drawing;

namespace Osmos
{
    internal class Game
    {
        private readonly Random random = new Random();

        private int gameFieldWidth;
        private int gameFieldHeight;

        public event Action Defeat = delegate { };
        public event Action Victory = delegate { };

        public void StartGame(int gameFieldWidth, int gameFieldHeight)
        {
            this.gameFieldWidth = gameFieldWidth;
            this.gameFieldHeight = gameFieldHeight;
        }

        public void Update()
        {

        }

        public void Draw(Graphics graphics)
        {

        }
    }
}
