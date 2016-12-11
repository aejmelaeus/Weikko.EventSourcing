using System.Transactions;
using Library.Interfaces;

namespace Library
{
    public class AggregateRepository<TEvent, TAggregate> where TEvent : class, new() where TAggregate : AggregateBase<TEvent>, new()
    {
        private readonly IEventSource<TEvent> _eventSource;
        private readonly IEventDispatcher<TEvent> _eventDispatcher;

        public AggregateRepository(IEventSource<TEvent> eventSource, IEventDispatcher<TEvent> eventDispatcher)
        {
            _eventSource = eventSource;
            _eventDispatcher = eventDispatcher;
        }

        public AggregateBase<TEvent> Read(string aggregateId)
        {
            var aggreagate = new TAggregate();

            var events = _eventSource.Stream(aggregateId);

            foreach (var @event in events)
            {
                aggreagate.ApplyEvent(@event);
            }

            return aggreagate;
        }

        public void Commit(AggregateBase<TEvent> aggregate)
        {
            using (var transactionScope = new TransactionScope())
            {
                _eventDispatcher.Dispatch(aggregate.UncommitedEvents);
                _eventSource.Commit(aggregate.UncommitedEvents);
                transactionScope.Complete();
            }
        }
    }
}