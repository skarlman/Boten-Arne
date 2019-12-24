docker build -t botenarne:v3 -f .\BotenArne\Dockerfile .


dotnet publish -c Release -r win-x64 --output ./publish/BigBrother .\BigBrother\BigBrother.csproj

dotnet publish -c Release -r win-x64 --output ./publish/TwitchHandler .\TwitchHandler\TwitchHandler.csproj

