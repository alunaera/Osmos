using System;
using System.Drawing;

namespace Osmos
{
    internal abstract class GameObject
    {
        protected int GameFieldWidth;
        protected int GameFieldHeight;
        protected double PositionX { get; set; }
        protected double PositionY { get; set; }

        public ObjectType ObjectType { get; protected set; }
        public double VectorX { get; protected set; }
        public double VectorY { get; protected set; }
        public double Radius { get; protected set; }
        public double Area => Radius * Radius * Math.PI;
        public double Impulse => Area * Math.Sqrt(VectorX * VectorX + VectorY * VectorY);

        public void Update(GameMode gameMode)
        {
            PositionX += VectorX;
            PositionY += VectorY;

            ProcessingMoving(gameMode);
        }

        public void SetNewPosition(double positionX, double positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        public void SetNewRadius(double radius)
        {
            Radius = radius;
        }

        public void SetNewVector(double vectorX, double vectorY)
        {
            VectorX = vectorX;
            VectorY = vectorY;
        }

        private void ProcessingMoving(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.Repulsion:
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

                    if (PositionX + Radius >= GameFieldWidth)
                    {
                        PositionX = GameFieldWidth - Radius;
                        SetOppositeVector(VectorDirection.X);
                    }

                    if (PositionY + Radius >= GameFieldHeight)
                    {
                        PositionY = GameFieldHeight - Radius;
                        SetOppositeVector(VectorDirection.Y);
                    }

                    break;

                case GameMode.Cycle:
                    if (PositionX + Radius < 0)
                        PositionX = GameFieldWidth + Radius;

                    if (PositionY + Radius < 0)
                        PositionY = GameFieldHeight + Radius;

                    if (PositionX - Radius >= GameFieldWidth)
                        PositionX = -Radius;

                    if (PositionY - Radius >= GameFieldHeight)
                        PositionY = -Radius;

                    break;
            }
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

        public double GetDistanceToObject(GameObject gameObject)
        {
            double componentX = Math.Pow(PositionX - gameObject.PositionX, 2);
            double componentY = Math.Pow(PositionY - gameObject.PositionY, 2);

            return Math.Sqrt(componentX + componentY);
        }

        public void Draw(Graphics graphics, Brush brush)
        {
            graphics.FillEllipse(brush, (int)(PositionX - Radius), (int)(PositionY - Radius), (int)Radius * 2, (int)Radius * 2);
        }
    }
}
