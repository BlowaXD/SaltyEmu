
# SaltyEmu

[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](http://forthebadge.com)[![forthebadge](https://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com)
[![Discord](https://discordapp.com/api/guilds/512650034257592336/widget.png?style=banner2)](https://discord.gg/8qAd9px)

## Description :

SaltyEmu is a software designed as a server emulator for the MMORPG Nostale.
It's based on [ChickenAPI](https://github.com/BlowaXD/ChickenAPI) and has an event driven architecture.
SaltyEmu is an opensource project, made for fun and skill improvements, it will stay as is.


What I wanted to do :
- Distributed Computing & Microservices oriented architecture
- Stateless softwares
- Fully tested project
- Really powerful project (modular & pluggable)
- Community Driven project

What have been done so far :
- Distributed Computing can be done, on most of the things except game events (it needs to modify the network wrapper for that), simply because I didn't want to waste my time on it
- Almost not tested project, except RelationService, the reason is that I've been changing quite a lot of things and this project was not intent to be that big right now.
- Powerful project (We are using latest technologies, always keeping an eye and reviewing our old work)


What have failed so far :
- Community Driven Project
  - That project is so big that it can not be written alone, but nostale scene is so cancerous that people can not understand why opensource is a good way to complete our achievements.
 


## Requirements :
> [.NET Core SDK 2.1+](https://www.microsoft.com/net/download)

> [Docker](https://www.docker.com/community-edition)
 

## Content

### [Getting Started](docs/started.md)
### [F.A.Q](docs/faq.md)
### [Plugins](docs/plugins.md)
### [Features](docs/features.md)

## Big features you might be interested in :
- Command Framework (Easily plug your commands)
- Plugin Framework (However, it can be improved with IoC)
- RPC Framework (Transport layer through MQTT, RabbitMQ, HTTP, whatever, it's abstracted enough for that)
- Advanced IoC Usage
- A lot of 3rd party wrappers (abstracted)
- A lot of **generic** & **abstracted** reusable code (It's not because you are writing a project that you can not let a piece of code living its own life)
- A lot of data about Nostale
- A lot of reworks, rewrites (you can see that through commits)

## Credits
### Authors : 
- Blowa

### Contributors :
- Kraken (Big thanks for all the time you took to implement Nostale boring features...)
- Zanou (Big thanks for all the time you took to implement Nostale boring features...)
- Kiritsu 
- GodnessCookie
- Quarry
- Clavs
- SylEze

### Special Thanks :
- Z0ltar (Thanks for the algorithms you shared)
- [Quahu](https://github.com/Quahu/) for your amazing work on [Qmmands] that we are using !
- [Eastrall](https://github.com/Eastrall) (Thanks for your hints, discussions and feedbacks)
- [DarkyZ](https://github.com/ImNotAVirus) (Thanks for your feedbacks since the beginning of the project, you helped me a lot)
- All people who supported my work, I never asked for donations or anything for SaltyEmu, just fun and some data & knowledge related help 

### Another type of thanks :
- 0Lucifer0 (SaltyEmu wouldn't be that powerful if I didn't see how shitty are OpenNos & OpenNosCore)
- Ciapa (I spent mhhhhhhhhhhhh 3 days reading your shitty BattlePacketHandler that killed my mind)
- Cryless (File.AppendAllText("Thanks for disrespecting people's work and being a trash");)
