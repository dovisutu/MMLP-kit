FROM mcr.microsoft.com/dotnet/runtime:8.0-nanoserver-1809 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/packer/Packer.csproj", "src/packer/"]
RUN dotnet restore "src\packer\Packer.csproj"
COPY . .
WORKDIR "/src/src/packer"
RUN dotnet build "Packer.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Packer.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENTRYPOINT ["dotnet", "Packer.dll"]
