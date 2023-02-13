FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine as build
WORKDIR /source

COPY src/ .
RUN dotnet restore ./AlmostAutomated.Api/

WORKDIR /source/AlmostAutomated.Api
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

COPY --from=build /app ./

ENV ASPNETCORE_URLs=http://*:5000

ENTRYPOINT [ "dotnet", "AlmostAutomated.Api.dll" ]
