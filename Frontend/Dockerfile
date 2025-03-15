# Stage 1: Build the Angular application
FROM node:18-alpine AS build

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build --production

# Stage 2: Serve the built application with nginx
FROM nginx:alpine

COPY --from=build /app/dist/frontend /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf
# Copy the nginx configuration file

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
