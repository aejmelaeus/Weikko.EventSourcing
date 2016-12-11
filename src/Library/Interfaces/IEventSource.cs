using System;
using System.Collections.Generic;

namespace Library.Interfaces
{
    public interface IEventSource<TEvent> where TEvent : class
    {
        IEnumerable<TEvent> Stream(string id);
        void Commit(IEnumerable<TEvent> events);
        void Commit(TEvent @event, long sequenceAnchor, Guid commitId);
    }
}