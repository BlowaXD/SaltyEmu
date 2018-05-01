# remove actual dist directory
rm -r dist

# release both windows & linux version
cd src/NosSharp.World
dotnet publish -r linux-x64 -c release -o ../../dist/World/linux
cd ..