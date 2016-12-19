using System;
using System.Linq;
using NEventStore;
using Example.Events;
using Library.Interfaces;
using System.Collections.Generic;

namespace Example
{
    public class EventSource : IEventSource<EventBase>
    {
        private readonly IStoreEvents _store;

        public EventSource(IStoreEvents store)
        {
            _store = store;
        }

        public IEnumerable<EventBase> Stream(string id)
        {
            using (var stream = _store.OpenStream(id))
            {
                return stream.CommittedEvents.Select(s => s.Body as EventBase);
            }
        }

        public void Commit(string id, IEnumerable<EventBase> events)
        {
            using (var stream = _store.OpenStream(id))
            {
                foreach (var @event in events)
                {
                    stream.Add(new EventMessage { Body = @event });
                }
                
                stream.CommitChanges(Guid.NewGuid());
            }
        }

        public void Commit(string id, EventBase @event, long sequenceAnchor, Guid commitId)
        {
            throw new NotImplementedException();
        }
    }
}
