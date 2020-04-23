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
        private GameMode gameMode = GameMode.Repulsion;
        private List<GameObject> gameObjects;
        private double SummaryArea => gameObjects.Sum(gameObject => gameObject.Area);
        private PlayerCircle PlayerCircle => (PlayerCircle) gameObjects[0];

        public event Action Defeat = delegate { };
        public event Action Victory = delegate { };

        public void StartGame(int gameFieldWidth, int gameFieldHeight)
        {
            this.gameFieldWidth = gameFieldWidth;
            this.gameFieldHeight = gameFieldHeight;

            gameObjects = new List<GameObject> {new PlayerCircle(gameFieldWidth, gameFieldHeight)};
            GenerateCircles();
        }

        private void GenerateCircles()
        {
            int circlesCount = Random.Next(300, 400);

            for (int i = 0; i < circlesCount; i++)
                gameObjects.Add(new EnemyCircle(gameFieldWidth, gameFieldHeight));
        }

        public void Update()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameMode);
            }

            foreach (GameObject gameObject in gameObjects)
            {
                foreach (GameObject nextGameObject in gameObjects)
                {
                    double valueOfIntersection = gameObject.Radius + nextGameObject.Radius -
                                                 gameObject.GetDistanceToObject(nextGameObject);

                    if (!(valueOfIntersection > 0))
                        continue;

                    double previousArea = SummaryArea;

                    if (gameObject.Radius > nextGameObject.Radius)
                    {
                        double gameObjectNewVectorX =
                            ((gameObject.Area - nextGameObject.Area) * gameObject.VectorX +
                             2 * nextGameObject.Area * nextGameObject.VectorX) /
                            (gameObject.Area + nextGameObject.Area);

                        double gameObjectNewVectorY =
                            ((gameObject.Area - nextGameObject.Area) * gameObject.VectorY +
                             2 * nextGameObject.Area * nextGameObject.VectorY) /
                            (gameObject.Area + nextGameObject.Area);

                        gameObject.SetNewVector(gameObjectNewVectorX, gameObjectNewVectorY);

                        nextGameObject.SetNewRadius(GetNewRadiusSmallerCircle(gameObject.Radius,
                            nextGameObject.Radius, valueOfIntersection == 0 ? 1 : valueOfIntersection));
                        gameObject.SetNewRadius(GetNewRadiusLargerCircle(gameObject,
                            previousArea - SummaryArea));
                    }
                    else
                    {
                        double nextGameObjectVectorX =
                            (2 * gameObject.Area * gameObject.VectorX +
                             nextGameObject.VectorX * (nextGameObject.Area - gameObject.Area))
                            / (gameObject.Area + nextGameObject.Area);

                        double nextGameObjectVectorY =
                            (2 * gameObject.Area * gameObject.VectorY +
                             nextGameObject.VectorY * (nextGameObject.Area - gameObject.Area))
                            / (gameObject.Area + nextGameObject.Area);

                        nextGameObject.SetNewVector(nextGameObjectVectorX, nextGameObjectVectorY);

                        gameObject.SetNewRadius(GetNewRadiusSmallerCircle(nextGameObject.Radius,
                            gameObject.Radius, valueOfIntersection == 0 ? 1 : valueOfIntersection));
                        nextGameObject.SetNewRadius(GetNewRadiusLargerCircle(nextGameObject,
                            previousArea - SummaryArea));
                    }
                }
            }

            gameObjects.RemoveAll(gameObject => gameObject.ObjectType != ObjectType.PlayerCircle && gameObject.Radius < 1);

            if (PlayerCircle.Radius <= 0)
                Defeat();

            if (PlayerCircle.Area > SummaryArea / 2)
                Victory();

        }

        public void MakeShot(int cursorPositionX, int cursorPositionY)
        {
            gameObjects.Add(PlayerCircle.GetNewEnemyCircle(cursorPositionX, cursorPositionY));
        }

        private static double GetNewRadiusLargerCircle(GameObject gameObject, double absorbedArea)
        {
            return Math.Sqrt((gameObject.Area + absorbedArea) / Math.PI);
        }

        private static double GetNewRadiusSmallerCircle(double largerRadius, double smallerRadius, double valueOfIntersection)
        {
            double b = largerRadius - smallerRadius + valueOfIntersection;
            double c = valueOfIntersection * (valueOfIntersection - 2 * smallerRadius) / 2;
            double sqrtOfDiscriminant = Math.Sqrt(b * b - 4 * c);

            return smallerRadius - valueOfIntersection - (sqrtOfDiscriminant - b) / 2;
        }

        public void ChangeGameMode(GameMode gameMode)
        {
            this.gameMode = gameMode;
            StartGame(gameFieldWidth, gameFieldHeight);
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
            graphics.DrawString("Summary area: " + (int)SummaryArea, font, Brushes.Black, gameFieldWidth - 250, 10);
            graphics.DrawString("Summary impulse: " + (int)gameObjects.Sum(gameObject => gameObject.Impulse), font, Brushes.Black, gameFieldWidth - 250, 30);
        }
    }
}
