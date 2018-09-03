using AutoMapper;
using ChickenAPI.Game.Data.AccessLayer.Skill;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Card
{
    public class CardDao : MappedRepositoryBase<CardDto, CardModel>, ICardService
    {
        public CardDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}