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

            VectorX = 0;
            VectorY = 0;
        }

        public override void Update()
        {
            PositionX += VectorX;
            PositionY += VectorY;
        }
    }
}
