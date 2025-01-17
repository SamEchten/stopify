FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev
WORKDIR /app

RUN dotnet tool install --global dotnet-ef

COPY Stopify/*.csproj ./

RUN dotnet restore
RUN dotnet tool restore

ENV PATH="$PATH:/root/.dotnet/tools"

COPY . ./

EXPOSE 80 443 9229

ENV ASPNETCORE_ENVIRONMENT=Development

ENV DOTNET_USE_POLLING_FILE_WATCHER=1

ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile", "--urls", "http://0.0.0.0:80"]
