# LinguaCorp Translation API

LBA - APIs .NET de André Vieira Tech 108

## Auth

JWT Bearer token authentication

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
