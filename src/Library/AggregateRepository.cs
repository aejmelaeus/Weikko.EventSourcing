using Library.Interfaces;
using System.Transactions;

namespace Library
{
    public class AggregateRepository<TEventBase> : IAggregateRepository<TEventBase> where TEventBase : class, new()
    {
        private readonly IEventSource<TEventBase> _eventSource;
        private readonly IProjectionRepository<TEventBase> _projectionRepository;

        public AggregateRepository(IEventSource<TEventBase> eventSource, IProjectionRepository<TEventBase> projectionRepository)
        {
            _eventSource = eventSource;
            _projectionRepository = projectionRepository;
        }

        public TAggregate Read<TAggregate>(string id) where TAggregate : IAggregate<TEventBase>, new()
        {
            var aggreagate = new TAggregate();

            var events = _eventSource.Stream(id);

            foreach (var @event in events)
            {
                aggreagate.ApplyEvent(@event);
            }

            return aggreagate;
        }
        
        public void Commit(IAggregate<TEventBase> aggregate)
        {
            using (var transactionScope = new TransactionScope())
            {
                _eventSource.Commit(aggregate.Id, aggregate.UncommittedEvents);

                _projectionRepository.Update(aggregate.Id, aggregate.UncommittedEvents);

                transactionScope.Complete();
            }
        }
    }
}