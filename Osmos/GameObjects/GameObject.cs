using System;
using System.Drawing;

namespace Osmos
{
    internal abstract class GameObject
    {
        public int PositionX { get; protected set; }
        public int PositionY { get; protected set; }
        public int VectorX { get; protected set; }
        public int VectorY { get; protected set; }
        public int Radius { get; protected set; }

        public double Area => Radius * Radius * Math.PI;

        public void Update()
        {
            PositionX += VectorX;
            PositionY += VectorY;
        }

        public void Draw(Graphics graphics, Brush brush)
        {
            graphics.FillEllipse(brush, PositionX, PositionY, Radius, Radius);
        }
    }
}
