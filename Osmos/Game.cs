using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Osmos
{
    internal class Game
    {
        public static readonly Random Random = new Random();
        private readonly Font font = new Font("Arial", 15);

        private int gameFieldWidth;
        private int gameFieldHeight;
        private double fps;
        private int delayOfShot;
        private GameMode gameMode;
        private List<GameObject> gameObjects;
        private double SummaryArea => gameObjects.Sum(gameObject => gameObject.Area);
        private PlayerCircle PlayerCircle => (PlayerCircle) gameObjects[0];

        public event Action Defeat = delegate { };
        public event Action Victory = delegate { };

        public void StartGame(int gameFieldWidth, int gameFieldHeight)
        {
            this.gameFieldWidth = gameFieldWidth;
            this.gameFieldHeight = gameFieldHeight;
            delayOfShot = 0;

            gameObjects = new List<GameObject> ();
            GenerateCircles();
        }

        private void GenerateCircles()
        {
            int circlesCount;

            switch (gameMode)
            {
                case GameMode.Cycle:
                case GameMode.Repulsion:
                    gameObjects.Add(new PlayerCircle(gameFieldWidth, gameFieldHeight));

                    circlesCount = Random.Next(300, 400);
                    for (int i = 0; i < circlesCount; i++)
                        gameObjects.Add(new EnemyCircle(gameFieldWidth, gameFieldHeight));
                    break;
                case GameMode.ManyCircles:
                    gameObjects.Add(new PlayerCircle(gameFieldWidth, gameFieldHeight));
                    gameObjects[0].Area = 8 * Math.PI;

                    circlesCount = 10000;
                    for (int i = 0; i < circlesCount; i++)
                    {
                        gameObjects.Add(new EnemyCircle(gameFieldWidth, gameFieldHeight));
                        gameObjects[i + 1].Area = 4 * Math.PI;
                    }

                    break;
            }

        }

        public void Update(double fps)
        {
            this.fps = fps;

            foreach (GameObject gameObject in gameObjects)
                gameObject.Update(gameMode);

            foreach (GameObject gameObject in gameObjects)
            {
                foreach (GameObject nextGameObject in gameObjects)
                {
                    double valueOfIntersection = gameObject.Radius + nextGameObject.Radius -
                                                 gameObject.GetDistanceToObject(nextGameObject);

                    if (!(valueOfIntersection > 0))
                        continue;

                    if (gameObject.Radius > nextGameObject.Radius)
                        Absorbing(gameObject, nextGameObject);
                    else
                        Absorbing(nextGameObject, gameObject);
                }
            }

            gameObjects.RemoveAll(gameObject => gameObject.ObjectType != ObjectType.PlayerCircle && gameObject.Radius <= 0);

            if (delayOfShot > 0)
                delayOfShot--;

            if (PlayerCircle.Radius <= 0)
                Defeat();

            if (PlayerCircle.Area > SummaryArea / 2)
                Victory();
        }

        private static void Absorbing(GameObject largerCircle, GameObject smallerCircle)
        {
            double distance = largerCircle.GetDistanceToObject(smallerCircle);
            double previousLargerCircleArea = largerCircle.Area;
            double previousSmallerCircleArea = smallerCircle.Area;

            if (largerCircle.Radius > smallerCircle.Radius + distance)
            {
                largerCircle.Area += smallerCircle.Area;
                smallerCircle.Area = 0;
            }
            else
            {
                smallerCircle.Area = GetNewRadiusSmallerCircle(largerCircle.Radius,
                    smallerCircle.Radius, largerCircle.Radius + smallerCircle.Radius - distance);
                largerCircle.Area += previousSmallerCircleArea - smallerCircle.Area;
            }


            double gameObjectNewVectorX =
                (smallerCircle.VectorX * (previousSmallerCircleArea - smallerCircle.Area) +
                 largerCircle.VectorX * previousLargerCircleArea) / largerCircle.Area;

            double gameObjectNewVectorY =
                (smallerCircle.VectorY * (previousSmallerCircleArea - smallerCircle.Area) +
                 largerCircle.VectorY * previousLargerCircleArea) / largerCircle.Area;

            largerCircle.SetNewVector(gameObjectNewVectorX, gameObjectNewVectorY);
        }

        public void MakeShot(int cursorPositionX, int cursorPositionY)
        {
            if (delayOfShot <= 0)
            {
                gameObjects.Add(PlayerCircle.GetNewEnemyCircle(cursorPositionX, cursorPositionY));
                delayOfShot = 20;
            }
        }


        private static double GetNewRadiusSmallerCircle(double largerRadius, double smallerRadius, double valueOfIntersection)
        {
            double b = largerRadius - smallerRadius + valueOfIntersection;
            double c = valueOfIntersection * (valueOfIntersection - 2 * smallerRadius) / 2;
            double sqrtOfDiscriminant = Math.Sqrt(b * b - 4 * c);

            double newRadius = smallerRadius - valueOfIntersection - (sqrtOfDiscriminant - b) / 2;

            return newRadius * newRadius * Math.PI;
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
            graphics.DrawString("FPS: " + (int)fps, font, Brushes.Black, gameFieldWidth - 250, 50);
        }
    }
}
