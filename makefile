req:
	pipreqs ./apps/py-loader ;\

run:
	sudo docker compose --profile linux up --build