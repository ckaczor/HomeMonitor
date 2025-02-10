# build stage
FROM node:lts-alpine AS build-stage
RUN npm install -g corepack@latest
RUN corepack enable && corepack prepare pnpm@latest --activate
WORKDIR /app
COPY package*.json ./
RUN pnpm install
COPY . .
RUN pnpm run build

# production stage
FROM nginx:stable-alpine AS production-stage
COPY nginx/default.conf /etc/nginx/conf.d/
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build-stage /app/dist /usr/share/nginx/html
EXPOSE 80
COPY ./nginx/entrypoint.sh ./docker-entrypoint.d/entrypoint.sh
RUN chmod +x ./docker-entrypoint.d/entrypoint.sh
RUN echo "daemon off;" >> /etc/nginx/nginx.conf
CMD ["nginx"]