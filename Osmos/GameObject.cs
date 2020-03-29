namespace Osmos
{
    internal abstract class GameObject
    {
        public int PositionX { get; protected set; }
        public int PositionY { get; protected set; }
        public int VectorX { get; protected set; }
        public int VectorY { get; protected set; }

    }
}
