#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Discord/Discord.csproj", "Discord/"]
COPY ["InitiativeBot/InitiativeBot.csproj", "InitiativeBot/"]
RUN dotnet restore "Discord/Discord.csproj"
COPY . .
WORKDIR "/src/Discord"
RUN dotnet build "Discord.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Discord.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discord.dll"]