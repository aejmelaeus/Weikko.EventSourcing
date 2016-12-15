using System;
using Library.Interfaces;
using System.Collections.Generic;

namespace Library
{
    public abstract class ProjectionBuilderBase<TEventBase, TView> : 
        IProjectionBuilder<TEventBase>
            where TEventBase : class 
            where TView : class, new()
    {
        private readonly IProjectionRepository _repository;
        private readonly IEventSource<TEventBase> _eventSource;

        private readonly Dictionary<Type, Handler<TEventBase, TView>> _routes = new Dictionary<Type, Handler<TEventBase, TView>>(); 

        protected void RegisterHandler<TEvent>(Func<TEvent, TView, TView> update, Func<TEvent, string> id) where TEvent : class
        {
            _routes.Add(typeof(TEvent), new Handler<TEventBase, TView>((e, v) => update(e as TEvent, v), e => id(e as TEvent)));
        }

        protected ProjectionBuilderBase(IProjectionRepository repository, IEventSource<TEventBase> eventSource)
        {
            _repository = repository;
            _eventSource = eventSource;
        }

        public void Handle(TEventBase @event)
        {
            var eventType = @event.GetType();
            if (!_routes.ContainsKey(eventType)) return;

            var id = _routes[eventType].Id(@event);
            var view = _repository.Read<TView>(id) ?? new TView();

            view = Handle(@event, view);

            _repository.Commit(id, view);
        }

        public void Rebuild(string id)
        {
            var events = _eventSource.Stream(id);
            var view = new TView();

            foreach (var @event in events)
            {
                var eventType = @event.GetType();
                if (_routes.ContainsKey(eventType))
                {
                    view = Handle(@event, view);
                }
            }

            _repository.Commit(id, view);
        }

        private TView Handle(TEventBase @event, TView view)
        {
            var eventType = @event.GetType();
            return _routes[eventType].Handle(@event, view);
        }
    }
}