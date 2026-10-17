CONTAINER = qubica-pipeline

.PHONY: setup build test teardown

setup:
	docker compose -f docker-compose.pipeline.yml up --build -d

build:
	docker exec $(CONTAINER) dotnet build QubicaMultiplex.sln

test:
	docker exec $(CONTAINER) dotnet test --no-build QubicaMultiplex.sln

teardown:
	docker compose -f docker-compose.pipeline.yml down --rmi local
