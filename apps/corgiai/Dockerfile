FROM node:lts as builder

WORKDIR /app

COPY package.json .
COPY yarn.lock .

RUN yarn install

COPY . .

RUN NODE_ENV=production yarn build

FROM node:lts as runtime

COPY package.json .
COPY yarn.lock .

RUN yarn install --frozen-lockfile

FROM node:lts

COPY --from=builder /app/build build
# COPY --from=runtime /app/node_modules node_modules

COPY package.json .
COPY yarn.lock .

ENV NODE_ENV=production

ARG PORT=8001
ENV PORT=${PORT}

ARG HOST=0.0.0.0
ENV HOST=${HOST}

EXPOSE ${PORT}

ENTRYPOINT [ "node", "build" ]