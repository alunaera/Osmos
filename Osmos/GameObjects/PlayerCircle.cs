namespace Osmos
{
    class PlayerCircle : GameObject
    {
        public PlayerCircle(int positionX, int positionY)
        {
            ObjectType = ObjectType.PlayerCircle;
            PositionX = positionX;
            PositionY = positionY;

            Radius = 10;
            VectorX = 1;
            VectorY = -1;
        }

        public override void Update()
        {
            PositionX += VectorX;
            PositionY += VectorY;
        }
    }
}
