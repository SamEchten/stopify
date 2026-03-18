# Variables to avoid repetition
DOCKER_COMPOSE = docker compose -f ./docker/docker-compose.yaml
CLIENT_DIR = ./client/Stopify

.PHONY: run-mac run-windows stop help

help:
	@echo "Available commands:"
	@echo "  make run-mac     - Start Docker and run Mac Catalyst client"
	@echo "  make run-windows - Start Docker and run Windows client"
	@echo "  make stop        - Shut down Docker containers"

run-mac:
	$(DOCKER_COMPOSE) up -d
	cd $(CLIENT_DIR) && dotnet run -f net9.0-maccatalyst

run-windows:
	$(DOCKER_COMPOSE) up -d
	cd $(CLIENT_DIR) && dotnet run -f net9.0-windows10.0.19041.0

stop:
	$(DOCKER_COMPOSE) down