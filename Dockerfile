FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ProjectManager.API/*.csproj ./ProjectManager.API/
COPY ProjectManager.Data/*.csproj ./ProjectManager.Data/
COPY ProjectManager.Domain/*.csproj ./ProjectManager.Domain/
RUN dotnet restore

# copy everything else and build app
COPY ProjectManager.API/. ./ProjectManager.API/
COPY ProjectManager.Data/. ./ProjectManager.Data/
COPY ProjectManager.Domain/. ./ProjectManager.Domain/
WORKDIR /source/ProjectManager.API/
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Colors.API.dll