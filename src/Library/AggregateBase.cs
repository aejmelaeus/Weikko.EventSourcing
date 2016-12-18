using System;
using System.Collections.Generic;

namespace Library
{
    public abstract class AggregateBase<TEvent> where TEvent : class
    {
        // TODO: Figure out this one...
        public string Id { get; }
        internal List<TEvent> UncommitedEvents { get; set; } = new List<TEvent>();
        private readonly Dictionary<Type, Action<TEvent>> _routes = new Dictionary<Type, Action<TEvent>>();

        internal void RegisterTransition<T>(Action<TEvent> transition)
        {
            _routes.Add(typeof(T), transition);
        }
        
        internal void RaiseEvent(TEvent @event)
        {
            if (ApplyEvent(@event))
            {
                UncommitedEvents.Add(@event);
            }
        }

        internal bool ApplyEvent(TEvent @event)
        {
            var eventType = @event.GetType();
            if (_routes.ContainsKey(eventType))
            {
                _routes[eventType](@event);
                return true;
            }
            return false;
        }
    }
}