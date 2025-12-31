FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /repo

COPY SimplePOS.sln ./
COPY src/SimplePOS.Api/SimplePOS.Api.csproj src/SimplePOS.Api/
COPY src/SimplePOS.Application/SimplePOS.Application.csproj src/SimplePOS.Application/
COPY src/SimplePOS.Domain/SimplePOS.Domain.csproj src/SimplePOS.Domain/
COPY src/SimplePOS.Infrastructure/SimplePOS.Infrastructure.csproj src/SimplePOS.Infrastructure/

RUN dotnet restore SimplePOS.sln

COPY src/ src/

WORKDIR /repo/src/SimplePOS.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SimplePOS.Api.dll"]