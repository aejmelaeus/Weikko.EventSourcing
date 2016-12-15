using System;
using Library.Interfaces;

namespace Library
{
    public class SqlProjectionRepository : IProjectionRepository
    {
        public void Commit<TView>(string id, TView view) where TView : class
        {
            throw new NotImplementedException();
        }

        public TView Read<TView>(string id) where TView : class
        {
            throw new NotImplementedException();
        }
    }
}