using System.Threading.Tasks;

namespace ChickenAPI.Game.Quicklist
{
    public interface IQuicklist
    {
        Task AddElement(QuicklistElement element);
        Task RemoveElement(QuicklistComponent element);
        Task<QuicklistElement> GetElementBySlot(short slot);
    }
}