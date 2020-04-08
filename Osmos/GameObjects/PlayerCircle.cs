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

            Radius = Game.Random.Next(40, 60);
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
            graphics.FillEllipse(Brushes.Green, PositionX - (int)Radius, PositionY - (int)Radius, (int)Radius * 2, (int)Radius * 2);
        }
    }
}
