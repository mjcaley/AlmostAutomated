FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /source

COPY *.sln .
COPY src/ src/
COPY tests/ tests/
RUN dotnet restore

WORKDIR /source/src/AlmostAutomated.Api
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

COPY --from=build /app ./
ENTRYPOINT [ "dotnet", "AlmostAutomated.Api.dll" ]
