FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine as build
WORKDIR /source

COPY src/ .
RUN dotnet restore ./AlmostAutomated.Migration/

WORKDIR /source/AlmostAutomated.Migration
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

COPY --from=build /app ./

ENV ASPNETCORE_URLs=http://*:5000
ENV DB_HOST="host.docker.internal"
ENV DB_PORT=5432
ENV DB_USERNAME="almostapidb"
ENV DB_PASSWORD="password"
ENV DB_NAME="almostapidb"

ENTRYPOINT [ "dotnet", "AlmostAutomated.Migration.dll" ]
