FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY api/*.csproj ./api/
RUN dotnet restore

# copy everything else and build app
COPY api/. ./api/
WORKDIR /app/api
RUN dotnet publish -c release -o out


FROM microsoft/aspnetcore:2.0 AS runtime
WORKDIR /app
COPY model.zip .
COPY --from=build /app/api/out ./
ENTRYPOINT ["dotnet", "api.dll"]