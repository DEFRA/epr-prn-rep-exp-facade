FROM defradigital/dotnetcore-development:dotnet8.0 AS build-env
USER root
 
# Expose the app on a defined port, configurable via a build argument
ARG PORT=3000
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}
WORKDIR /src

     FROM defradigital/dotnetcore-development:dotnet8.0 AS build
     USER root
     WORKDIR /src

    COPY ["Epr.Reprocessor.Exporter.Facade.Api/Epr.Reprocessor.Exporter.Facade.Api.csproj", "Epr.Reprocessor.Exporter.Facade.Api/"]
    COPY ["Epr.Reprocessor.Exporter.Facade.App/Epr.Reprocessor.Exporter.Facade.App.csproj", "Epr.Reprocessor.Exporter.Facade.App/"]
    COPY ["NuGet.Config",""]
    
COPY Epr.Reprocessor.Exporter.Facade.Api/. ./Epr.Reprocessor.Exporter.Facade.Api/.
COPY Epr.Reprocessor.Exporter.Facade.App/. ./Epr.Reprocessor.Exporter.Facade.App/.
COPY NuGet.Config ./

# Use Build Argument to insert PAT into Nuget file
ARG PAT=localhost
RUN sed -i "s|</configuration>|<packageSourceCredentials><epr-packaging-common><add key=\"Username\" value=\"PAT\" /><add key=\"ClearTextPassword\" value=\"${PAT}\" /></epr-packaging-common></packageSourceCredentials></configuration>|" NuGet.Config
  
# Restore as distinct layers
COPY Directory.Build.props ./
RUN dotnet restore "Epr.Reprocessor.Exporter.Facade.Api/Epr.Reprocessor.Exporter.Facade.Api.csproj" --configfile "./NuGet.Config"

# Build and publish a release
WORKDIR "/src/Epr.Reprocessor.Exporter.Facade.Api"
RUN dotnet build "Epr.Reprocessor.Exporter.Facade.Api.csproj" -c Release -o /app/build
 
FROM build-env AS publish
RUN dotnet publish -c Release -o out

# Build runtime image
FROM defradigital/dotnetcore:dotnet8.0
# Switch to non-root user
USER dotnet
COPY --from=build-env /src/Epr.Reprocessor.Exporter.Facade.Api/out .

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0
FROM base AS final 

WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Epr.Reprocessor.Exporter.Facade.Api.dll"]
