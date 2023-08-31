FROM node:lts-alpine    
WORKDIR /app
# install vite globally
RUN npm install -g @vue/cli
# copy all filtes
COPY . .    
# install all deps
RUN yarn install

# vite default port
EXPOSE 8080    
CMD ["vue", "serve"]
