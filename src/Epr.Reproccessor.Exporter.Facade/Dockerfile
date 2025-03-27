FROM defradigital/dotnetcore-development:dotnet8.0 AS base
USER root
 
# Expose the app on a defined port, configurable via a build argument
ARG PORT=3000
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}

FROM defradigital/dotnetcore-development:dotnet8.0 AS build
USER root
WORKDIR /src
COPY ["Epr.Reproccessor.Exporter.Facade/Epr.Reproccessor.Exporter.Facade.Api.csproj", "Epr.Reproccessor.Exporter.Facade/"]
COPY ["Epr.Reproccessor.Exporter.Facade.App/Epr.Reproccessor.Exporter.Facade.App.csproj", "Epr.Reproccessor.Exporter.Facade.App/"]
COPY ["NuGet.Config",""]
 
# Use Build Argument to insert PAT into Nuget file
ARG PAT=localhost
RUN sed -i "s|</configuration>|<packageSourceCredentials><epr-packaging-common><add key=\"Username\" value=\"PAT\" /><add key=\"ClearTextPassword\" value=\"${PAT}\" /></epr-packaging-common></packageSourceCredentials></configuration>|" NuGet.Config
 
# Restore as distinct layers
COPY Directory.Build.props ./
RUN dotnet restore "Epr.Reproccessor.Exporter.Facade/Epr.Reproccessor.Exporter.Facade.Api.csproj" --configfile "./NuGet.Config"
 
# Install NPM
USER root
RUN apk update && apk add --no-cache npm
 
ENV NODE_EXTRA_CA_CERTS=/usr/local/share/ca-certificates/internal-ca.crt
 
# Build and publish a release
COPY Epr.Reproccessor.Exporter.Facade/. ./Epr.Reproccessor.Exporter.Facade/.
COPY Epr.Reproccessor.Exporter.Facade.App/. ./Epr.Reproccessor.Exporter.Facade.App/.
WORKDIR "/src/Epr.Reproccessor.Exporter.Facade"
RUN dotnet build "Epr.Reproccessor.Exporter.Facade.Api.csproj" -c Release -o /app/build
 
FROM build AS publish
RUN dotnet publish "Epr.Reproccessor.Exporter.Facade.Api.csproj" -c Release -o /app/publish

RUN dotnet publish -c Release -o out

# Build runtime image
FROM defradigital/dotnetcore:dotnet8.0
COPY --from=build-env /home/dotnet/Epr.Reproccessor.Exporter.Facade.Api/out .

# Install tzdata
USER root
RUN apk add --no-cache tzdata
 
# Add internationalisation support
USER root
RUN apk add --no-cache icu-libs icu-data-full libintl
 
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0
FROM base AS final
 
USER dotnet
 
WORKDIR /app

COPY --from=publish /app/publish .
USER dotnet
ENTRYPOINT ["dotnet", "Epr.Reproccessor.Exporter.Facade.Api.dll"]