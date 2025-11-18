# LinguaCorp Translation API

LBA - APIs .NET de André Vieira Tech 108

## Auth

JWT Bearer token authentication

### GCP Link

https://console.cloud.google.com/billing/linkedaccount?project=linguacorp-api-prod

### Token

Send a POST request to `/api/auth/login` with credentials:

#### Request
```json
{
  "username": "admin",
  "password": "admin"
}
```

#### Response
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 86400
}
```

### Using the Token with tools like Postman, etc...

Include the token in the Authorization tab/header as "Bearer token" for all API requests by pasting it inside without the need of "Bearer ..."

### Swagger UI

1. Navigate to `/swagger`
2. Click "Authorize" button (top right)
3. Paste your token (just the token, "Bearer" is added automatically)
4. Click "Authorize"
5. Now you can test all endpoints


# Deployment da API para Google Cloud Run - Guia

## 1. Criação dos Ficheiros de Deployment

### Dockerfile
```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY LinguaCorp.sln ./
COPY LinguaCorp.API/LinguaCorp.API.csproj LinguaCorp.API/

RUN dotnet restore LinguaCorp.sln -r linux-x64

COPY LinguaCorp.API/ LinguaCorp.API/

WORKDIR /src/LinguaCorp.API
RUN dotnet publish -c Release -o /app/publish -r linux-x64 --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

RUN groupadd -g 1000 appuser && \
    useradd -u 1000 -g appuser -s /bin/sh -m -d /home/appuser appuser && \
    chown -R appuser:appuser /app

COPY --from=build --chown=appuser:appuser /app/publish .

USER appuser

ENV PORT=8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:${PORT}/health || exit 1

CMD ["dotnet", "LinguaCorp.API.dll"]
```

### .dockerignore
```
**/bin/
**/obj/
**/.vs/
**/.vscode/
**/node_modules/
**/.git/
**/.gitignore
**/README.md
**/*.user
```

### .gcloudignore
```
.git
.gitignore
bin/
obj/
.vs/
.vscode/
```

## 2. Atualização do Program.cs

Adiciona após `var builder = WebApplication.CreateBuilder(args);`:

```csharp
// Configurar port da cloud run
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}
```

Adiciona antes de `app.Run();`:

```csharp
// Health check endpoint para Cloud Run (necessário)
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
    .ExcludeFromDescription();
```

## 3. Configuração do Google Cloud

### Autenticação
```cmd
gcloud auth login
```

### Criação do Projeto
```cmd
gcloud projects create linguacorp-api-prod --name="LinguaCorp API"
gcloud config set project linguacorp-api-prod
```

### Ativação dos Serviços
```cmd
gcloud services enable run.googleapis.com
gcloud services enable artifactregistry.googleapis.com
gcloud services enable cloudbuild.googleapis.com
```

### Ativar Faturação
Visita: https://console.cloud.google.com/billing/linkedaccount?project=linguacorp-api-prod (este é o meu link)

## 4. Deployment

Na pasta raiz do projeto (onde está o `.sln`):

```cmd
gcloud run deploy linguacorp-api --source . --region europe-west1 --allow-unauthenticated --platform managed --port 8080 --memory 512Mi
```

## 5. Verificar o Deployment

### Obter URL do serviço
```cmd
gcloud run services describe linguacorp-api --region europe-west1 --format='value(status.url)'
```

### Ver logs
```cmd
gcloud run services logs read linguacorp-api --region europe-west1
```

### Testar a API
```cmd
curl https://[O-TEU-URL]/health
```
