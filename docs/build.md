# Build

If you want to build everything, there is are scripts called `build_all_Release` or `build_all_Debug`, `.cmd` or `.sh` depending on your platform

## Toolkit
releaseMode : 
 * `Debug`
 * `Release`

Command : `dotnet build --configuration {releaseMode} {SaltyEmu}/src/Toolkit/`

Scripts : 
* Windows : `scripts/build/build_Toolkit_{releaseMode}.cmd`
* Linux (sh) : `scripts/build/build_Toolkit_{releaseMode}.sh`

outputDirectory : 
`dist/{releaseMode}/Toolkit/`

## Login
releaseMode : 
 * `Debug`
 * `Release`

outputDirectory : `dist/{releaseMode}/Login/`

Command : `dotnet build --configuration {releaseMode} src/Login/`

Scripts : 
* Windows : `scripts/build/build_Login_{releaseMode}.cmd`
* Linux (sh) : `scripts/build/build_Login_{releaseMode}.sh`

outputFile : `Login.dll`

## World 
releaseMode : 
 * `Debug`
 * `Release`


Command : `dotnet build --configuration {releaseMode} {SaltyEmu}/src/World/`

Scripts : 
* Windows : `scripts/build/build_World_{releaseMode}.cmd`
* Linux (sh) : `scripts/build/build_World_{releaseMode}.sh`

outputDirectory : 
`dist/{releaseMode}/World/`

outputFile : `World.dll`
