namespace Library.Interfaces
{
    public interface IHandleMessageSync<in TMessage>
    {
        void Handle(TMessage message);
    }
}
