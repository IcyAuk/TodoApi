FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["TodoApi.csproj","."]
RUN dotnet restore "TodoApi.csproj"
COPY . .
RUN dotnet build "TodoApi.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/build .
COPY --from=build /src/Migrations ./Migrations
EXPOSE 8080

ENTRYPOINT [ "dotnet","TodoApi.dll" ]