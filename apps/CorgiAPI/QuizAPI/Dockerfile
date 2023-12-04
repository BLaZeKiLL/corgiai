#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#Run the following command from sln folder: docker build -t quiz-api -f ./QuizAPI/Dockerfile .

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["QuizAPI/QuizAPI.csproj", "QuizAPI/"]
RUN dotnet restore "./QuizAPI/./QuizAPI.csproj"
COPY . .
WORKDIR "/src/QuizAPI"
RUN dotnet build "./QuizAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./QuizAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ARG PORT=8300
ENV PORT=${PORT}

ARG HOST=0.0.0.0
ENV HOST=${HOST}

ARG MODE="Production"
ENV ASPNETCORE_ENVIRONMENT=${MODE}

ENV ASPNETCORE_URLS=http://${HOST}:${PORT}/

EXPOSE ${PORT}

ENTRYPOINT ["dotnet", "QuizAPI.dll"]