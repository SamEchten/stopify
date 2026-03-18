#!/bin/bash

set -e

FRAMEWORK=""

usage() {
  echo "Usage: $0 --framework <mac|windows>"
  exit 1
}

while [[ $# -gt 0 ]]; do
  case "$1" in
    --framework)
      FRAMEWORK="$2"
      shift 2
      ;;
    *)
      usage
      ;;
  esac
done

case "$FRAMEWORK" in
  mac)
    TARGET="net9.0-maccatalyst"
    ;;
  windows)
    TARGET="net9.0-windows10.0.19041.0"
    ;;
  *)
    echo "Error: --framework must be 'mac' or 'windows'"
    usage
    ;;
esac

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "Starting backend..."
docker compose -f "$SCRIPT_DIR/docker/docker-compose.yaml" up -d

echo "Starting client ($TARGET)..."
cd "$SCRIPT_DIR/client/Stopify"
dotnet run -f "$TARGET"
