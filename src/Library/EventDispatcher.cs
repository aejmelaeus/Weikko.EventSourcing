using Library.Interfaces;
using System.Collections.Generic;

namespace Library
{
    public class EventDispatcher<TEvent> : IEventDispatcher<TEvent> where TEvent : class
    {
        private readonly List<IHandleMessageSync<TEvent>> _handlers = new List<IHandleMessageSync<TEvent>>();

        public void Register(IHandleMessageSync<TEvent> handler)
        {
            _handlers.Add(handler);    
        }

        public void Dispatch(IEnumerable<TEvent> events)
        {
            foreach (var @event in events)
            {
                foreach (var handler in _handlers)
                {
                    handler.Handle(@event);
                }
            }
        }
    }
}