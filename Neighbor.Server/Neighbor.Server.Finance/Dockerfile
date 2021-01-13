FROM mcr.microsoft.com/dotnet/sdk:5.0.101-alpine3.12 as build
ENV ASPNETCORE_ENVIRONMENT=SIT
RUN mkdir app
COPY . /app/

WORKDIR /app/Neighbor.Server/Neighbor.Server.Finance
RUN dotnet restore
RUN dotnet publish -c Release -r linux-x64 -o output --no-restore
RUN cp /app/Neighbor.Certificates/arrakya.thddns.net.crt output/

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal as run
ENV ASPNETCORE_ENVIRONMENT=SIT
RUN mkdir app
COPY --from=build /app/Neighbor.Server/Neighbor.Server.Finance/output /app/

WORKDIR /app
RUN ls -lah
ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT [ "dotnet", "./Neighbor.Server.Finance.dll" ]