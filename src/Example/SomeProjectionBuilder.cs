using Library;
using Library.Interfaces;

namespace Example
{
    public class SomeProjectionBuilder 
    {
        private readonly IProjectionRepository _repository;
        private readonly IEventSource<EventBase> _eventSource;

        public SomeProjectionBuilder(IProjectionRepository repository, IEventSource<EventBase> eventSource)
        {
            _repository = repository;
            _eventSource = eventSource;
        }
    }
}
