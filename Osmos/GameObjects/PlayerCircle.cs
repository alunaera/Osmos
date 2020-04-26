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
            ObjectType = ObjectType.PlayerCircle;

            SetAreaByRadius(Game.Random.Next(60, 80));
            PositionX = Game.Random.Next(0, gameFieldWidth);
            PositionY = Game.Random.Next(0, gameFieldHeight);
        }

        public EnemyCircle GetNewEnemyCircle(int cursorPositionX, int cursorPositionY)
        {
            double offsetAngle = PositionX - cursorPositionX >= 0
                ? Math.Atan((PositionY - cursorPositionY) / (PositionX - cursorPositionX)) - Math.PI
                : Math.Atan((PositionY - cursorPositionY) / (PositionX - cursorPositionX));
            double newCirclesRadius = Radius / Math.Sqrt(25);
            double newCirclesPositionX = (Radius + newCirclesRadius) * Math.Cos(offsetAngle) + PositionX;
            double newCirclesPositionY = (Radius + newCirclesRadius) * Math.Sin(offsetAngle) + PositionY;

            EnemyCircle newEnemyCircle = new EnemyCircle(GameFieldWidth, GameFieldHeight, newCirclesRadius);

            newEnemyCircle.SetNewPosition(newCirclesPositionX, newCirclesPositionY);
            newEnemyCircle.SetNewVector(Math.Cos(offsetAngle) * 20, Math.Sin(offsetAngle) * 20);

            SetAreaByRadius(Math.Sqrt(Radius * Radius - newCirclesRadius * newCirclesRadius));

            double playerCircleNewVectorX = (VectorX - newEnemyCircle.VectorX * newEnemyCircle.Area) / Area;
            double playerCircleNewVectorY = (VectorY - newEnemyCircle.VectorY * newEnemyCircle.Area) / Area;

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
