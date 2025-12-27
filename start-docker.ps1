# Stop on any errors
$ErrorActionPreference = "Stop"

Write-Host "Stopping and cleaning old containers..."
# Bring down all containers in the project to start fresh
docker compose -f docker-compose.infra.yml -f docker-compose.yml down

Write-Host "Starting all containers..."
# Start infra first
docker compose -f docker-compose.infra.yml -f docker-compose.yml up -d --build

Write-Host "All containers should now be running."
Write-Host "You can check with 'docker ps'."
