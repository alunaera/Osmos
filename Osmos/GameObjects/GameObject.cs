using System;
using System.Drawing;

namespace Osmos
{
    internal abstract class GameObject
    {
        public ObjectType ObjectType { get; protected set; }
        public int PositionX { get; protected set; }
        public int PositionY { get; protected set; }
        public int VectorX { get; protected set; }
        public int VectorY { get; protected set; }
        public int Radius { get; protected set; }

        public double Area => Radius * Radius * Math.PI;

        public abstract void Update();

        public void SetNewPosition(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        public void ProcessingRepulsion(int gameFieldWidth, int gameFieldHeight)
        {
            if (PositionX - Radius < 0)
            {
                SetNewPosition(Radius, PositionY);
                SetOppositeVector(VectorDirection.X);
            }

            if (PositionY - Radius < 0)
            {
                SetNewPosition(PositionX, Radius);
                SetOppositeVector(VectorDirection.Y);
            }

            // In these blocks we multiply radius by 2 for correct animation
            if (PositionX + Radius * 2 > gameFieldWidth)
            {
                SetNewPosition(gameFieldWidth - Radius * 2, PositionY);
                SetOppositeVector(VectorDirection.X);
            }

            if (PositionY + Radius * 2 > gameFieldHeight)
            {
                SetNewPosition(PositionX, gameFieldHeight - Radius * 2);
                SetOppositeVector(VectorDirection.Y);
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

        public bool IntersectsWith(GameObject gameObject)
        {
            return GetSquareDistanceToObject(gameObject.PositionX, gameObject.PositionY) <= Math.Pow(Radius + gameObject.Radius, 2) / 4;
        }

        private int GetSquareDistanceToObject(int objectX, int objectY)
        {
            double componentX = Math.Pow(PositionX - objectX, 2);
            double componentY = Math.Pow(PositionY - objectY, 2);

            return (int)(componentX + componentY);
        }

        public void Draw(Graphics graphics, Brush brush)
        {
            graphics.FillEllipse(brush, PositionX, PositionY, Radius * 2, Radius * 2);
        }
    }
}
