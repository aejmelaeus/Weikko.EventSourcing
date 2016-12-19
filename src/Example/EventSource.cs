using System;
using System.Linq;
using NEventStore;
using Library.Interfaces;
using System.Collections.Generic;

namespace Example
{
    public class EventSource<TEventBase> : IEventSource<TEventBase> where TEventBase : class
    {
        private readonly IStoreEvents _store;

        public EventSource(IStoreEvents store)
        {
            _store = store;
        }

        public IEnumerable<TEventBase> Stream(string id)
        {
            using (var stream = _store.OpenStream(id))
            {
                return stream.CommittedEvents.Select(s => s.Body as TEventBase);
            }
        }

        public void Commit(string id, IEnumerable<TEventBase> events)
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

        public void Commit(string id, TEventBase @event, long sequenceAnchor, Guid commitId)
        {
            throw new NotImplementedException();
        }
    }
}
