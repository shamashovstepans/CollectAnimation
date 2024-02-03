namespace Game
{
    public interface IResourceCollectionSystem
    {
        void Register(IResourceCollector resourceCollector);
        void Remove(IResourceCollector resourceCollector);
    }
}