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
            SetNewPosition(gameFieldWidth / 2, gameFieldHeight / 2);
            ObjectType = ObjectType.PlayerCircle;
            Radius = Game.Random.Next(60, 80);
        }

        public EnemyCircle GetNewEnemyCircle(int cursorPositionX, int cursorPositionY)
        {
            double offsetAngle = PositionX - cursorPositionX >= 0
                ? Math.Atan((PositionY - cursorPositionY) / (PositionX - cursorPositionX)) - Math.PI
                : Math.Atan((PositionY - cursorPositionY) / (PositionX - cursorPositionX));

            double newCirclesRadius = Radius / Math.Sqrt(20);

            Point newCirclesPosition = new Point(
                (int) ((Radius + newCirclesRadius) * Math.Cos(offsetAngle) + PositionX),
                (int) ((Radius + newCirclesRadius) * Math.Sin(offsetAngle) + PositionY));

            EnemyCircle newEnemyCircle = new EnemyCircle(GameFieldWidth, GameFieldHeight, newCirclesRadius);
            newEnemyCircle.SetNewPosition(newCirclesPosition.X, newCirclesPosition.Y);
            newEnemyCircle.SetNewVector(Math.Cos(offsetAngle) * 20, Math.Sin(offsetAngle) * 20);

            SetNewRadius(Math.Sqrt(Radius * Radius - newCirclesRadius * newCirclesRadius));

            double playerCircleNewVectorX = -newEnemyCircle.VectorX * newEnemyCircle.Area / Area * 3;
            double playerCircleNewVectorY = -newEnemyCircle.VectorY * newEnemyCircle.Area / Area * 3;

            SetNewVector(playerCircleNewVectorX, playerCircleNewVectorY);

            return newEnemyCircle;
        }

        public void Draw(Graphics graphics)
        {
            graphics.FillEllipse(Brushes.Green, (int) (PositionX - Radius), (int) (PositionY - Radius),
                                             (int) Radius * 2, (int) Radius * 2);
        }
    }
}
