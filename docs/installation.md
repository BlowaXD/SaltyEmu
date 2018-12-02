

# SaltyEmu - Installation

## Enable Session Service
You will need to start a Redis

### Through Docker
Docker way to start a redis : 

`docker run -p 6379:6379 --name saltyemu-session -d redis:latest`


## Enable Database
You also need to start a MSSQL Server instance
 
### Through Docker
Docker way to start a MSSQL

`docker run -p 1433:1433 -e ACCEPT_EULA=Y -e SA_PASSWORD=strong_pass2018 --name saltyemu-database -d microsoft/mssql-server-linux:latest`

Now create your Database, by default called `saltynos` in configs

## Parse Datas 

We designed a tool which is able to parse Nostale's .dat files to fill your database.
 
`dotnet dist/Debug/Toolkit/Toolkit.dll parse all -i {PathToParsingDirectory}`
> /!\ AT THE FIRST RUN YOU WILL HAVE TO CONFIGURE YOU DATABASE ACCESS RIGHTLY
>
> `dist/Debug/Toolkit/plugins/configs/DatabasePlugin/conf.json`

## Login Server


### Through dotnet
`dotnet dist/Debug/Login/Login.dll`
> /!\ AT THE FIRST RUN YOU WILL HAVE TO CONFIGURE YOU DATABASE ACCESS RIGHTLY
>
> `dist/Debug/Login/plugins/configs/DatabasePlugin/conf.json`

## World Server


### Through dotnet
`dotnet dist/Debug/World/World.dll`
> /!\ AT THE FIRST RUN YOU WILL HAVE TO CONFIGURE YOU DATABASE ACCESS RIGHTLY
>
> `dist/Debug/World/plugins/configs/DatabasePlugin/conf.json`
