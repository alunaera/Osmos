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

            gameObjects = new List<GameObject> {new PlayerCircle(gameFieldWidth, gameFieldHeight)};
           // GenerateCircles();
        }

        private void GenerateCircles()
        {
            int circlesCount = Random.Next(400, 1000);

            for (int i = 0; i < circlesCount; i++)
                gameObjects.Add(new EnemyCircle(gameFieldWidth, gameFieldHeight));
        }

        public void Update()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update();
                gameObject.ProcessingRepulsion();
            }

            foreach (GameObject gameObject in gameObjects)
            {
                foreach (GameObject nextGameObject in gameObjects)
                {
                    double valueOfIntersection = gameObject.Radius + nextGameObject.Radius - gameObject.GetDistanceToObject(nextGameObject);

                    if (!(valueOfIntersection > 0))
                        continue;

                    double gameObjectNewVectorX =
                        ((gameObject.Area - nextGameObject.Area) * gameObject.VectorX +
                         2 * nextGameObject.Area * nextGameObject.VectorX) / (gameObject.Area + nextGameObject.Area);

                    double gameObjectNewVectorY =
                        ((gameObject.Area - nextGameObject.Area) * gameObject.VectorY +
                         2 * nextGameObject.Area * nextGameObject.VectorY) / (gameObject.Area + nextGameObject.Area);

                    double nextGameObjectVectorX =
                        (2 * gameObject.Area * gameObject.VectorX +
                         nextGameObject.VectorX * (nextGameObject.Area - gameObject.Area)) /
                        (gameObject.Area + nextGameObject.Area);

                    double nextGameObjectVectorY =
                        (2 * gameObject.Area * gameObject.VectorY +
                         nextGameObject.VectorY * (nextGameObject.Area - gameObject.Area)) /
                        (gameObject.Area + nextGameObject.Area);

                    gameObject.SetNewVector(gameObjectNewVectorX, gameObjectNewVectorY);
                    nextGameObject.SetNewVector(nextGameObjectVectorX, nextGameObjectVectorY);

                    if (gameObject.Radius > nextGameObject.Radius)
                    {
                        double nextGameObjectPreviousArea = nextGameObject.Area;

                        nextGameObject.ChangeRadius(GetValueOfDecreaseSmallerCircle(gameObject.Radius, nextGameObject.Radius, valueOfIntersection));
                        gameObject.ChangeRadius(GetValueOfIncreaseLargerCircle(gameObject, nextGameObjectPreviousArea - nextGameObject.Area)); ;
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
                gameObject.ObjectType != ObjectType.PlayerCircle && gameObject.Radius < 1);

            //if (PlayerCircle.Radius <= 0)
            //    Defeat();

            //if (PlayerCircle.Area >= gameObjects.Sum(gameObject => gameObject.Area) / 2)
            //    Victory();

        }

        public void MakeShot(int cursorPositionX, int cursorPositionY)
        {
            double offsetAngle = PlayerCircle.PositionX - cursorPositionX >= 0
                ? Math.Atan((PlayerCircle.PositionY - cursorPositionY) / (PlayerCircle.PositionX - cursorPositionX)) - Math.PI
                : Math.Atan((PlayerCircle.PositionY - cursorPositionY) / (PlayerCircle.PositionX - cursorPositionX));

            double newCirclesRadius = PlayerCircle.Radius / Math.Sqrt(10);

            Point newCirclesPosition = new Point((int)((PlayerCircle.Radius + newCirclesRadius) * Math.Cos(offsetAngle) + PlayerCircle.PositionX),
                (int)((PlayerCircle.Radius + newCirclesRadius) * Math.Sin(offsetAngle) + PlayerCircle.PositionY));

            EnemyCircle newEnemyCircle = new EnemyCircle(gameFieldHeight, gameFieldWidth, newCirclesRadius);
            newEnemyCircle.SetNewPosition(newCirclesPosition.X, newCirclesPosition.Y);
            newEnemyCircle.SetNewVector(Math.Sign(cursorPositionX - PlayerCircle.PositionX) * 10,
                -Math.Sign(cursorPositionY - PlayerCircle.PositionY) * 10);

            PlayerCircle.SetNewRadius(Math.Sqrt(PlayerCircle.Radius * PlayerCircle.Radius - newCirclesRadius * newCirclesRadius));

            double playerCircleNewVectorX =
                ((PlayerCircle.Area - newEnemyCircle.Radius) * PlayerCircle.VectorX +
                 2 * newEnemyCircle.Area * newEnemyCircle.VectorX) / (PlayerCircle.Area + newEnemyCircle.Area);

            double playerCircleNewVectorY =
                ((PlayerCircle.Area - newEnemyCircle.Area) * PlayerCircle.VectorY +
                 2 * newEnemyCircle.Area * newEnemyCircle.VectorY) / (PlayerCircle.Area + newEnemyCircle.Area);

            PlayerCircle.SetNewVector(playerCircleNewVectorX, playerCircleNewVectorY);

            gameObjects.Add(newEnemyCircle);
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
            graphics.DrawString("Summary area: " + (int)gameObjects.Sum(gameObject => gameObject.Area), font, Brushes.Black, gameFieldWidth - 250, 10);
            graphics.DrawString("Summary impulse: " + (int)gameObjects.Sum(gameObject => gameObject.Impulse), font, Brushes.Black, gameFieldWidth - 250, 30);
        }
    }
}
