FROM node:lts as build

WORKDIR /app

COPY package.json .
COPY yarn.lock .

RUN yarn install

COPY . .

ARG MODE=production

RUN if [ "$MODE" = "production" ]; \
    then NODE_ENV=production yarn build; \
    else yarn build:tailscale; \
    fi

FROM nginx

COPY --from=build /app/dist /usr/share/nginx/html