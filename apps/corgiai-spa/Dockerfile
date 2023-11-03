FROM node:lts as build

WORKDIR /app

COPY package.json .
COPY yarn.lock .

RUN yarn install

COPY . .

RUN NODE_ENV=production yarn build

FROM nginx

COPY --from=build /app/dist /usr/share/nginx/html