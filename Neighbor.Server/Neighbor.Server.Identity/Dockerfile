# At solution root dir run
# PROD environment : docker build -f ./Neighbor.Server/Neighbor.Server.Identity/Dockerfile -t arrak/neighbor.server.identity:prod-latest . --build-arg EnvName=Production --no-cache
# SIT environment  : docker build -f ./Neighbor.Server/Neighbor.Server.Identity/Dockerfile -t arrak/neighbor.server.identity:sit-latest . --build-arg EnvName=SIT --no-cache
# above command will build docker image of Neighbor.Server.Identity for SIT environment

FROM mcr.microsoft.com/dotnet/sdk:5.0.101-alpine3.12 as build
ARG EnvName
ENV ASPNETCORE_ENVIRONMENT $EnvName
RUN mkdir app
COPY . /app/
RUN echo "Build for $ASPNETCORE_ENVIRONMENT environment."
WORKDIR /app/Neighbor.Server/Neighbor.Server.Identity
RUN dotnet restore
RUN dotnet publish -c $EnvName -r linux-x64 -o output --no-restore
RUN cp /app/Neighbor.Certificates/arrakya.thddns.net.crt output/
RUN cp /app/Neighbor.Certificates/arrakya.thddns.net.pfx output/

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal as run
ENV ASPNETCORE_ENVIRONMENT $EnvName
RUN mkdir app
COPY --from=build /app/Neighbor.Server/Neighbor.Server.Identity/output /app/

WORKDIR /app
RUN ls -lah
ENV ASPNETCORE_URLS http://*:6000

ENTRYPOINT [ "dotnet", "./Neighbor.Server.Identity.dll" ]