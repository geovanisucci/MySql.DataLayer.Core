FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ./src .
RUN dotnet restore
RUN dotnet build

WORKDIR /src/MySql.DataLayer.Core