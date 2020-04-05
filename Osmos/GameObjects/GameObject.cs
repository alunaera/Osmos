﻿using System;
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
        public double Radius { get; protected set; }

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
                SetNewPosition((int)Radius, PositionY);
                SetOppositeVector(VectorDirection.X);
            }

            if (PositionY - Radius < 0)
            {
                SetNewPosition(PositionX, (int)Radius);
                SetOppositeVector(VectorDirection.Y);
            }

            // In these blocks we multiply radius by 2 for correct animation
            if (PositionX + Radius * 2 > gameFieldWidth)
            {
                SetNewPosition(gameFieldWidth - (int)Radius * 2, PositionY);
                SetOppositeVector(VectorDirection.X);
            }

            if (PositionY + Radius * 2 > gameFieldHeight)
            {
                SetNewPosition(PositionX, gameFieldHeight - (int)Radius * 2);
                SetOppositeVector(VectorDirection.Y);
            }
        }

        public void ChangeRadius(double valueOfChange)
        {
            Radius += valueOfChange;
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
            graphics.FillEllipse(brush, PositionX, PositionY, (int)Radius * 2, (int)Radius * 2);
        }
    }
}
