FROM mcr.microsoft.com/dotnet/core/sdk:3.1.403-alpine3.12 AS build
RUN mkdir app
COPY . /app/

RUN ls -lah /app

WORKDIR /app/Neighbor.Server/Neighbor.Server.Finance

RUN dotnet restore
RUN dotnet publish -c Release -r linux-musl-x64 -o output --no-restore



FROM alpine:latest as run
RUN mkdir app
COPY --from=build /app/Neighbor.Server/Neighbor.Server.Finance/output /app/

WORKDIR /app

RUN apk add --no-cache libstdc++ libintl
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT [ "./Neighbor.Server.Finance.MonthlyBalance" ]