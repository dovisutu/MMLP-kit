FROM mcr.microsoft.com/dotnet/runtime:8.0-nanoserver-1809 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/uploader/Uploader.csproj", "src/uploader/"]
RUN dotnet restore "src\uploader\Uploader.csproj"
COPY . .
WORKDIR "/src/src/uploader"
RUN dotnet build "Uploader.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Uploader.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENTRYPOINT ["dotnet", "Uploader.dll"]
