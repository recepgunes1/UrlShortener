#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Buffer/WebAPI/Buffer.WebAPI.csproj", "src/Buffer/WebAPI/"]
COPY ["src/Buffer/Infrastructure/Buffer.Infrastructure.csproj", "src/Buffer/Infrastructure/"]
COPY ["src/Utils/Shared/Shared.csproj", "src/Utils/Shared/"]
RUN dotnet restore "src/Buffer/WebAPI/Buffer.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Buffer/WebAPI"
RUN dotnet build "Buffer.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Buffer.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Buffer.WebAPI.dll"]