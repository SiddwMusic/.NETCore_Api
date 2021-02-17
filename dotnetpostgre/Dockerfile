FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 7687
EXPOSE 7474

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY dotnetpostgre.csproj dotnetpostgre/

#COPY appsettings.json ./
#COPY appsettings.json dotnetpostgre/
RUN dotnet restore dotnetpostgre/dotnetpostgre.csproj
COPY . dotnetpostgre/
WORKDIR /src/dotnetpostgre
RUN dotnet build dotnetpostgre.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish dotnetpostgre.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "dotnetpostgre.dll"]


