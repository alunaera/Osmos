using System;
using System.Drawing;

namespace Osmos
{
    internal class PlayerCircle : GameObject
    {
        public PlayerCircle(int gameFieldWidth, int gameFieldHeight)
        {
            GameFieldWidth = gameFieldWidth;
            GameFieldHeight = gameFieldHeight;
            //PositionX = Game.Random.Next(0, gameFieldWidth);
            //PositionY = Game.Random.Next(0, gameFieldHeight);
            PositionX = gameFieldWidth / 2;
            PositionY = gameFieldHeight / 2;
            ObjectType = ObjectType.PlayerCircle;
            Radius = Game.Random.Next(60, 80);
           // VectorX = Game.Random.Next(-15, 15);
           // VectorY = Game.Random.Next(-15, 15);
        }

        public override void Update()
        {
            PositionX += (int)VectorX;
            PositionY += (int)VectorY;
        }

        public EnemyCircle GetNewEnemyCircle(int cursorPositionX, int cursorPositionY)
        {
            double offsetAngle = PositionX - cursorPositionX >= 0
                ? Math.Atan((PositionY - cursorPositionY) / (PositionX - cursorPositionX)) - Math.PI
                : Math.Atan((PositionY - cursorPositionY) / (PositionX - cursorPositionX));

            double newCirclesRadius = Radius / Math.Sqrt(10);

            Point newCirclesPosition = new Point((int)((Radius + newCirclesRadius) * Math.Cos(offsetAngle) + PositionX),
                (int)((Radius + newCirclesRadius) * Math.Sin(offsetAngle) + PositionY));

            EnemyCircle newEnemyCircle = new EnemyCircle(GameFieldHeight, GameFieldWidth, newCirclesRadius);
            newEnemyCircle.SetNewPosition(newCirclesPosition.X, newCirclesPosition.Y);
            newEnemyCircle.SetNewVector(10, 10);

            double gameObjectNewVectorX =
                ((Area - newEnemyCircle.Radius) * VectorX +
                 2 * newEnemyCircle.Area * newEnemyCircle.VectorX) / (Area + newEnemyCircle.Area);

            double gameObjectNewVectorY =
                ((Area - newEnemyCircle.Area) * VectorY +
                 2 * newEnemyCircle.Area * newEnemyCircle.VectorY) / (Area + newEnemyCircle.Area);

            return newEnemyCircle;
        }

        public void SetNewRadius(double radius)
        {
            Radius = radius;
        }

        public void Draw(Graphics graphics)
        {
            graphics.FillEllipse(Brushes.Green, (int)(PositionX - Radius), (int)(PositionY - Radius), (int)Radius * 2, (int)Radius * 2);
        }
    }
}
