using System;
using System.Collections.Generic;

namespace Library.Interfaces
{
    public interface IEventSource<TEventBase> where TEventBase : class
    {
        IEnumerable<TEventBase> Stream(string id);
        void Commit(string id, IEnumerable<TEventBase> events);
        void Commit(string id, TEventBase @event, long sequenceAnchor, Guid commitId);
    }
}