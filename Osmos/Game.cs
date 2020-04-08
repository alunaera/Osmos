using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Osmos
{
    internal class Game
    {
        public static readonly Random Random = new Random();
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
            int circlesCount = Random.Next(90, 100);

            for (int i = 0; i < circlesCount; i++)
                gameObjects.Add(new EnemyCircle(gameFieldWidth, gameFieldHeight));
        }

        public void Update()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update();
                gameObject.ProcessingRepulsion(gameFieldWidth, gameFieldHeight);
            }

            foreach (GameObject gameObject in gameObjects)
            {
                foreach (GameObject nextGameObject in gameObjects)
                {
                    double valueOfIntersection = gameObject.Radius + nextGameObject.Radius - gameObject.GetDistanceToObject(nextGameObject);

                    if (!(valueOfIntersection >= 0) || gameObject.Radius == nextGameObject.Radius)
                        continue;

                    if (gameObject.Radius > nextGameObject.Radius)
                    {
                        double nextGameObjectPreviousArea = nextGameObject.Area;
                        nextGameObject.ChangeRadius(GetValueOfDecreaseSmallerCircle(gameObject.Radius, nextGameObject.Radius, valueOfIntersection));

                        gameObject.ChangeRadius(GetValueOfIncreaseLargerCircle(gameObject, nextGameObjectPreviousArea - nextGameObject.Area));
                    }
                    else
                    {
                        double gameObjectPreviousArea = gameObject.Area;
                        gameObject.ChangeRadius(GetValueOfDecreaseSmallerCircle(nextGameObject.Radius, gameObject.Radius, valueOfIntersection));

                        nextGameObject.ChangeRadius(GetValueOfIncreaseLargerCircle(nextGameObject, gameObjectPreviousArea - gameObject.Area));
                    }
                }
            }

            gameObjects.RemoveAll(gameObject =>
                gameObject.ObjectType != ObjectType.PlayerCircle && gameObject.Radius <= 0);

            if (PlayerCircle.Radius <= 0)
                Defeat();

            if (PlayerCircle.Area >= gameObjects.Sum(gameObject => gameObject.Area) / 2)
                Victory();
        }

        private static double GetValueOfIncreaseLargerCircle(GameObject gameObject, double absorbedArea)
        {
            return Math.Sqrt((gameObject.Area + absorbedArea) / Math.PI) - gameObject.Radius;
        }

        private static double GetValueOfDecreaseSmallerCircle(double largerRadius, double smallerRadius, double valueOfIntersection)
        {
            double b = largerRadius - smallerRadius + valueOfIntersection;
            double c = valueOfIntersection * (valueOfIntersection - 2 * smallerRadius) / 2;
            double sqrtOfDiscriminant = Math.Sqrt(b * b - 4 * c);

            return -valueOfIntersection - (sqrtOfDiscriminant - b) / 2;
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
        }
    }
}
