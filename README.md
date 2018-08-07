# SaltyEmu World Server

[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](http://forthebadge.com)
 
[![forthebadge](https://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com)


## Requirements :
[.NET Core SDK 2.1+](https://www.microsoft.com/net/download)
 
[Docker](https://www.docker.com/community-edition)
 
[SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-2017)
 

## Installation

### Enable Session Service
You will need to start a Redis 
 
`docker run -p 6379:6379 --name saltyemu-session -d redis:latest`


### Enable Database
You also need to start a MSSQL Server instance
 
`docker run -p 1433:1433 -e ACCEPT_EULA=Y -e SA_PASSWORD=strong_pass2018 --name saltyemu-database -d microsoft/mssql-server-linux:latest`

Now create your Database, by default called `saltynos` in configs

### Parse Datas

Go in SaltyEmu.Toolkit
 
`dotnet src/SaltyEmu.Toolkit parse all -i {PathToParsingDirectory}`

### Run Login

`dotnet bin/Debug/Login.dll`

### Run World 

`dotnet bin/Debug/World.dll`


## Credits
Authors : 
- Blowa
- Kraken


Contributors :
- SylEze
- Kiritsu