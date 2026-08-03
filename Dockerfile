# 1) BUILD asamasi — SDK imajiyla derle ve yayimla
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src


COPY Domain/Domain.csproj Domain/
COPY Application/Application.csproj Application/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY DCOM-API/DCOM-API.csproj DCOM-API/
RUN dotnet restore DCOM-API/DCOM-API.csproj


COPY . .
RUN dotnet publish DCOM-API/DCOM-API.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "DCOM-API.dll"]