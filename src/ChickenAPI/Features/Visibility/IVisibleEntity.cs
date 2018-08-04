namespace ChickenAPI.Game.Features.Visibility
{
    public interface IVisibleEntity
    {
        VisibilityComponent Visibility { get; }
    }
}