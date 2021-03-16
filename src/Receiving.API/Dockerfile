FROM mcr.microsoft.com/dotnet/sdk:3.1 

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://*:5001

EXPOSE 5001

WORKDIR /src

COPY ["Receiving.API/Receiving.API.csproj", "Receiving.API/"]

RUN dotnet restore Receiving.API/Receiving.API.csproj

COPY ["Receiving.API/.", "Receiving.API/"]

WORKDIR /src/Receiving.API
RUN dotnet build -c Debug
ENTRYPOINT ["dotnet", "run", "--no-build", "--no-launch-profile", "-c", "Debug", "--"]