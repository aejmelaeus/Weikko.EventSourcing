using System.Collections.Generic;

namespace Library.Interfaces
{
    public interface IProjections<in TEventBase>
    {
        TView Read<TView>(string id) where TView : class;
        void Rebuild<TView>(string id);
        void Update(string id, IEnumerable<TEventBase> events);
    }

    public interface IAggregates<TEventBase>
    {
        void Commit<TAggregate>(TAggregate aggregate) where TAggregate : IAggregate<TEventBase>;
        TAggregate Read<TAggregate>(string id) where TAggregate : IAggregate<TEventBase>, new();
    }

    public interface IAggregate<TEventBase>
    {
        string Id { get; }
        bool ApplyEvent(TEventBase @event);
        IEnumerable<TEventBase> UncommittedEvents { get; }
        void ClearUncommitedEvents();
    }
}
