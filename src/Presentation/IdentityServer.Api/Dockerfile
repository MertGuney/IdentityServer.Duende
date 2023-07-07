#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Presentation/IdentityServer.Api/IdentityServer.Api.csproj", "src/Presentation/IdentityServer.Api/"]
COPY ["src/Core/IdentityServer.Application/IdentityServer.Application.csproj", "src/Core/IdentityServer.Application/"]
COPY ["src/Shared/IdentityServer.Shared/IdentityServer.Shared.csproj", "src/Shared/IdentityServer.Shared/"]
COPY ["src/Core/IdentityServer.Domain/IdentityServer.Domain.csproj", "src/Core/IdentityServer.Domain/"]
COPY ["src/Infrastructure/IdentityServer.Infrastructure/IdentityServer.Infrastructure.csproj", "src/Infrastructure/IdentityServer.Infrastructure/"]
COPY ["src/Infrastructure/IdentityServer.Persistence/IdentityServer.Persistence.csproj", "src/Infrastructure/IdentityServer.Persistence/"]
RUN dotnet restore "src/Presentation/IdentityServer.Api/IdentityServer.Api.csproj"
COPY . .
WORKDIR "/src/src/Presentation/IdentityServer.Api"
RUN dotnet build "IdentityServer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityServer.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServer.Api.dll"]