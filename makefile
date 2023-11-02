req:
	pipreqs ./apps/py-loader ;\

rund:
	sudo docker compose --profile linux --env-file ./config/.env up --build -d

run:
	sudo docker compose --profile linux --env-file ./config/.env up --build

log:
	sudo docker compose logs -f

stats:
	sudo docker compose ps

down:
	sudo docker compose down