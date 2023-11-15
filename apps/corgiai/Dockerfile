FROM node:lts as builder

WORKDIR /app

COPY package.json .
COPY yarn.lock .

RUN yarn install

COPY . .

RUN NODE_ENV=production yarn build

FROM node:lts

COPY --from=builder /app/build build
COPY --from=builder /app/node_modules node_modules

COPY package.json .

ENV NODE_ENV=production

EXPOSE 8001

ENTRYPOINT [ "node", "build" ]