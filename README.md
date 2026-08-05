# guidebot-api

Backend API supporting a campus guide robot used in a Human-Robot Interaction (HRI)
research study. The robot gives wayfinding/directions to visitors and periodically
polls this API to log its interactions and report that it's still active, so
researchers can monitor the robot's status remotely without being physically present.

## Purpose

- **Interaction logging** — the robot reports what was said (by itself and by the
  person it's guiding) so conversations/sessions can be reviewed and analyzed later.
- **Remote status tracking** — the robot polls the API on an interval, giving
  researchers a way to tell whether it's online and functioning without checking on
  it in person.
- **Directions** — the robot requests wayfinding/direction data for the campus from
  the API rather than embedding it locally.

## Status

This project is in early development. The API surface, data models, and services
are still being built out; several files are intentional placeholders:

- `Controllers/InteractionController.cs` — `POST /Interaction` (`LogInteraction`)
  is scaffolded but not yet implemented (currently returns `404`).
- `Models/Interaction.cs` — has an initial shape (`Speech`, `RobotTurn`) but is
  expected to change as logging requirements are finalized.
- `Models/Event.cs`, `Services/DirectionsService.cs`,
  `Services/IDirectionService.cs` — empty stubs reserved for the directions
  feature and event/status logging; not implemented yet.

## Tech stack

- [.NET 9](https://dotnet.microsoft.com/) / ASP.NET Core (MVC + Web API controllers)
- C#
- Docker for containerized deployment

## Project structure

```
Controllers/
  HomeController.cs         Default MVC scaffold (index/privacy/error views)
  InteractionController.cs  Robot-facing endpoint(s) for logging interactions
Models/
  Interaction.cs             A single logged interaction (speech turn)
  Event.cs                   (planned) robot/system events, e.g. status pings
  ErrorViewModel.cs           MVC error view model
Services/
  IDirectionService.cs       (planned) interface for directions/wayfinding logic
  DirectionsService.cs        (planned) implementation
Views/                        Razor views for the default MVC scaffold
Dockerfile
Program.cs                    App startup / middleware pipeline
appsettings*.json             Configuration
```

## Running locally

Prerequisites: [.NET 9 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet restore
dotnet run
```

By default this launches on `http://localhost:5270` (see
[Properties/launchSettings.json](Properties/launchSettings.json) for the HTTPS
profile and ports).

## Running with Docker

```bash
docker build -t guidebot-api .
docker run -p 8080:8080 guidebot-api
```

The container listens on port `8080` (`ASPNETCORE_HTTP_PORTS=8080`).

## Authentication

Endpoints intended for use by the robot (e.g. `LogInteraction`) are marked
`[Authorize]`. Authentication scheme/configuration is not yet wired up in
`Program.cs`.

## Context

Built for the [UNR Robotics Research Lab](https://github.com/UNR-RoboticsResearchLab)
to support an in-person HRI study with a guide robot deployed on a college campus.
