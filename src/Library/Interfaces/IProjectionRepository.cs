namespace Library.Interfaces
{
    public interface IProjectionRepository
    {
        void Commit<TView>(TView view);
        TView Read<TView>(string id);
    }
}