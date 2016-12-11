namespace Library.Interfaces
{
    public interface IProjectionBuilder<TEvent>
    {
        void Rebuild(string id);
    }
}