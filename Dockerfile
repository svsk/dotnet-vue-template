# Build Vue App
FROM node:16-alpine AS node-build-env
WORKDIR /app

COPY . ./

WORKDIR ./src/vuetemplate.app
RUN npm ci
RUN npm run build

# Build dotnet API
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS dotnet-build-env
WORKDIR /app

COPY . ./

WORKDIR ./src/vuetemplate.api
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=dotnet-build-env /app/src/vuetemplate.api/out .
COPY --from=node-build-env /app/src/vuetemplate.app/dist ./wwwroot
ENTRYPOINT ["dotnet", "VueTemplate.Api.dll"]
