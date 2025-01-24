FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev
WORKDIR /app/Stopify

RUN dotnet tool install --global dotnet-ef

COPY Stopify/*.csproj ./

RUN dotnet restore --no-cache
RUN dotnet tool restore

ENV PATH="$PATH:/root/.dotnet/tools"

COPY . ./

EXPOSE 80 443 9229

ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV DOTNET_WATCH_RESTART_ON_RUDE_EDIT=true

ENTRYPOINT ["sh", "-c", "dotnet watch --project Stopify run --no-launch-profile --non-interactive --urls http://0.0.0.0:8080"]
