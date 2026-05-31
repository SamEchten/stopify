# Stopify

A cross-platform music streaming application built with .NET MAUI (client) and ASP.NET Core (server), backed by MariaDB.

---

## Getting Started

Prerequisites: [Docker](https://www.docker.com/) and the [.NET 9 SDK](https://dotnet.microsoft.com/download).

### macOS

```bash
make run-mac
```

### Windows

```bash
make run-windows
```

Both commands start the Docker containers (API + database) and launch the client. To rebuild the Docker image first, append `BUILD=1`:

```bash
make run-mac BUILD=1
```

### Stop the stack

```bash
make stop
```

---

## Components

### Client (`client/Stopify`) — .NET MAUI Razor Components

**Layout**

| Component | Description |
|-----------|-------------|
| `MainLayout` | Root layout wrapping all authenticated pages |
| `LoginLayout` | Minimal layout used for auth pages |
| `NavMenu` | Side navigation menu |
| `TopBar` | Top navigation bar with search and user actions |
| `PlayerBar` | Persistent playback controls bar |

**Pages**

| Component | Description |
|-----------|-------------|
| `Home` | Landing page with song/artist overview |
| `Login` / `Register` | User authentication flows |
| `Logout` | Clears session and redirects to login |
| `Profile` | Displays and edits the current user's profile |
| `ArtistPage` | Lists all artists |
| `ArtistDetail` | Artist detail with their songs |
| `Playlists` | Lists all playlists for the current user |
| `PlaylistDetail` | Playlist detail with track management |
| `CreatePlaylist` | Form to create a new playlist |
| `CurrentSession` | Shows the active listening session |

---

### Server (`server/Stopify`) — ASP.NET Core 9 Web API

| Layer | Description |
|-------|-------------|
| **Controllers** | REST endpoints for auth, songs, artists, playlists, and sessions |
| **Application (CQRS)** | MediatR commands and queries with dedicated handlers |
| **Services** | Business logic (auth, music playback, sessions) |
| **Repositories** | Data-access layer on top of Entity Framework Core |
| **Hubs** | SignalR hub for real-time session synchronisation |
| **Middleware** | Global exception handling and JWT refresh logic |
| **Migrations** | EF Core database migrations targeting MariaDB |

---

### Infrastructure

| Component | Technology |
|-----------|------------|
| Database | MariaDB 10.4 |
| ORM | Entity Framework Core 9 + Pomelo MySQL provider |
| Authentication | JWT Bearer + refresh tokens stored in cookies |
| Real-time | ASP.NET Core SignalR |
| Audio metadata | TagLibSharp |
| API docs | Swagger / OpenAPI (Swashbuckle) |
| Containerisation | Docker Compose (`docker/docker-compose.yaml`) |
