using System;
using System.Drawing;

namespace Osmos
{
    internal abstract class GameObject
    {
        public ObjectType ObjectType { get; protected set; }
        public double PositionX { get; protected set; }
        public double PositionY { get; protected set; }
        public double VectorX { get; protected set; }
        public double VectorY { get; protected set; }
        public double Radius { get; protected set; }

        public double Area => Radius * Radius * Math.PI;
        public double Vector => Math.Sqrt(VectorX * VectorX + VectorY * VectorY);
        public double Impulse => Area * Vector;

        public abstract void Update();

        public void SetNewPosition(double positionX, double positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        public void ProcessingRepulsion(int gameFieldWidth, int gameFieldHeight)
        {
            if (PositionX - Radius < 0)
            {
                SetNewPosition((int)Radius, PositionY);
                SetOppositeVector(VectorDirection.X);
            }

            if (PositionY - Radius < 0)
            {
                SetNewPosition(PositionX, (int)Radius);
                SetOppositeVector(VectorDirection.Y);
            }

            if (PositionX + Radius >= gameFieldWidth)
            {
                SetNewPosition(gameFieldWidth - (int)Radius, PositionY);
                SetOppositeVector(VectorDirection.X);
            }

            if (PositionY + Radius >= gameFieldHeight)
            {
                SetNewPosition(PositionX, gameFieldHeight - (int)Radius);
                SetOppositeVector(VectorDirection.Y);
            }
        }

        public void ChangeRadius(double valueOfChange)
        {
            Radius += valueOfChange;
        }

        public void SetNewVector(double vectorX, double vectorY)
        {
            VectorX = vectorX;
            VectorY = vectorY;
        }

        public void ChangeVector(double reductionPercentage)
        {
            VectorX *= reductionPercentage;
            VectorY *= reductionPercentage;
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
