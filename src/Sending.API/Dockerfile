FROM mcr.microsoft.com/dotnet/sdk:3.1 as build

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://*:5000

EXPOSE 5000

WORKDIR /src


COPY ["Sending.Domain/Sending.Domain.csproj", "Sending.Domain/"]
COPY ["Sending.API/Sending.API.csproj", "Sending.API/"]
COPY ["Sending.UnitTests/Sending.UnitTests.csproj", "Sending.UnitTests/"]

RUN dotnet restore Sending.API/Sending.API.csproj

COPY ["Sending.Domain/.", "Sending.Domain/"]
COPY ["Sending.API/.", "Sending.API/"]
COPY ["Sending.UnitTests/.", "Sending.UnitTests/"]

FROM build as unittest
WORKDIR /src/Sending.UnitTests
CMD ["dotnet", "test", "--logger:trx"]

# WORKDIR /src/Sending.API
# RUN dotnet build -c Debug
# ENTRYPOINT ["dotnet", "run", "--no-build", "--no-launch-profile", "-c", "Debug", "--"]