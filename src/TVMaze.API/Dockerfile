#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TVMaze.API/TVMaze.API.csproj", "TVMaze.API/"]
COPY ["TVMaze.Application/TVMaze.Application.csproj", "TVMaze.Application/"]
COPY ["TvMaze.Infrastructure/TvMaze.Infrastructure.csproj", "TvMaze.Infrastructure/"]
COPY ["TVMaze.Core/TVMaze.Core.csproj", "TVMaze.Core/"]
RUN dotnet restore "TVMaze.API/TVMaze.API.csproj"
COPY . .
WORKDIR "/src/TVMaze.API"
RUN dotnet build "TVMaze.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TVMaze.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TVMaze.API.dll"]
