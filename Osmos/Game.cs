using System;
using System.Collections.Generic;
using System.Drawing;

namespace Osmos
{
    internal class Game
    {
        private readonly Random random = new Random();

        private int gameFieldWidth;
        private int gameFieldHeight;
        private List<GameObject> gameObjects;

        public event Action Defeat = delegate { };
        public event Action Victory = delegate { };

        public void StartGame(int gameFieldWidth, int gameFieldHeight)
        {
            this.gameFieldWidth = gameFieldWidth;
            this.gameFieldHeight = gameFieldHeight;

            gameObjects = new List<GameObject> {new PlayerCircle(150, 150)};

        }

        public void Update()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update();
                gameObject.ProcessingRepulsion(gameFieldWidth, gameFieldHeight);
            }
        }

        public void Draw(Graphics graphics)
        {
            foreach (GameObject gameObject in gameObjects)
                gameObject.Draw(graphics, Brushes.Black);
        }
    }
}
