using System.Collections.Generic;
using Library;
using Example.Events;

namespace Example
{
    public class CompanyAggregate : AggregateBase<EventBase>
    {
        private string _id;
        private string _currentName;
        private string _category;
        private List<string> _names = new List<string>();

        public CompanyAggregate()
        {
            RegisterTransition<CompanyCreated>(Handle);
            RegisterTransition<CompanyNameUpdated>(Handle);
        }

        public void CreateCompany(string id, string name, string category)
        {
            RaiseEvent(new CompanyCreated
            {
                Id = id,
                Name = name,
                Category = category
            });
        }

        public void UpdateName(string id, string newName)
        {
            if (_names.Contains(newName)) return;

            RaiseEvent(new CompanyNameUpdated
            {
                Id = id,
                NewName = newName
            });
        }

        private void Handle(CompanyCreated e)
        {
            _id = e.Id;
            _currentName = e.Name;
            _category = e.Category;
            _names.Add(e.Name);
        }

        private void Handle(CompanyNameUpdated e)
        {
            _currentName = e.NewName;
            _names.Add(e.NewName);
        }

        public override string Id => _id;
    }
}
