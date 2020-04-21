namespace Osmos
{
    internal class EnemyCircle : GameObject
    {
        public EnemyCircle(int gameFieldWidth, int gameFieldHeight)
        {
            ObjectType = ObjectType.EnemyCircle;
            GameFieldWidth = gameFieldWidth;
            GameFieldHeight = gameFieldHeight;
            PositionX = Game.Random.Next(0, gameFieldWidth);
            PositionY = Game.Random.Next(0, gameFieldHeight);
            Radius = Game.Random.Next(10, 20);
            VectorX = Game.Random.Next(-5, 5);
            VectorY = Game.Random.Next(-5, 5);
        }

        public EnemyCircle(int gameFieldWidth, int gameFieldHeight, double radius)
        {
            ObjectType = ObjectType.EnemyCircle;

            GameFieldWidth = gameFieldWidth;
            GameFieldHeight = gameFieldHeight;
            Radius = radius;
        }
    }
}
