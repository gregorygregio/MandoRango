#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MandoRango.Courier.Position.WorkerService/MandoRango.Courier.Position.WorkerService.csproj", "MandoRango.Courier.Position.WorkerService/"]
RUN dotnet restore "MandoRango.Courier.Position.WorkerService/MandoRango.Courier.Position.WorkerService.csproj"
COPY . .
WORKDIR "/src/MandoRango.Courier.Position.WorkerService"
RUN dotnet build "MandoRango.Courier.Position.WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MandoRango.Courier.Position.WorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MandoRango.Courier.Position.WorkerService.dll"]