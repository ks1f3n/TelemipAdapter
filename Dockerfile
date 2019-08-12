FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000

# copy csproj and restore as distinct layers
COPY *.sln .
COPY TelemipAdapter/*.csproj ./TelemipAdapter/
RUN dotnet restore

# copy everything else and build app
COPY TelemipAdapter/. ./TelemipAdapter/
WORKDIR /app/TelemipAdapter
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/TelemipAdapter/out ./
ENTRYPOINT ["dotnet", "TelemipAdapter.dll"]
