using AutoMapper;
using ChickenAPI.Game.Data.AccessLayer.Skill;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using NosSharp.DatabasePlugin.Context;
using NosSharp.DatabasePlugin.Models;
using NosSharp.DatabasePlugin.Services.Base;

namespace NosSharp.DatabasePlugin.Services.Card
{
    public class CardDao : MappedRepositoryBase<CardDto, CardModel>, ICardService
    {
        public CardDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}