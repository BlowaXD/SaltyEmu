# Build

### Build Toolkit
releaseMode : 
 * `Debug`
 * `Release`

Command : `dotnet build --configuration {releaseMode} {SaltyEmu}/src/Toolkit/`
outputDirectory : 
`dist/{releaseMode}/Toolkit/`

### Build Login
releaseMode : 
 * `Debug`
 * `Release`

outputDirectory : 
`dist/{releaseMode}/Login/`
Command : `dotnet build --configuration {releaseMode} {SaltyEmu}/src/Login/`

outputFile : `Login.dll`

### Build World 
releaseMode : 
 * `Debug`
 * `Release`


Command : `dotnet build --configuration {releaseMode} {SaltyEmu}/src/World/`

outputDirectory : 
`dist/{releaseMode}/World/`

outputFile : `World.dll`
