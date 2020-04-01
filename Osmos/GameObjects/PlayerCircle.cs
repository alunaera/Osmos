using System.Drawing;

namespace Osmos
{
    internal class PlayerCircle : GameObject
    {
        public PlayerCircle(int positionX, int positionY)
        {
            ObjectType = ObjectType.PlayerCircle;
            PositionX = positionX;
            PositionY = positionY;

            Radius = 15;
            VectorX = 1;
            VectorY = -1;
        }

        public override void Update()
        {
            PositionX += VectorX;
            PositionY += VectorY;
        }

        public void Draw(Graphics graphics)
        {
            graphics.FillEllipse(Brushes.Green, PositionX, PositionY, Radius * 2, Radius * 2);
        }
    }
}
