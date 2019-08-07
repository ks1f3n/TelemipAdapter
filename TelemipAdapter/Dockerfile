FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["TelemipAdapter.csproj", "TelemipAdapter/"]
RUN dotnet restore "TelemipAdapter.csproj"
COPY . .
WORKDIR "/src/TelemipAdapter"
RUN dotnet build "TelemipAdapter.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TelemipAdapter.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TelemipAdapter.dll"]
