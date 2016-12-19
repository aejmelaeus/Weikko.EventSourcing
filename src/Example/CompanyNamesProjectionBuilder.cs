using Library;
using Example.Events;

namespace Example
{
    public class CompanyNamesProjectionBuilder : ProjectionBuilderBase<EventBase, CompanyNamesView>
    {
        public CompanyNamesProjectionBuilder()
        {
            RegisterHandler<CompanyCreated>(Handle);
            RegisterHandler<CompanyNameUpdated>(Handle);
        }

        private CompanyNamesView Handle(CompanyCreated e, CompanyNamesView view)
        {
            view.Id = e.Id;
            view.Names.Add(e.Name);

            return view;
        }

        private CompanyNamesView Handle(CompanyNameUpdated e, CompanyNamesView view)
        {
            view.Names.Add(e.NewName);

            return view;
        }
    }
}