req:
	pipreqs ./apps/py-loader ;\

run:
	sudo docker compose --profile linux --env-file ./config/.env up --build