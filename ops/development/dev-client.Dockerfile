FROM node:14.17-alpine
WORKDIR /var/app/frontend
COPY package.json .
COPY package-lock.json .
RUN npm install
COPY . .
EXPOSE 80
CMD ["npm", "start"]