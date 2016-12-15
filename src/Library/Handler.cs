using System;

namespace Library
{
    internal class Handler<TEvent, TView>
    {
        public Handler(Func<TEvent, TView, TView> handle, Func<TEvent, string> id)
        {
            Handle = handle;
            Id = id;
        }

        public Func<TEvent, TView, TView> Handle { get; }
        public Func<TEvent, string> Id { get; } 
    }
}