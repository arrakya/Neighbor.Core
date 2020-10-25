# dotnet publish -o app -c -r linux-musl-x64
FROM mcr.microsoft.com/dotnet/core/sdk:3.1.403-alpine3.12 AS build
RUN mkdir app
WORKDIR /app
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -r linux-musl-x64 -o output --no-restore



FROM alpine:latest as run

RUN mkdir app
COPY --from=build /app/output /app/

WORKDIR /app
RUN ls /

RUN apk add --no-cache libstdc++ libintl
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT [ "./Neighbor.Finance.MonthlyBalance" ]