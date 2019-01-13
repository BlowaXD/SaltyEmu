FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/services/NosWings.UserService/NosWings.UserService.csproj", "src/services/NosWings.UserService/"]
RUN dotnet restore "src/services/NosWings.UserService/NosWings.UserService.csproj"
COPY . .
WORKDIR "/src/src/services/NosWings.UserService"
RUN dotnet build "NosWings.UserService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NosWings.UserService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV JWT_TOKEN_SECRET="secret" \
    HOSTNAME="api.dev.website"
ENTRYPOINT ["dotnet", "NosWings.UserService.dll"]