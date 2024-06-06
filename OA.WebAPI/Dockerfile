#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OA.WebAPI/OA.WebAPI.csproj", "OA.WebAPI/"]
COPY ["Core/OA.Application/OA.Application.csproj", "Core/OA.Application/"]
COPY ["Infrastructure/OA.Infrastructure/OA.Infrastructure.csproj", "Infrastructure/OA.Infrastructure/"]
COPY ["Infrastructure/OA.Persistence/OA.Persistence.csproj", "Infrastructure/OA.Persistence/"]
COPY ["Core/OA.Domain/OA.Domain.csproj", "Core/OA.Domain/"]
COPY ["Presentation/OA.Presentation/OA.Presentation.csproj", "Presentation/OA.Presentation/"]
COPY ["WorkerService/OA.LogService.Worker/OA.LogService.Worker.csproj", "WorkerService/OA.LogService.Worker/"]
RUN dotnet restore "./OA.WebAPI/OA.WebAPI.csproj"
COPY . .
WORKDIR "/src/OA.WebAPI"
RUN dotnet build "./OA.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OA.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OA.WebAPI.dll"]