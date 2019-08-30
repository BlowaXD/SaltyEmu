
# SaltyEmu

[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](http://forthebadge.com)[![forthebadge](https://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com)
[![Discord](https://discordapp.com/api/guilds/556349908890812418/widget.png?style=banner2)](https://discord.gg/7sTFU8d)

## Description :

SaltyEmu is a software designed as a server emulator for the MMORPG Nostale.
It's based on [ChickenAPI](https://github.com/BlowaXD/ChickenAPI) and has an event driven architecture.
SaltyEmu is an opensource project, made for fun and skill improvements, it will stay as is.

**This project is no more maintained, I'm working on a private project that runs [NosWings](https://noswings.com)**

**Do not hesitate to study its code/architecture for learning purposes, there are a lot of good and bad designs**

**The new project (WingsEmu) is an upgrade of SaltyEmu with much more "entreprise-grade" architecture choices thanks to my recent studies**

**Some parts of WingsEmu might be put opensource**

What I wanted to do :
- Distributed Computing & Microservices oriented architecture
- Stateless softwares
- Fully tested project
- Really powerful project (modular & pluggable)
- Community Driven project

What have been done so far :
- Distributed Computing can be done, on most of the things except game events (it needs to modify the network wrapper for that), simply because I didn't want to waste my time on it
- Almost not tested project, except RelationService, the reason is that I've been changing quite a lot of things and this project was not intent to be that big right now.
- Powerful project (We are using a lot of modern technologies, always keeping an eye on new  and reviewing our old work)

What have failed so far :
- Community Driven Project
  - That project is so big that it can not be written alone, but nostale scene is so cancerous that people do not understand why opensource is a good way to complete our achievements
 


## Requirements
> [.NET Core SDK 2.1+](https://www.microsoft.com/net/download)

> [Docker](https://www.docker.com/community-edition)
 

## Technologies
### Already using / supporting
- Git
- Docker
- C#
- MSSQL (should be dropped soon)
- MongoDB (should be moved to a lighter DB soon)
- Redis
- Gitlab CI/CD
- RabbitMQ (AQMP) / EMQX (MQTT) / Emitter (MQTT)

### Incoming for future usage :
- [Protobuf](https://github.com/protocolbuffers/protobuf) (Marshaling tool)
- [gRPC](https://grpc.io/) (RPC Framework)
- [Envoy](https://www.envoyproxy.io/) (Proxy & HTTP Load Balancer)
- [Kong](https://github.com/Kong/kong) or [Istio](https://github.com/istio/istio) (some kind of really powerful API Gateway for our microservices)
- [Vault](https://www.vaultproject.io/) (Credentials store)
- [Consul](https://www.consul.io/) (Service Discovery)
- [VueJS](https://vuejs.org/) (Potential SaltyEmu Admin interface)
- [Grafana](https://github.com/grafana/grafana)

## Content

### [Getting Started](docs/started.md)
### [F.A.Q](docs/faq.md)
### [Plugins](docs/plugins.md)
### [Features](docs/features.md)
### [Architectural Documentation](docs/architecture.md)

## Big features you might be interested in :
- Command Framework (Easily plug your commands or help creating some new ones !)
- Plugin Framework (However, it can be improved with IoC)
- RPC Framework (Transport layer through MQTT, RabbitMQ, HTTP, whatever, it's abstracted enough for that)
- Advanced IoC Usage (It still needs to be improved)
- A lot of 3rd party wrappers (abstracted)
- A lot of **generic** & **abstracted** reusable code (It's not because you are writing a project that you can not let a piece of code living its own life)
- A lot of data about Nostale (mostly seeable in ChickenAPI)
- A lot of reworks, rewrites (you can see that through commits)
- Cool & modern technologies

## Credits
### Authors : 
- Blowa

### Contributors :
- [Kraken](https://github.com/Kraken01) (Big thanks for all the time you took to implement Nostale boring features...)
- Zanou (Big thanks for all the time you took to implement Nostale boring features...)
- [Kiritsu](https://github.com/Kiritsu/) 
- GodnessCookie
- [Quarry](https://github.com/imquarry)
- Clavs
- [SylEze](https://github.com/SylEze) (For helping me when I was questioning myself about some architectural choices)

### Special Thanks :
- Z0ltar (Thanks for the algorithms you shared)
- [Quahu](https://github.com/Quahu/) for your amazing work on [Qmmands] that we are using !
- [Eastrall](https://github.com/Eastrall) (Thanks for your hints, discussions and feedbacks)
- [DarkyZ](https://github.com/ImNotAVirus) (Thanks for your feedbacks since the beginning of the project, you helped me a lot)
- [Elendan](https://github.com/Elendan) For the name of the emulator (Nos# then WingsEmu then AsyncSaltyWaatOnMicroService then finally SaltyEmu)
- All people who supported my work, I never asked for donations or anything for SaltyEmu, just fun and some data & knowledge related help 

### Another type of thanks :
- 0Lucifer0 (SaltyEmu wouldn't be that powerful if I didn't see how shitty are OpenNos & OpenNosCore)
- Ciapa (I spent mhhhhhhhhhhhh 3 days reading your shitty BattlePacketHandler that killed my mind)
- Cryless (File.AppendAllText("Thanks for disrespecting people's work and being a trash");)

### Backlinks :
[ChickenAPI](https://github.com/BlowaXD/ChickenAPI) (Nostale Private Server API)
[noswings.com](https://noswings.com) (Maybe soon will be running SaltyEmu)
[elitepvpers presentation thread](https://www.elitepvpers.com/forum/nostale/4544355-opensource-nostale-private-server-emulator-saltyemu-based-chickenapi.html)
