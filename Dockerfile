# Set the major version of dotnet
ARG DOTNET_VERSION=8.0

# ==============================================
# .NET SDK: Build
# ==============================================
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-azurelinux3.0 AS build
WORKDIR /build
ARG CI
ENV CI=${CI}

# Mount GitHub Token as a Docker secret so that NuGet Feed can be accessed
RUN --mount=type=secret,id=github_token dotnet nuget add source --username USERNAME --password $(cat /run/secrets/github_token) --store-password-in-clear-text --name github "https://nuget.pkg.github.com/DFE-Digital/index.json"

# Copy the application code
COPY ./src/ ./src/
COPY Directory.Build.props ./
COPY Dfe.CaseAggregationService.sln ./

# Build and publish the dotnet solution
RUN dotnet restore Dfe.CaseAggregationService.sln && \
    dotnet build ./src/Dfe.CaseAggregationService.Api --no-restore -c Release && \
    dotnet publish ./src/Dfe.CaseAggregationService.Api --no-build -o /app

# ==============================================
# .NET Runtime: Publish
# ==============================================
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-azurelinux3.0 AS final
WORKDIR /app
LABEL org.opencontainers.image.source="https://github.com/DFE-Digital/case-aggregation-api"
LABEL org.opencontainers.image.description="Dfe.CaseAggregationService"

COPY --from=build /app /app
COPY ./script/docker-entrypoint.sh /app/docker-entrypoint.sh
RUN chmod +x ./docker-entrypoint.sh

USER $APP_UID
