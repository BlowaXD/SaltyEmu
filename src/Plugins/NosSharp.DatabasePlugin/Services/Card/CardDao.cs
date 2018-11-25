using AutoMapper;
using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Data.AccessLayer.Skill;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.Cards;

namespace SaltyEmu.DatabasePlugin.Services.Card
{
    public class CardDao : MappedRepositoryBase<CardDto, CardModel>, ICardService
    {
        public CardDao(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}