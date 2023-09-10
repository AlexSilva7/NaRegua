#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Crie a pasta /app/logs dentro do container
RUN mkdir -p /app/logs

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NaRegua_Api.csproj", "NaRegua_Api/"]
RUN dotnet restore "NaRegua_Api/NaRegua_Api.csproj"

WORKDIR "/src/NaRegua_Api/"
COPY . .
RUN dotnet build "NaRegua_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NaRegua_Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NaRegua_Api.dll"]