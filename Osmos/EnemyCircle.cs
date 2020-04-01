namespace Osmos
{
    internal class EnemyCircle : GameObject
    {
        public EnemyCircle(int positionX, int positionY, int radius)
        {
            ObjectType = ObjectType.EnemyCircle;
            PositionX = positionX;
            PositionY = positionY;
            Radius = radius;

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
