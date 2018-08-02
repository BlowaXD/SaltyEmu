using ChickenAPI.Game.Components;

namespace ChickenAPI.Game.Entities
{
    public interface INamedEntity
    {
        NameComponent Name { get; set; }
    }
}