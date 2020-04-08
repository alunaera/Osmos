namespace Osmos
{
    internal class EnemyCircle : GameObject
    {
        public EnemyCircle(int gameFieldWidth, int gameFieldHeight)
        {
            ObjectType = ObjectType.EnemyCircle;
            PositionX = Game.Random.Next(0, gameFieldWidth);
            PositionY = Game.Random.Next(0, gameFieldHeight);
            Radius = Game.Random.Next(10, 40);

            VectorX = Game.Random.Next(-1, 1);
            VectorY = Game.Random.Next(-1, 1);
        }

        public override void Update()
        {
            PositionX += VectorX;
            PositionY += VectorY;
        }
    }
}
