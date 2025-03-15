# Stage 1: Build the Angular application
FROM node:latest AS build

WORKDIR /dist/src/app

RUN npm cache clean --force

COPY . .
RUN npm install
RUN npm run build --prod


FROM nginx:latest AS ngi

COPY --from=build /dist/src/app/dist/ /usr/share/nginx/html
COPY /nginx.conf  /etc/nginx/conf.d/default.conf

EXPOSE 80
