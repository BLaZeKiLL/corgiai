req:
	pipreqs ./apps/py-loader ;\

spa:
	cd apps/corgiai-spa && yarn dev

rund: tailscale
	sudo docker compose --profile linux --env-file ./config/.env up --build -d

run: tailscale
	sudo docker compose --profile linux --env-file ./config/.env up --build

clean:
	sudo docker compose --profile linux --env-file ./config/.env up --build --remove-orphans --force-recreate

log:
	sudo docker compose logs -f

stats:
	sudo docker stats

down:
	sudo docker compose down

tailscale:
	cd apps/corgiai-spa && node tailscale.env.js