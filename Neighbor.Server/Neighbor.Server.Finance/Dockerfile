FROM mcr.microsoft.com/dotnet/core/sdk:3.1.403-alpine3.12 AS build
ENV ASPNETCORE_ENVIRONMENT=Production
RUN mkdir app
COPY . /app/

WORKDIR /app/Neighbor.Server/Neighbor.Server.Finance
RUN dotnet restore
RUN dotnet publish -c Release -r linux-x64 -o output --no-restore


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.9-focal as run
ENV ASPNETCORE_ENVIRONMENT=Production
RUN mkdir app
COPY --from=build /app/Neighbor.Server/Neighbor.Server.Finance/output /app/

WORKDIR /app
RUN ls -lah
ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT [ "dotnet", "./Neighbor.Server.Finance.MonthlyBalance.dll" ]