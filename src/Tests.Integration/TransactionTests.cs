using System;
using System.Collections.Generic;
using Autofac;
using Example;
using Example.Events;
using Library;
using Library.Interfaces;
using NEventStore;
using NEventStore.Persistence.Sql.SqlDialects;
using NUnit.Framework;

namespace Tests.Integration
{
    [TestFixture]
    public class TransactionTests
    {
        private IContainer _container;

        /*
        ** Some TODOs:
        ** - Merge the Sequenced Aggregate into here...
        ** - Create some smart configuration.
        */

        [Test]
        public void Commit_WhenThereIsAProjectionBuilderThatCrashes_WriteToEventStreamNotCommitted()
        {
            // Shiiiet...

            // Arrange
            //string id = Guid.NewGuid().ToString();
            
            //var transaction = new TransactionAggregate();
            //transaction.CreateTransaction(id);

            //var projectionRepository = new SqlServerProjectionRepository();
            //var projectionBuilder = new TransactionProjectionBuilder
            //{
            //    ProjectionRepository = projectionRepository
            //};

            //var projectionBuilders = new List<IProjectionBuilder<TransactionEventBase>>
            //{
            //    projectionBuilder
            //};

            //var storeEvents = GetEventSource();
            //var eventSource = new EventSource(storeEvents);

            //var aggregates = new Aggregates<TransactionEventBase>(eventSource, );


            //try
            //{
            //    // Act
            //    aggregates.Commit(transaction);
            //}
            //catch (Exception)
            //{
            //    // Assert
            //    var transactionFromDb = aggregates.Read<TransactionAggregate>(id);
            //    Assert.That(string.IsNullOrEmpty(transactionFromDb.Id));
            //}
        }

        private IStoreEvents GetEventSource()
        {
            return Wireup
                .Init()
                .UsingSqlPersistence("EventSource")
                .WithDialect(new MsSqlDialect())
                .EnlistInAmbientTransaction()
                .InitializeStorageEngine()
                .UsingJsonSerialization()
                .Compress()
                .Build();
        }
    }

    public class TransactionEventBase
    {
        // Nothing here...
    }

    public class TransactionCreated : TransactionEventBase
    {
        public string Id { get; set; }
    }

    public class TransactionAggregate : AggregateBase<TransactionEventBase>
    {
        private string _id;

        public override string Id => _id;

        public TransactionAggregate()
        {
            RegisterTransition<TransactionCreated>(Handler);
        }

        public void CreateTransaction(string id)
        {
            RaiseEvent(new TransactionCreated
            {
                Id = id
            });
        }

        private void Handler(TransactionCreated e)
        {
            _id = e.Id;
        }
    }

    public class TransactionProjectionBuilder : ProjectionBuilderBase<TransactionEventBase, TransactionView>
    {
        public TransactionProjectionBuilder()
        {
            RegisterHandler<TransactionCreated>(Handler);
        }

        private TransactionView Handler(TransactionCreated e, TransactionView view)
        {
            throw new Exception("Nasty stuff - We need to make sure that the Domain stays in sync. Let's hope the Transactions are configured!");
        }
    }

    public class TransactionView
    {
        public string Id { get; set; }
    }
}
