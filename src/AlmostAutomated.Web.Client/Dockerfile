FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine as build

WORKDIR /source
COPY src/ .

WORKDIR /source/AlmostAutomated.Web.Client
RUN dotnet workload restore
RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

FROM nginx:alpine

WORKDIR /usr/share/nginx/html
COPY --from=build /app/wwwroot .
COPY src/AlmostAutomated.Web.Client/nginx.conf /etc/nginx/nginx.conf
