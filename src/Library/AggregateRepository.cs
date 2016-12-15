using System.Transactions;
using Library.Interfaces;

namespace Library
{
    public class AggregateRepository<TEventBase, TAggregate> 
        where TEventBase : class, new() 
        where TAggregate : AggregateBase<TEventBase>, new()
    {
        private readonly IEventSource<TEventBase> _eventSource;

        public AggregateRepository(IEventSource<TEventBase> eventSource)
        {
            _eventSource = eventSource;
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
                var projectionBuilders = ProjectionBuilders.ListProjectionBuilders<TEventBase>();

                foreach (var projectionBuilder in projectionBuilders)
                {
                    foreach (var uncommitedEvent in aggregate.UncommitedEvents)
                    {
                        // projectionBuilder.Handle(uncommitedEvent);
                    }
                }

                _eventSource.Commit(aggregate.UncommitedEvents);

                transactionScope.Complete();
            }
        }
    }
}