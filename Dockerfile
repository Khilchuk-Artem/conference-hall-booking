FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["ConferenceBooking.Api/ConferenceBooking.Api.csproj", "ConferenceBooking.Api/"]
COPY ["ConferenceBooking.Application/ConferenceBooking.Application.csproj", "ConferenceBooking.Application/"]
COPY ["ConferenceBooking.Infrastructure/ConferenceBooking.Infrastructure.csproj", "ConferenceBooking.Infrastructure/"]
COPY ["ConferenceBooking.Domain/ConferenceBooking.Domain.csproj", "ConferenceBooking.Domain/"]
RUN dotnet restore "ConferenceBooking.Api/ConferenceBooking.Api.csproj"
COPY . .
WORKDIR "/src/ConferenceBooking.Api"
RUN dotnet build "ConferenceBooking.Api.csproj" -c Release --no-restore -o /app/build

FROM build AS publish
RUN dotnet publish "ConferenceBooking.Api.csproj" -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "ConferenceBooking.Api.dll"]
