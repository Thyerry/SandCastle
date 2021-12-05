FROM node:lts as dependencies
WORKDIR /sandcastle-frontend
COPY package.json ./
RUN npm install --frozen-lockfile

FROM node:lts as builder
WORKDIR /sandcastle-frontend
COPY . .
COPY --from=dependencies /sandcastle-frontend/node_modules ./node_modules
RUN npm run build

FROM node:lts as runner
WORKDIR /sandcastle-frontend
ENV NODE_ENV production
# If you are using a custom next.config.js file, uncomment this line.
# COPY --from=builder /sandcastle-frontend/next.config.js ./
COPY --from=builder /sandcastle-frontend/public ./public
COPY --from=builder /sandcastle-frontend/.next ./.next
COPY --from=builder /sandcastle-frontend/node_modules ./node_modules
COPY --from=builder /sandcastle-frontend/package.json ./package.json

EXPOSE 3000
CMD ["npm", "start"]