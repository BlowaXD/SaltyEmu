using ChickenAPI.Game.Game.Components;

namespace ChickenAPI.Game.Entities
{
    public interface INamedEntity
    {
        NameComponent Name { get; set; }
    }
}