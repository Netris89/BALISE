# BALISE

BALISE stands for *Borne d'Accès aux Lieux, Informations, Services et Expositions* (Access Point for Venues, Information, Services and Exhibitions).

It is an interactive kiosk application for visitors of the museums of the City of Liège, developed during my internship at the Département de la culture and as my thesis project (TFE).

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

- .NET SDK <!-- TODO: version -->
- A PostgreSQL instance
- An Auth0 application configured for a native client

### Installation

```bash
git clone https://github.com/Netris89/Museum.git
cd Museum
```

### Configuration

<!-- TODO: describe where the connection string and the Auth0 settings (domain, client ID, redirect URI) are set. Never commit secrets. -->

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

Developed as an internship project at the Département de la culture of the City of Liège and as a thesis project at my school.