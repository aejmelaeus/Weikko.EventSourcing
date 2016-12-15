namespace Library.Interfaces
{
    public interface IProjectionRepository<TView> where TView : class 
    {
        void Commit(string id, TView view);
        TView Read(string id);
    }
}