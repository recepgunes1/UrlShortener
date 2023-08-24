#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Shortener/WebAPI/Shortener.WebAPI.csproj", "src/Shortener/WebAPI/"]
COPY ["src/Shortener/Infrastructure/Shortener.Infrastructure.csproj", "src/Shortener/Infrastructure/"]
COPY ["src/Utils/Shared/Shared.csproj", "src/Utils/Shared/"]
RUN dotnet restore "src/Shortener/WebAPI/Shortener.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Shortener/WebAPI"
RUN dotnet build "Shortener.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shortener.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shortener.WebAPI.dll"]