using System;
using System.Linq;
using Library.Interfaces;
using System.Collections.Generic;

namespace Library
{
    internal class ProjectionRepository<TEventBase> : IProjectionRepository<TEventBase> where TEventBase : class
    {
        private readonly IViewRepository _viewRepository;
        private readonly IEventSource<TEventBase> _eventSource;
        private readonly Dictionary<Type, IProjectionBuilder<TEventBase>> _projectionBuilders;

        public ProjectionRepository(IEnumerable<IProjectionBuilder<TEventBase>> projectionBuilders, 
            IViewRepository viewRepository, IEventSource<TEventBase> eventSource)
        {
            _viewRepository = viewRepository;
            _eventSource = eventSource;
            _projectionBuilders = projectionBuilders.ToDictionary(pb => pb.ViewType);
        }

        public TView Read<TView>(string id) where TView : class
        {
            return _viewRepository.Read<TView>(id);
        }

        public void Rebuild<TView>(string id)
        {
            var viewType = typeof(TView);

            if (_projectionBuilders.ContainsKey(viewType))
            {
                var events = _eventSource.Stream(id);
                _projectionBuilders[viewType].Rebuild(id, events);
            }
        }

        public void Update(string id, IEnumerable<TEventBase> events)
        {
            events = events.ToList();

            foreach (var projectionBuilder in _projectionBuilders.Values)
            {
                projectionBuilder.Handle(id, events);
            }
        }
    }
}
