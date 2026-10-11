# BALISE

BALISE stands for *Borne d'Accès aux Lieux, Informations, Services et Expositions* (Access Point for Venues, Information, Services and Exhibitions).

It is an interactive kiosk application for visitors of the museums of the City of Liège, developed during my internship at the Département de la culture and as my thesis project (TFE).

![.NET](https://img.shields.io/badge/.NET-10.0-purple?style=for-the-badge)![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)![Avalonia](https://img.shields.io/badge/Avalonia-%238B44AC.svg?style=for-the-badge&logo=avaloniaui&logoColor=white)![ReactiveUI](https://img.shields.io/badge/ReactiveUI-%23B5179E.svg?style=for-the-badge)![EF Core](https://img.shields.io/badge/EF_Core-%23512BD4.svg?style=for-the-badge)![PostgreSQL](https://img.shields.io/badge/PostgreSQL-%234169E1.svg?style=for-the-badge&logo=postgresql&logoColor=white)![Auth0](https://img.shields.io/badge/Auth0-%23EB5424.svg?style=for-the-badge&logo=auth0&logoColor=white)![Mapsui](https://img.shields.io/badge/Mapsui-%232E7D32.svg?style=for-the-badge)

## Features

**Visitor kiosk**
- Interactive floor maps of each museum, with spaces and points of interest
- Museum agenda
- Newsletter registration

**Content management**
- Secure staff login through Auth0 (OIDC)
- Management of spaces and points of interest
- Bulk content import from a JSON file
- Multilingual content support

## Tech stack

| Area | Technology |
|---|---|
| UI | [Avalonia](https://avaloniaui.net/) |
| Pattern | MVVM with [ReactiveUI](https://www.reactiveui.net/) |
| Data access | Entity Framework Core |
| Database | PostgreSQL |
| Authentication | OIDC with Auth0 (Duende client library) |
| Map | [Mapsui](https://mapsui.com/) |
| Language | C# |

## Getting started

### Prerequisites

- .NET SDK 10
- A PostgreSQL instance
- An Auth0 application configured for a native client

### Installation

```bash
git clone https://github.com/Netris89/Museum.git
cd Museum
```

### Configuration

Configuration lives in `appsettings.json`. Set the following values before the first run:

| Key | Description |
|---|---|
| `Postgres:ConnectionString` | PostgreSQL connection string |
| `Auth0:Domain` | Auth0 tenant domain |
| `Auth0:ClientId` | Client ID of the native Auth0 application |
| `Auth0:RedirectUri` | Redirect URI registered in Auth0 |

Do not commit secrets. Use [user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) or environment variables for local values.

### Database

```bash
dotnet ef database update
```

### Run

```bash
dotnet run
```

## Architecture

The application follows the MVVM pattern:

- **Models and DTOs**: domain entities and data transfer objects
- **Repositories**: data access through Entity Framework Core
- **Services**: business logic, authentication and session management
- **ViewModels**: presentation logic built with ReactiveUI
- **Views**: Avalonia XAML views

## Content import

Content can be imported from a JSON file. Currently supported:

- Creation of spaces

Not yet supported:

- Standalone points of interest (without a parent space)
- Update of objects that already exist in the database

## Context

Developed as an internship project at the Département de la culture of the City of Liège and as a thesis project at the Institut Saint-Laurent in Liège.