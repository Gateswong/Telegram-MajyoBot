FROM mcr.microsoft.com/dotnet/core/runtime:2.1-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["MajyoBot/MajyoBot.csproj", "MajyoBot/"]
RUN dotnet restore "MajyoBot/MajyoBot.csproj"
COPY . .
WORKDIR "/src/MajyoBot"
RUN dotnet build "MajyoBot.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MajyoBot.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MajyoBot.dll"]