using System.Collections.Generic;
using Example;
using Library.Interfaces;

namespace Tests
{
    internal class TestCompanyProjectionRepository : IProjectionRepository<CompanyView>
    {
        private readonly Dictionary<string, CompanyView> _views = new Dictionary<string, CompanyView>();
         
        public void Commit(string id, CompanyView view)
        {
            if (_views.ContainsKey(id))
            {
                _views[id] = view;
            }
            else
            {
                _views.Add(id, view);
            }
        }

        public CompanyView Read(string id)
        {
            if (_views.ContainsKey(id))
            {
                return _views[id];
            }

            return null;
        }

        public void WithExistingView(string id, CompanyView view)
        {
            Commit(id, view);
        }
    }
}