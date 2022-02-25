FROM node:14.17-alpine
WORKDIR /usr/src/app
COPY package.json .
COPY package-lock.json .
RUN npm install
COPY . .
EXPOSE 80
CMD ["npm", "start"]