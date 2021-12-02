#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SandCastle-BackEnd/SandCastle-BackEnd.csproj", "SandCastle-BackEnd/"]
RUN dotnet restore "SandCastle-BackEnd/SandCastle-BackEnd.csproj"
COPY . .
WORKDIR "/src/SandCastle-BackEnd"
RUN dotnet build "SandCastle-BackEnd.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SandCastle-BackEnd.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SandCastle-BackEnd.dll"]