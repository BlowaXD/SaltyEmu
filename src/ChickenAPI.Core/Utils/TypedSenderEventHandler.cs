using System;

namespace ChickenAPI.Core.Utils
{
    public delegate void TypedSenderEventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs e)
    where TSender : class
    where TEventArgs : EventArgs;

    public delegate void EventHandlerWithoutArgs<in TSender>(TSender sender) where TSender : class;
}