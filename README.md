# ShortURL WebApp (ASP.NET Core)

Simple and modern URL shortener with:
- Dark/light theme UI.
- Token feature (JWT issuance + protected APIs).
- Secure defaults and encrypted critical configuration support.
- Flexible architecture for future growth.

## API planning

### Auth
- `POST /api/token`
  - Header: `X-API-KEY: <AppSecurity:ApiToken>`
  - Returns: JWT access token.

### URL APIs (protected)
- `POST /api/shorten`
  - Bearer token required.
  - Body: `{ "url": "https://example.com" }`
  - Returns short code + short URL.
- `GET /api/urls`
  - Bearer token required.
  - Returns list of generated short URLs.

### Redirect API
- `GET /s/{code}`
  - Redirects to original URL.

## Security and encryption

- `AppSecurity:ApiToken` protects token minting endpoint.
- JWT signing key loaded from `Token:SigningKeyEncrypted`.
- App tries to decrypt this value through ASP.NET Core Data Protection service.
- If value is plain text (development), app falls back safely.

### Encrypting critical values example

Inject `IEncryptedConfigurationService` and call:

```csharp
var encrypted = encryptedConfigurationService.Encrypt("your-very-secret-key");
```

Store output in `appsettings`/secret store as `SigningKeyEncrypted`.

## Run

```bash
dotnet restore
dotnet run
```

Open `https://localhost:5001` (or printed URL).
