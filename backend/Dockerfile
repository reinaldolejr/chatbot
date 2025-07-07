FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["chatbot.Api/chatbot.Api.csproj", "chatbot.Api/"]
COPY ["chatbot.Application/chatbot.Application.csproj", "chatbot.Application/"]
COPY ["chatbot.Domain/chatbot.Domain.csproj", "chatbot.Domain/"]
COPY ["chatbot.Infrastructure/chatbot.Infrastructure.csproj", "chatbot.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "chatbot.Api/chatbot.Api.csproj"

# Copy source code
COPY . .

# Build the application
WORKDIR "/src/chatbot.Api"
RUN dotnet build "chatbot.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "chatbot.Api.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "chatbot.Api.dll"] 