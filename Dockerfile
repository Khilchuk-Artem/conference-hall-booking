FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/ConferenceBooking.Api/ConferenceBooking.Api.csproj", "src/ConferenceBooking.Api/"]
COPY ["src/ConferenceBooking.Application/ConferenceBooking.Application.csproj", "src/ConferenceBooking.Application/"]
COPY ["src/ConferenceBooking.Infrastructure/ConferenceBooking.Infrastructure.csproj", "src/ConferenceBooking.Infrastructure/"]
COPY ["src/ConferenceBooking.Domain/ConferenceBooking.Domain.csproj", "src/ConferenceBooking.Domain/"]
RUN dotnet restore "src/ConferenceBooking.Api/ConferenceBooking.Api.csproj"
COPY . .
WORKDIR "/src/src/ConferenceBooking.Api"
RUN dotnet build "ConferenceBooking.Api.csproj" -c Release --no-restore -o /app/build

FROM build AS publish
RUN dotnet publish "ConferenceBooking.Api.csproj" -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "ConferenceBooking.Api.dll"]
