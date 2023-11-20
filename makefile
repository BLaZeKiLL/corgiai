app:
	cd apps/corgiai && yarn dev

rund:
	sudo docker compose --profile linux --env-file ./config/.env up --build -d

run:
	sudo docker compose --profile linux --env-file ./config/.env up --build

clean:
	sudo docker compose --profile linux --env-file ./config/.env up --build -d --remove-orphans --force-recreate

log:
	sudo docker compose logs -f

stats:
	sudo docker stats

down:
	sudo docker compose down

build-app:
	sudo docker build ./apps/corgiai -t corgiai:latest --build-context config=./config