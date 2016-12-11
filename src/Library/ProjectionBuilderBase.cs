using System;
using System.Collections.Generic;
using Library.Interfaces;

namespace Library
{
    public abstract class ProjectionBuilderBase<TEvent, TView> : 
        IProjectionBuilder<TEvent>, 
        IHandleMessageSync<TEvent> 
            where TEvent : class where TView : new()
    {
        private readonly IProjectionRepository _repository;
        private readonly IEventSource<TEvent> _eventSource;

        private readonly Dictionary<Type, Func<TEvent, TView, string>> _routes = new Dictionary<Type, Func<TEvent, TView, string>>();

        internal void RegisterTransition(Func<TEvent, TView, string> transition)
        {
            _routes.Add(typeof(TEvent), transition);
        }

        protected ProjectionBuilderBase(IProjectionRepository repository, IEventSource<TEvent> eventSource)
        {
            _repository = repository;
            _eventSource = eventSource;
        }

        public void Rebuild(string id)
        {
            var events = _eventSource.Stream(id);
            var view = new TView();

            foreach (var @event in events)
            {
                ApplyEvent(@event, view);
            }

            _repository.Commit(view);
        }

        private void ApplyEvent(TEvent @event, TView view)
        {
            var eventType = @event.GetType();
            if (_routes.ContainsKey(eventType))
            {
                _routes[eventType](@event, view);
            }
        }

        public void Handle(TEvent message)
        {
            // TODO - figure out what to do here!
            // var view = _repository.Read<TView>()
        }
    }
}