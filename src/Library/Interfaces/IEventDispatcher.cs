using System.Collections.Generic;

namespace Library.Interfaces
{
    public interface IEventDispatcher<TEvent> where TEvent : class
    {
        void Register(IHandleMessageSync<TEvent> handler);
        void Dispatch(IEnumerable<TEvent> events);
    }
}