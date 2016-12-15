namespace Library.Interfaces
{
    public interface IProjectionBuilder<in TEventBase> where TEventBase : class
    {
        void Rebuild(string id);
        void Handle(TEventBase @event);
    }
}