using System;
using Library.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    public abstract class ProjectionBuilderBase<TEventBase, TView> : 
        IProjectionBuilder<TEventBase>
            where TEventBase : class 
            where TView : class, new()
    {
        private readonly IProjectionRepository _repository;

        private readonly Dictionary<Type, Func<TEventBase, TView, TView>> _routes = new Dictionary<Type, Func<TEventBase, TView, TView>>(); 

        protected void RegisterHandler<TEvent>(Func<TEvent, TView, TView> update) where TEvent : class
        {
            _routes.Add(typeof(TEvent), (e, v) => update(e as TEvent, v));
        }

        protected ProjectionBuilderBase(IProjectionRepository repository)
        {
            _repository = repository;
        }

        public void Handle(string id, IEnumerable<TEventBase> events)
        {
            var view = _repository.Read<TView>(id) ?? new TView();

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

        public Type EventBaseType => typeof(TEventBase);

        public void Rebuild(string id, IEnumerable<TEventBase> events)
        {
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
            return _routes[eventType](@event, view);
        }
    }
}