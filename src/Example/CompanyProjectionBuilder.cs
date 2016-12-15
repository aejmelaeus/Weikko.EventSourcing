using Library;
using Example.Events;
using Library.Interfaces;

namespace Example
{
    public class CompanyProjectionBuilder : ProjectionBuilderBase<EventBase, CompanyView>
    {
        // TODO - these should not be ctor params...
        public CompanyProjectionBuilder(IProjectionRepository repository, IEventSource<EventBase> eventSource) : 
            base(repository, eventSource)
        {
            RegisterHandler<CompanyCreated>(Handle, e => e.Id);
            RegisterHandler<CompanyNameUpdated>(Handle, e => e.Id);
            RegisterHandler<CompanyCategoryUpdated>(Handle, e => e.Id);
        }

        private CompanyView Handle(CompanyCategoryUpdated e, CompanyView view)
        {
            view.Category = e.NewCategory;

            return view;
        }

        private CompanyView Handle(CompanyCreated e, CompanyView view)
        {
            view.Id = e.Id;
            view.Name = e.Name;
            view.Category = e.Category;

            return view;
        }

        private CompanyView Handle(CompanyNameUpdated e, CompanyView view)
        {
            view.Name = e.NewName;

            return view;
        }
    }
}
