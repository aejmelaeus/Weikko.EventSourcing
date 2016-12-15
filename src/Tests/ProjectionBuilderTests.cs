using Example;
using NSubstitute;
using Example.Events;
using NUnit.Framework;
using Library.Interfaces;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class ProjectionBuilderTests
    {
        [Test]
        public void Handle_WhenHandlingCompanyCreatedEvent_UpdateCalledOnRepository()
        {
            // Arrange
            var eventStore = Substitute.For<IEventSource<EventBase>>();
            var projectionRepository = new TestCompanyProjectionRepository();

            var projectionBuilder = new CompanyProjectionBuilder(projectionRepository, eventStore);

            const string id = "SomeId";
            const string name = "SomeName";
            const string category = "SomeCategory";

            // Act
            projectionBuilder.Handle(new CompanyCreated
            {
                Name = name,
                Id = id,
                Category = category
            });

            var view = projectionRepository.Read(id);
            
            // Assert
            Assert.That(view.Id, Is.EqualTo(id));
            Assert.That(view.Name, Is.EqualTo(name));
            Assert.That(view.Category, Is.EqualTo(category));
        }

        [Test]
        public void Handle_WhenHandlingCompanyUpdatedEvent_EventAppliedOnExistingCompany()
        {
            const string id = "SomeId";
            const string existingName = "SomeName";
            const string theNewCategory = "TheNewCategory";

            var eventStore = Substitute.For<IEventSource<EventBase>>();
            var projectionRepository = new TestCompanyProjectionRepository();

            projectionRepository.WithExistingView(id, new CompanyView
            {
                Id = id,
                Name = existingName,
                Category = "SomeCategory"
            });

            var projectionBuilder = new CompanyProjectionBuilder(projectionRepository, eventStore);

            // Act
            projectionBuilder.Handle(new CompanyCategoryUpdated
            {
                Id = id,
                NewCategory = theNewCategory
            });

            var view = projectionRepository.Read(id);

            Assert.That(view.Id, Is.EqualTo(id));
            Assert.That(view.Name, Is.EqualTo(existingName));
            Assert.That(view.Category, Is.EqualTo(theNewCategory));
        }

        [Test]
        public void Rebuild_WhenInvoked_AllEventsReplayedOnProjection()
        {
            // Arrange
            const string id = "SomeId";
            const string theNewName = "TheNewName";
            const string theNewCategory = "TheNewCategory";

            var events = new List<EventBase>
            {
                new CompanyCreated
                {
                    Id = id,
                    Name = "SomeName"
                },
                new CompanyNameUpdated
                {
                    Id = id,
                    NewName = theNewName
                },
                new CompanyCategoryUpdated
                {
                    Id = id,
                    NewCategory = theNewCategory
                }
            };

            var eventStore = Substitute.For<IEventSource<EventBase>>();
            var projectionRepository = new TestCompanyProjectionRepository();

            var projectionBuilder = new CompanyProjectionBuilder(projectionRepository, eventStore);

            eventStore.Stream(id).Returns(events);

            // Act
            projectionBuilder.Rebuild(id);
            var view = projectionRepository.Read(id);

            // Assert
            Assert.That(view.Id, Is.EqualTo(id));
            Assert.That(view.Name, Is.EqualTo(theNewName));
            Assert.That(view.Category, Is.EqualTo(theNewCategory));
        }
    }
}
