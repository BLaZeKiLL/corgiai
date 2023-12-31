FROM node:lts-alpine as builder

WORKDIR /app

COPY package.json .
COPY yarn.lock .

RUN yarn install

COPY . .

RUN NODE_ENV=production yarn build

FROM node:lts-alpine as runtime

COPY --from=builder /app/build app/build

WORKDIR /app

COPY package.json .
COPY yarn.lock .

RUN yarn install --frozen-lockfile --production

FROM node:lts-alpine

COPY --from=runtime /app/build app/build
COPY --from=runtime /app/node_modules app/node_modules

# Copy Tailscale binaries from the tailscale image on Docker Hub.
COPY --from=docker.io/tailscale/tailscale:stable /usr/local/bin/tailscaled /app/tailscaled
COPY --from=docker.io/tailscale/tailscale:stable /usr/local/bin/tailscale /app/tailscale
RUN mkdir -p /var/run/tailscale /var/cache/tailscale /var/lib/tailscale

WORKDIR /app

COPY .env.production .env
COPY package.json .
COPY yarn.lock .
COPY start.sh .

RUN ["chmod", "0755", "./start.sh"]

ENV NODE_ENV=production

ARG PORT=8080
ENV PORT=${PORT}

ARG HOST=0.0.0.0
ENV HOST=${HOST}

EXPOSE ${PORT}

ENTRYPOINT [ "sh", "./start.sh" ]