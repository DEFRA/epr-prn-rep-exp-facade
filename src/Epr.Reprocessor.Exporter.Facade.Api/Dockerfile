FROM defradigital/dotnetcore-development:dotnet8.0 AS base
USER root

# Expose the app on a defined port, configurable via a build argument
ARG PORT=3000
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}

RUN apk update && apk --no-cache add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0

FROM defradigital/dotnetcore-development:dotnet8.0 AS build
USER root
WORKDIR /src
COPY ["Epr.Reprocessor.Exporter.Facade.Api/Epr.Reprocessor.Exporter.Facade.Api.csproj", "Epr.Reprocessor.Exporter.Facade.Api/"]
COPY ["Epr.Reprocessor.Exporter.Facade.App/Epr.Reprocessor.Exporter.Facade.App.csproj", "Epr.Reprocessor.Exporter.Facade.App/"]
COPY ["NuGet.Config",""]

# Use Build Argument to insert PAT into Nuget file
ARG PAT=localhost
RUN sed -i "s|</configuration>|<packageSourceCredentials><epr-packaging-common><add key=\"Username\" value=\"PAT\" /><add key=\"ClearTextPassword\" value=\"${PAT}\" /></epr-packaging-common></packageSourceCredentials></configuration>|" NuGet.Config

# Restore as distinct layers
COPY Directory.Build.props ./
RUN dotnet restore "Epr.Reprocessor.Exporter.Facade.Api/Epr.Reprocessor.Exporter.Facade.Api.csproj" --configfile "./NuGet.Config"

# Build and publish a release
COPY Epr.Reprocessor.Exporter.Facade.Api/. ./Epr.Reprocessor.Exporter.Facade.Api/.
COPY Epr.Reprocessor.Exporter.Facade.App/. ./Epr.Reprocessor.Exporter.Facade.App/.
WORKDIR "/src/Epr.Reprocessor.Exporter.Facade.Api"
RUN dotnet build "Epr.Reprocessor.Exporter.Facade.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Epr.Reprocessor.Exporter.Facade.Api.csproj" -c Release -o /app/publish

FROM base AS final

# Switch to non-root user
USER dotnet

WORKDIR /app
COPY --from=publish /app/publish .
USER dotnet
ENTRYPOINT ["dotnet", "Epr.Reprocessor.Exporter.Facade.Api.dll"]
