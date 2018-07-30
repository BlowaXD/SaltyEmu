using System;

namespace ChickenAPI.Utils
{
    public delegate void TypedSenderEventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs e)
    where TSender : class
    where TEventArgs : EventArgs;
}