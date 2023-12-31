# Use a Node.js base image
FROM node:15.12-alpine as build-stage

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./
COPY .env.Production .env

# Install project dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the Vue application
RUN npm run build

# Production image
FROM nginx:1.25-alpine

# Copy the built Vue application from the build-stage
COPY --from=build-stage /app/dist /usr/share/nginx/html
COPY production_nginx.conf /etc/nginx/conf.d/default.conf
# Expose port 80
EXPOSE 80

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]
