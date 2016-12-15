using System;
using Library.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    public static class ProjectionBuilders
    {
        public static IEnumerable<IProjectionBuilder<TEventBase>> ListProjectionBuilders<TEventBase>() where TEventBase : class 
        {
            var projectionBuilders = new List<IProjectionBuilder<TEventBase>>();

            var interfaceType = typeof(IProjectionBuilder<TEventBase>);

            // TODO - it should find stuff here!
            var foundTypes = AppDomain.CurrentDomain.GetAssemblies()
                                                    .SelectMany(s => s.GetTypes())
                                                    .Where(p => interfaceType.IsAssignableFrom(p));

            foreach (var type in foundTypes)
            {
                var projectionBuilder = Activator.CreateInstance(type) as IProjectionBuilder<TEventBase>;
                projectionBuilders.Add(projectionBuilder);
            }

            return projectionBuilders;
        }
    }
}