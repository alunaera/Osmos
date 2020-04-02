using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Osmos
{
    internal class Game
    {
        private static readonly Random Random = new Random();
        private readonly Font font = new Font("Arial", 15);

        private int gameFieldWidth;
        private int gameFieldHeight;
        private PlayerCircle PlayerCircle => (PlayerCircle) gameObjects[0];
        private List<GameObject> gameObjects;

        public event Action Defeat = delegate { };
        public event Action Victory = delegate { };

        public void StartGame(int gameFieldWidth, int gameFieldHeight)
        {
            this.gameFieldWidth = gameFieldWidth;
            this.gameFieldHeight = gameFieldHeight;

            gameObjects = new List<GameObject> {new PlayerCircle(gameFieldWidth / 2, gameFieldHeight / 2)};
            GenerateCircles();
        }

        private void GenerateCircles()
        {
            int circlesCount = Random.Next(50, 60);

            for (int i = 0; i < circlesCount; i++)
            {
                int positionX = Random.Next(0, gameFieldWidth);
                int positionY = Random.Next(0, gameFieldHeight);
                int radius = Random.Next(10, 30);

                gameObjects.Add(new EnemyCircle(positionX, positionY, radius));
            }
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
                switch (gameObject.ObjectType)
                {
                    case ObjectType.PlayerCircle:
                        PlayerCircle.Draw(graphics);
                        break;
                    case ObjectType.EnemyCircle:
                        gameObject.Draw(graphics, gameObject.Radius <= PlayerCircle.Radius ? Brushes.Blue : Brushes.Red);
                        break;
                }
            
            DrawInterface(graphics);
        }

        private void DrawInterface(Graphics graphics)
        {
            graphics.DrawString("Summary area: " + (int)gameObjects.Sum(gameObject => gameObject.Area), font, Brushes.Black, gameFieldWidth - 200, 10);
           // graphics.DrawString("Player's health: " + PlayerShip.Health, font, Brushes.Black, gameFieldWidth - 165, 30);
        }
    }
}
