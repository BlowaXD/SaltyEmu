# Getting Started

## Download requirements

* [Visual Studio](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&rel=15)
* [.NET Core SDK 2.1](https://download.visualstudio.microsoft.com/download/pr/9b60a25e-5b31-4550-aae1-72516c1067f6/52e8387487fecef06266a7a19c97ddee/dotnet-sdk-2.1.500-win-gs-x64.exe)
* [Docker For Windows](https://download.docker.com/win/stable/Docker%20for%20Windows%20Installer.exe)
* [Kitematic](https://download.docker.com/kitematic/Kitematic-Windows.zip)
* [Git](http://rogerdudler.github.io/git-guide/)
    * [Git For Windows - Command Line](https://git-scm.com/download/win)
    * [GitKraken - Graphical Interface](https://www.gitkraken.com/download)

## Build the solution

Open the solution and build it

### /!\ ACHTUNG /!\
### You need to build the solution to be able to find the executables

#### How to build the solution :
* **Dotnet CLI** (In your terminal)
    * `dotnet build` (execute it in the same directory as SaltyEmu.sln)
* **Visual Studio**
    * Ctrl + Shift + B
    * Right click on solution "Build all projects"

Executable paths : 
* World : `dist/Debug/World/`
* Login : `dist/Debug/Login/`
* Toolkit : `dist/Debug/Toolkit/`
* Microservices (under development)
    * GiftService : `dist/Debug/SaltyEmu.GiftService/` // not finished yet
    * FamilyService : `dist/Debug/SaltyEmu.FamilyService/` // not finished yet
    * FriendService : `dist/Debug/SaltyEmu.FriendService/` // not finished yet
    * NosBazaarService : `dist/Debug/SaltyEmu.NosBazaarService/` // not finished yet


## Deploy the necessary softwares

Pull the given images : 

* [MSSQL](https://hub.docker.com/r/microsoft/mssql-server-linux/)
* [Redis](https://hub.docker.com/_/redis/)
* [MQTT Broker](https://hub.docker.com/r/emqx/emqx/)

Use given scripts


## Create a MSSQL Database

Use [SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-2017) or [Blowa's Docker](https://github.com/BlowaXD/docker-mssql-createdb-tool)


## Fill your database data with the toolkit

Pull the following [Parsing repository](https://github.com/BlowaXD/nostale-parsing)

### /!\ ACHTUNG ! /!\
#### RUN THAT COMMAND IN YOUR TERMINAL (CMD / POWERSHELL / SHELL / BASH)
#### PARSING DIRECTORY HAS TO BE CHANGED BY THE DIRECTORY WHERE YOU JUST PULLED PARSING
`dotnet dist/Debug/Toolkit/Toolkit.dll parse all -i {parsingDirectory}`

### /!\ ACHTUNG ! /!\
#### AT THE FIRST RUN YOU WILL HAVE TO CONFIGURE YOU DATABASE ACCESS RIGHTLY
`dist/Debug/Toolkit/plugins/config/DatabasePlugin/conf.json`

## Configure DatabasePlugin

### /!\ ACHTUNG ! /!\
#### AT THE FIRST RUN YOU WILL HAVE TO CONFIGURE YOU DATABASE ACCESS RIGHTLY
`dist/Debug/{Login|World|Toolkit}/plugins/config/DatabasePlugin/conf.json`

## Enjoy

Have fun !