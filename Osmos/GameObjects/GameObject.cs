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

        public bool IntersectsWith(GameObject gameObject)
        {
            return GetSquareDistanceToObject(gameObject.PositionX, gameObject.PositionY) <= Math.Pow(Radius + gameObject.Radius, 2) / 4;
        }

        protected int GetSquareDistanceToObject(int objectX, int objectY)
        {
            double componentX = Math.Pow(PositionX - objectX, 2);
            double componentY = Math.Pow(PositionY - objectY, 2);

            return (int)(componentX + componentY);
        }

        public void Draw(Graphics graphics, Brush brush)
        {
            graphics.FillEllipse(brush, PositionX, PositionY, Radius, Radius);
        }
    }
}
