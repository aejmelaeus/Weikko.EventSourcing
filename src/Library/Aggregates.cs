using Library.Interfaces;
using System.Transactions;

namespace Library
{
    public class Aggregates<TEventBase> : IAggregates<TEventBase> where TEventBase : class, new()
    {
        private readonly IEventSource<TEventBase> _eventSource;
        private readonly IProjections<TEventBase> _projections;

        public Aggregates(IEventSource<TEventBase> eventSource, IProjections<TEventBase> projections)
        {
            _eventSource = eventSource;
            _projections = projections;
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
            //using (var transactionScope = new TransactionScope())
            //{
                _eventSource.Commit(aggregate.Id, aggregate.UncommittedEvents);

                _projections.Update(aggregate.Id, aggregate.UncommittedEvents);
                
            //    transactionScope.Complete();
            //}
        }
    }
}