namespace ChickenAPI.Core.Utils
{
    public delegate void EventHandlerWithoutArgs<in TSender>(TSender sender) where TSender : class;
}