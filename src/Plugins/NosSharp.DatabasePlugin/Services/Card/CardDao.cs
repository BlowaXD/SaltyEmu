using AutoMapper;
using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Data.AccessLayer.Skill;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models;
using SaltyEmu.DatabasePlugin.Models.Cards;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Card
{
    public class CardDao : MappedRepositoryBase<CardDto, CardModel>, ICardService
    {
        public CardDao(SaltyDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}