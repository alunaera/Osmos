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
            VectorX = Game.Random.Next(-10, 10);
            VectorY = Game.Random.Next(-10, 10);
        }

        public override void Update()
        {
            PositionX += (int)VectorX;
            PositionY += (int)VectorY;
        }

        public void Draw(Graphics graphics)
        {
            graphics.FillEllipse(Brushes.Green, (int)(PositionX - Radius), (int)(PositionY - Radius), (int)Radius * 2, (int)Radius * 2);
        }
    }
}
