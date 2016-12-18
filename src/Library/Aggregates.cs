using Library.Interfaces;
using System.Transactions;

namespace Library
{
    public class Aggregates<TEventBase, TAggregate> 
        where TEventBase : class, new() 
        where TAggregate : AggregateBase<TEventBase>, new()
    {
        private readonly IEventSource<TEventBase> _eventSource;
        private readonly IProjections<TEventBase> _projections;

        public Aggregates(IEventSource<TEventBase> eventSource, IProjections<TEventBase> projections)
        {
            _eventSource = eventSource;
            _projections = projections;
        }

        public AggregateBase<TEventBase> Read(string aggregateId)
        {
            var aggreagate = new TAggregate();

            var events = _eventSource.Stream(aggregateId);

            foreach (var @event in events)
            {
                aggreagate.ApplyEvent(@event);
            }

            return aggreagate;
        }

        public void Commit(AggregateBase<TEventBase> aggregate)
        {
            using (var transactionScope = new TransactionScope())
            {
                _projections.Update(aggregate.Id, aggregate.UncommitedEvents);

                _eventSource.Commit(aggregate.UncommitedEvents);

                transactionScope.Complete();
            }
        }
    }
}