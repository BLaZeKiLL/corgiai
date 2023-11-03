req:
	pipreqs ./apps/py-loader ;\

spa:
	cd apps/corgiai-spa && yarn dev

rund:
	sudo docker compose --profile linux --env-file ./config/.env up --build -d

run:
	sudo docker compose --profile linux --env-file ./config/.env up --build

clean:
	sudo docker compose --profile linux --env-file ./config/.env up --build --remove-orphans --force-recreate

log:
	sudo docker compose logs -f

stats:
	sudo docker compose ps

down:
	sudo docker compose down