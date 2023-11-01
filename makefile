req:
	pipreqs ./apps/py-loader ;\

run:
	sudo docker compose --profile linux --env-file ./config/.env up --build -d

log:
	sudo docker compose logs -f

stats:
	sudo docker compose ps