using Library;
using Example.Events;
using Library.Interfaces;

namespace Example
{
    public class CompanyProjectionBuilder : ProjectionBuilderBase<EventBase, CompanyView>
    {
        // TODO - these should not be ctor params...
        public CompanyProjectionBuilder(IProjectionRepository repository) : 
            base(repository)
        {
            RegisterHandler<CompanyCreated>(Handle);
            RegisterHandler<CompanyNameUpdated>(Handle);
            RegisterHandler<CompanyCategoryUpdated>(Handle);
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
