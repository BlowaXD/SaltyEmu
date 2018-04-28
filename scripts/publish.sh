# remove actual dist directory
rm -r dist

# release both windows & linux version
cd src/NosSharp.Login
dotnet publish --self-contained -r win-x64 -c release -o ../../dist/Login/windows
dotnet publish --self-contained -r linux-x64 -c release -o ../../dist/Login/linux
cd ..