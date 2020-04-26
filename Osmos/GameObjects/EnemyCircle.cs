namespace Osmos
{
    internal class EnemyCircle : GameObject
    {
        public EnemyCircle(int gameFieldWidth, int gameFieldHeight)
        {
            GameFieldWidth = gameFieldWidth;
            GameFieldHeight = gameFieldHeight;
            ObjectType = ObjectType.EnemyCircle;

            SetAreaByRadius(Game.Random.Next(10, 20));
            PositionX = Game.Random.Next(0, gameFieldWidth);
            PositionY = Game.Random.Next(0, gameFieldHeight);
            VectorX = Game.Random.Next(-2, 2);
            VectorY = Game.Random.Next(-2, 2);
        }

        public EnemyCircle(int gameFieldWidth, int gameFieldHeight, double radius)
        {
            GameFieldWidth = gameFieldWidth;
            GameFieldHeight = gameFieldHeight;
            ObjectType = ObjectType.EnemyCircle;
            SetAreaByRadius(radius);
        }
    }
}
