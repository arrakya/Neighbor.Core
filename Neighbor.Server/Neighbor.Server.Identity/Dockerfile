FROM mcr.microsoft.com/dotnet/core/sdk:3.1.403-alpine3.12 AS build
ENV ASPNETCORE_ENVIRONMENT=Production
RUN mkdir app
COPY . /app/

WORKDIR /app/Neighbor.Server/Neighbor.Server.Identity
RUN dotnet restore
RUN dotnet publish -c Release -r linux-x64 -o output --no-restore


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.9-focal as run
ENV ASPNETCORE_ENVIRONMENT=Production
RUN mkdir app
COPY --from=build /app/Neighbor.Server/Neighbor.Server.Identity/output /app/

WORKDIR /app
RUN ls -lah
ENV ASPNETCORE_URLS http://*:6000
ENV NEIGHBOR_IDENTITY_KEY 310060161466031006016146603100601614660

ENTRYPOINT [ "dotnet", "./Neighbor.Server.Identity.dll" ]