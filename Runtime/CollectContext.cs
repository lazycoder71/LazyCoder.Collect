namespace LazyCoder.Collect
{
    public class CollectContext
    {
        public int SpawnCount { get; private set; }

        public CollectContext(int spawnCount)
        {
            SpawnCount = spawnCount;
        }
    }
}