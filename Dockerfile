FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Freelance Project Management Platform/Freelance Project Management Platform.csproj", "Freelance Project Management Platform/"]
RUN dotnet restore "Freelance Project Management Platform/Freelance Project Management Platform.csproj"
COPY . .
WORKDIR "/src/Freelance Project Management Platform"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Freelance Project Management Platform.dll"]