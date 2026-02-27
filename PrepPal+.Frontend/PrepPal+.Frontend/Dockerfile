FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci

#using a non root user
#USER node 
#COPY --chown=node:node . .
COPY . .

ARG VITE_API_BASE_URL
ENV VITE_API_BASE_URL=$VITE_API_BASE_URL

RUN npm run build -- --mode docker

#apparently exposes port 80
FROM nginx:alpine
EXPOSE 80
COPY --from=build /app/dist /usr/share/nginx/html