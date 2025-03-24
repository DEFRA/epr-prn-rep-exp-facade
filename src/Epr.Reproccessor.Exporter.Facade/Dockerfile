FROM defradigital/dotnetcore-development:dotnet8.0 AS build-env
USER root

# Expose the app on a defined port, configurable via a build argument
ARG PORT=3000
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}

# Copy files
COPY Epr.Reproccessor.Exporter.Facade.Api/. ./Epr.Reproccessor.Exporter.Facade.Api/.
COPY Epr.Reproccessor.Exporter.Facade.App/. ./Epr.Reproccessor.Exporter.Facade.App/.
COPY NuGet.Config ./
COPY Directory.Build.props ./

# Use build argument to insert PAT into Nuget file
ARG PAT=localhost
RUN sed -i "s|</configuration>|<packageSourceCredentials><defra-epr-common><add key=\"Username\" value=\"PAT\" /><add key=\"ClearTextPassword\" value=\"${PAT}\" /></defra-epr-common></packageSourceCredentials></configuration>|" NuGet.Config

# Restore as distinct layers
RUN dotnet restore "Epr.Reproccessor.Exporter.Facade.Api/Epr.Reproccessor.Exporter.Facade.Api.csproj" --configfile "NuGet.Config"

# Build and publish a release
WORKDIR /home/dotnet/Epr.Reproccessor.Exporter.Facade.Api
RUN dotnet publish -c Release -o out

# Build runtime image
FROM defradigital/dotnetcore:dotnet8.0

# Switch to non-root user
USER dotnet

COPY --from=build-env /home/dotnet/Epr.Reproccessor.Exporter.Facade.Api/out .

# Expose the app on a defined port, configurable via a build argument
ARG PORT=3000
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}

CMD dotnet Epr.Reproccessor.Exporter.Facade.Api.dll