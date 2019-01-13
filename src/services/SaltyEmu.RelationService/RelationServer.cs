using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Services.Character;
using SaltyEmu.FriendsPlugin.Protocol;
using SaltyEmu.Redis;
using SaltyEmu.RelationService.DAO;
using SaltyEmu.RelationService.Handling;

namespace SaltyEmu.RelationService
{
    public class RelationServer : MqttIpcServer<RelationServer>
    {
        public RelationServer(MqttServerConfigurationBuilder builder, RedisConfiguration conf) : base(builder)
        {
            var relations = new RelationDao(conf);
            var invitation = new RelationInvitationDao(conf);
            var context = ChickenContainer.Instance.Resolve<SaltyDbContext>();
            var mapper = ChickenContainer.Instance.Resolve<IMapper>();

            RequestHandler.Register<GetRelationsByCharacterId>(new GetRelationsByCharacterIdHandler(relations).Handle);
            RequestHandler.Register<GetRelationsInvitationByCharacterId>(new GetInvitationsByCharacterIdHandler(invitation).Handle);
            RequestHandler.Register<ProcessInvitation>(new ProcessInvitationHandler(invitation, relations, new CharacterDao(context, mapper, new CharacterDto())).Handle);
            RequestHandler.Register<SendInvitation>(new SendInvitationHandler(invitation).Handle);
            RequestHandler.Register<RemoveRelations>(new RemoveRelationsHandler(relations).Handle);
        }

        public new async Task<RelationServer> InitializeAsync()
        {
            await base.InitializeAsync();
            return this;
        }
    }
}