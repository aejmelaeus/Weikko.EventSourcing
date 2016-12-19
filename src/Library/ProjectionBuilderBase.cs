﻿using System;
using System.Linq;
using Library.Interfaces;
using System.Collections.Generic;

namespace Library
{
    public abstract class ProjectionBuilderBase<TEventBase, TView> : 
        IProjectionBuilder<TEventBase>
            where TEventBase : class 
            where TView : class, new()
    {
        public IViewRepository ViewRepository { get; set; }

        public Type ViewType => typeof(TView);

        private readonly Dictionary<Type, Func<TEventBase, TView, TView>> _routes = new Dictionary<Type, Func<TEventBase, TView, TView>>();

        protected void RegisterHandler<TEvent>(Func<TEvent, TView, TView> update) where TEvent : class
        {
            _routes.Add(typeof(TEvent), (e, v) => update(e as TEvent, v));
        }

        public void Handle(string id, IEnumerable<TEventBase> events)
        {
            // TODO - Hmmm..
            var materializedEvents = events?.ToList() ?? new List<TEventBase>();

            if (!AnyEventsToHandle(materializedEvents)) return;

            var view = ViewRepository.Read<TView>(id) ?? new TView();

            view = Handle(materializedEvents, view);

            ViewRepository.Commit(id, view);
        }

        public void Rebuild(string id, IEnumerable<TEventBase> events)
        {
            var view = new TView();

            view = Handle(events, view);

            ViewRepository.Commit(id, view);
        }

        private TView Handle(IEnumerable<TEventBase> events, TView view)
        {
            foreach (var @event in events)
            {
                var eventType = @event.GetType();

                if (_routes.ContainsKey(eventType))
                {
                    view = _routes[eventType](@event, view);
                }
            }

            return view;
        }

        private bool AnyEventsToHandle(List<TEventBase> materializedEvents)
        {
            var handlerTypes = _routes.Keys;
            var eventTypes = materializedEvents.Select(e => e.GetType());

            return handlerTypes.Intersect(eventTypes).Any();
        }
    }
}