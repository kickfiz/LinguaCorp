# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY LinguaCorp.sln ./
COPY LinguaCorp.API/LinguaCorp.API.csproj LinguaCorp.API/

# Restore dependencies with runtime identifier
RUN dotnet restore LinguaCorp.sln -r linux-x64

# Copy remaining source code
COPY LinguaCorp.API/ LinguaCorp.API/

# Build and publish the application
# Using ReadyToRun for faster startup times (important for Cloud Run cold starts)
WORKDIR /src/LinguaCorp.API
RUN dotnet publish -c Release -o /app/publish -r linux-x64 --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create a non-root user for security
RUN groupadd -g 1000 appuser && \
    useradd -u 1000 -g appuser -s /bin/sh -m -d /home/appuser appuser && \
    chown -R appuser:appuser /app

# Copy published application from build stage
COPY --from=build --chown=appuser:appuser /app/publish .

# Switch to non-root user
USER appuser

# Set default port (Cloud Run will override with PORT env var)
ENV PORT=8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Expose port (documentation only, Cloud Run will set this dynamically)
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:${PORT}/health || exit 1

# Start the application
# Use shell form to allow environment variable expansion
CMD ["dotnet", "LinguaCorp.API.dll"]