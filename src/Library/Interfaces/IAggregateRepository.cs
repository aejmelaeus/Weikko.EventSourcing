namespace Library.Interfaces
{
    public interface IAggregateRepository<TEventBase>
    {
        void Commit(IAggregate<TEventBase> aggregate);
        TAggregate Read<TAggregate>(string aggregateId) where TAggregate : IAggregate<TEventBase>, new();
    }
}