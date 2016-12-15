using System.Linq;
using Example.Events;
using Library;
using Library.Interfaces;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ProjectionBuildersTests
    {
        [Test]
        public void ListProjectionBuilders_WhenProjectionBuildersExists_Listed()
        {
            // Arrange & Act
            var projectionBuilders = ProjectionBuilders.ListProjectionBuilders<IProjectionBuilder<EventBase>>();

            // Assert
            Assert.That(projectionBuilders.Count, Is.AtLeast(2));
        }
    }

    public interface IProjectionBuilder2
    {
        // Nothing here...
    }

    public class ProjectionBuilderOne : IProjectionBuilder<EventBase>
    {
        public void Rebuild(string id)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(EventBase @event)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ProjectionBuilderTwo : IProjectionBuilder<EventBase>
    {
        public void Rebuild(string id)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(EventBase @event)
        {
            throw new System.NotImplementedException();
        }
    }
}
