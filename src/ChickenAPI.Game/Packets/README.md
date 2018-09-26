# ChickenAPI Packet list


## CharacterScreen Packets (only in character selection screen)

### Sent by Client

- [x] [Char_DEL](CharacterScreen/Client/CharacterDeletePacketBase.cs)
- [x] [Char_NEW](CharacterScreen/Client/CharNewPacketBase.cs)
- [x] [EntryPoint](CharacterScreen/Client/EntryPointPacketBase.cs)
- [x] [game_start](CharacterScreen/Client/GameStartPacketBase.cs)
- [x] [select](CharacterScreen/Client/SelectPacketBase.cs)


### Sent by Server

- [x] [clist](CharacterScreen/Server/ClistPacketBase.cs)
- [x] [clist_end](CharacterScreen/Server/ClistEndPacketBase.cs)
- [x] [clist_start](CharacterScreen/Server/ClistStartPacketBase.cs)
- [x] [info](CharacterScreen/Server/InfoPacketBase.cs)
- [x] [OK](CharacterScreen/Server/OkPacketBase.cs)


## Game Packets (sent/received only in game)

### Sent by Client

- [x] [addobj](Game/Client/AddobjPacket.cs)
- [x] [b_i](Game/Client/BiPacket.cs)
- [x] [compl](Game/Client/ComplimentPacket.cs)
- [x] [dir](Game/Client/DirectionPacket.cs)
- [x] [drop](Game/Client/DropPacket.cs)
- [x] [eff](Game/Client/EffPacket.cs)
- [x] [eq](Game/Client/EqPacket.cs)
- [x] [eqinfo](Game/Client/EquipmentInfoPacket.cs)
- [x] [get](Game/Client/GetPacket.cs)
- [x] [glmk](Game/Client/CreateFamilyPacket.cs)
- [x] [gop](Game/Client/CharacterOptionPacket.cs)
- [x] [mlobj](Game/Client/MlObjPacket.cs)
- [x] [mvi](Game/Client/MviPacket.cs)
- [x] [ncif](Game/Client/NcifPacket.cs)
- [x] [pairy](Game/Client/PairyPacket.cs)
- [x] [put](Game/Client/PutPacket.cs)
- [x] [rmvobj](Game/Client/RmvobjPacket.cs)
- [x] [say](Game/Client/ClientSayPacket.cs)
- [x] [sc](Game/Client/ScPacket.cs)
- [x] [shopping](Game/Client/ShoppingPacket.cs)
- [x] [u_as](Game/Client/UseAoeSkillPacket.cs)
- [x] [u_s](Game/Client/UseSkillPacket.cs)
- [x] [walk](Game/Client/WalkPacket.cs)
- [x] [wear](Game/Client/WearPacket.cs)


### Sent by Server

- [x] [at](Game/Server/AtPacketBase.cs)
- [x] [bn](Game/Server/BnPacket.cs)
- [x] [c_info](Game/Server/CInfoPacketBase.cs)
- [x] [c_map](Game/Server/CMapPacketBase.cs)
- [x] [c_mode](Game/Server/CModePacketBase.cs)
- [x] [cond](Game/Server/CondPacketBase.cs)
- [x] [fd](Game/Server/FdPacket.cs)
- [x] [gp](Game/Server/GpPacket.cs)
- [x] [in](Game/Server/InPacketBase.cs)
- [x] [in_alive_subpacket](Game/Server/InAliveSubPacketBase.cs)
- [x] [in_character_subpacket](Game/Server/InCharacterSubPacketBase.cs)
- [x] [in_item_subpacket](Game/Server/InItemSubPacketBase.cs)
- [x] [in_monster_subpacket](Game/Server/InMonsterSubPacket.cs)
- [x] [in_non_player_subpacket](Game/Server/InNonPlayerSubPacketBase.cs)
- [x] [in_npc_subpacket](Game/Server/InNpcSubPacket.cs)
- [x] [in_ownable_subpacket](Game/Server/InOwnableSubPacket.cs)
- [x] [inv_main_subpacket](Game/Server/InPacketMainItem.cs)
- [x] [ivn](Game/Server/IvnPacket.cs)
- [x] [ivn_wear_subpacket](Game/Server/InvPacketWearItem.cs)
- [x] [lev](Game/Server/LevPacket.cs)
- [x] [levelup](Game/Server/LevelUpPacket.cs)
- [x] [mv](Game/Server/MvPacket.cs)
- [x] [out](Game/Server/OutPacketBase.cs)
- [x] [remove](Game/Server/RemovePacket.cs)
- [x] [say](Game/Server/SayPacket.cs)
- [x] [st](Game/Server/StPacket.cs)
- [x] [stat](Game/Server/StatPacket.cs)
- [x] [tit](Game/Server/TitPacket.cs)
- [x] [u_i](Game/Server/UiPacket.cs)

