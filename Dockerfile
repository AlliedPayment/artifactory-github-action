FROM mcr.microsoft.com/dotnet/sdk:3.1-bionic AS build-env 
COPY . /artifactory/
WORKDIR /artifactory
RUN dotnet restore  ./artifactory/artifactory.csproj 
RUN dotnet build  ./artifactory/artifactory.csproj

FROM mcr.microsoft.com/dotnet/runtime:3.1-bionic
COPY --from=build-env /artifactory/artifactory/bin/Debug/netcoreapp3.1 ./app

ENTRYPOINT ["/app/artifactory"]
