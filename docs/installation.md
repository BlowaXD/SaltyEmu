

# SaltyEmu - Installation

## Enable Session Service
You will need to start a Redis 
`docker run -p 6379:6379 --name saltyemu-session -d redis:latest`


## Enable Database
You also need to start a MSSQL Server instance
 
`docker run -p 1433:1433 -e ACCEPT_EULA=Y -e SA_PASSWORD=strong_pass2018 --name saltyemu-database -d microsoft/mssql-server-linux:latest`

Now create your Database, by default called `saltynos` in configs

## Parse Datas 

We designed a tool which is able to parse Nostale's .dat files to fill your database

 
`dotnet src/SaltyEmu.Toolkit parse all -i {PathToParsingDirectory}`

### Run Login

`dotnet bin/Debug/Login.dll`

### Run World 

`dotnet bin/Release/World.dll`
