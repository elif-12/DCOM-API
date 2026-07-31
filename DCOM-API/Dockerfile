# 1) BUILD asamasi — SDK imajiyla derle ve yayimla
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Once csproj'u kopyalayip restore et (katman cache icin)
COPY DCOM-API.csproj .
RUN dotnet restore DCOM-API.csproj

# Kalan kodu kopyala ve Release yayimla
COPY . .
RUN dotnet publish DCOM-API.csproj -c Release -o /app/publish

# 2) RUNTIME asamasi — kucuk aspnet imajiyla sadece calistir
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Konteyner icinde 8080 portunda dinle
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "DCOM-API.dll"]