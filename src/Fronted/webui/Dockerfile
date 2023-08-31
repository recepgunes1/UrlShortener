# Use the official Node.js image as the base image
FROM node:14

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json before other files
COPY package*.json ./

# Install dependencies including devDependencies
RUN npm install

# Copy the rest of the application to the container
COPY . .

# Expose the default Vue.js development server port
EXPOSE 8080

# Command to run the app in development mode
CMD ["npm", "run", "serve"]
