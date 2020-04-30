using System;
using System.Drawing;

namespace Osmos
{
    internal class Circle
    {
        private readonly int gameFieldWidth;
        private readonly int gameFieldHeight;

        private double PositionX { get; set; }
        private double PositionY { get; set; }

        public CircleType CircleType { get; }
        public double VectorX { get; private set; }
        public double VectorY { get; private set; }
        public double Area { get; set; }

        public double Radius => Math.Sqrt(Area / Math.PI);
        public double ImpulseX => Area * VectorX;
        public double ImpulseY => Area * VectorY;

        public Circle(int gameFieldWidth, int gameFieldHeight, CircleType circleType)
        {
            this.gameFieldWidth = gameFieldWidth;
            this.gameFieldHeight = gameFieldHeight;

            PositionX = Game.Random.Next(0, gameFieldWidth);
            PositionY = Game.Random.Next(0, gameFieldHeight);

            CircleType = circleType;

            switch (circleType)
            {
                case CircleType.EnemyCircle:
                    SetAreaByRadius(Game.Random.Next(10, 20));
                    VectorX = Game.Random.Next(-2, 2);
                    VectorY = Game.Random.Next(-2, 2);
                    break;
                case CircleType.PlayerCircle:
                    SetAreaByRadius(Game.Random.Next(40, 60));
                    VectorX = 0;
                    VectorY = 0;
                    break;
            }
        }

        public void Update(GameMode gameMode)
        {
            PositionX += VectorX;
            PositionY += VectorY;

            ProcessingMoving(gameMode);
        }

        private void ProcessingMoving(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.Repulsion:
                case GameMode.ManyCircles:
                    if (PositionX - Radius < 0)
                    {
                        PositionX = Radius;
                        SetOppositeVector(VectorDirection.X);
                    }

                    if (PositionY - Radius < 0)
                    {
                        PositionY = Radius;
                        SetOppositeVector(VectorDirection.Y);
                    }

                    if (PositionX + Radius >= gameFieldWidth)
                    {
                        PositionX = gameFieldWidth - Radius;
                        SetOppositeVector(VectorDirection.X);
                    }

                    if (PositionY + Radius >= gameFieldHeight)
                    {
                        PositionY = gameFieldHeight - Radius;
                        SetOppositeVector(VectorDirection.Y);
                    }

                    break;

                case GameMode.Cycle:
                    if (PositionX + Radius < 0)
                        PositionX = gameFieldWidth + Radius;

                    if (PositionY + Radius < 0)
                        PositionY = gameFieldHeight + Radius;

                    if (PositionX - Radius > gameFieldWidth)
                        PositionX = 0;

                    if (PositionY - Radius > gameFieldHeight)
                        PositionY = 0;

                    break;
            }
        }

        private void SetAreaByRadius(double radius)
        {
            Area = radius * radius * Math.PI;
        }

        public void SetNewVector(double vectorX, double vectorY)
        {
            VectorX = vectorX;
            VectorY = vectorY;
        }

        public Circle GetNewEnemyCircle(int cursorPositionX, int cursorPositionY)
        {
            double offsetAngle = PositionX - cursorPositionX >= 0
                ? Math.Atan((PositionY - cursorPositionY) / (PositionX - cursorPositionX)) - Math.PI
                : Math.Atan((PositionY - cursorPositionY) / (PositionX - cursorPositionX));

            double newCirclesRadius = Radius / 5;

            Circle newEnemyCircle = new Circle(gameFieldWidth, gameFieldHeight, CircleType.EnemyCircle)
            {
                PositionX = (Radius + newCirclesRadius) * Math.Cos(offsetAngle) + PositionX,
                PositionY = (Radius + newCirclesRadius) * Math.Sin(offsetAngle) + PositionY,
                VectorX = Math.Cos(offsetAngle) * 20,
                VectorY = Math.Sin(offsetAngle) * 20,
                Area = newCirclesRadius * newCirclesRadius * Math.PI
            };

            Area -= newEnemyCircle.Area;

            VectorX += (VectorX - newEnemyCircle.VectorX * newEnemyCircle.Area) / Area;
            VectorY += (VectorY - newEnemyCircle.VectorY * newEnemyCircle.Area) / Area;

            return newEnemyCircle;
        }

        private void SetOppositeVector(VectorDirection vectorDirection)
        {
            switch (vectorDirection)
            {
                case VectorDirection.X:
                    VectorX = -VectorX;
                    break;
                case VectorDirection.Y:
                    VectorY = -VectorY;
                    break;
            }
        }

        public double GetDistanceToObject(Circle circle)
        {
            double componentX = (PositionX - circle.PositionX) * (PositionX - circle.PositionX);
            double componentY = (PositionY - circle.PositionY) * (PositionY - circle.PositionY);

            return componentX + componentY;
        }

        public void Draw(Graphics graphics, Brush brush)
        {
            graphics.FillEllipse(brush, (int) (PositionX - Radius), (int) (PositionY - Radius), 
                (float) Radius * 2, (float) Radius * 2);
        }
    }
}
