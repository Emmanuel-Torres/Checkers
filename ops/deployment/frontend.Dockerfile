FROM node:14.17-alpine AS client_build
WORKDIR /usr/src/app
COPY package.json .
COPY package-lock.json .
RUN npm install
COPY . .
RUN npm run build

FROM nginx AS nginx_build
COPY --from=client_build /usr/src/app/build /usr/share/nginx/html
EXPOSE 80