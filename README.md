# Graph Processing Engine

Interactive playground for building, visualizing, and analyzing graphs with C# 12 / .NET 8. The solution includes a reusable core library, CLI tools, a minimal Web API, and a WPF preview shell.

## Tech stack
- Language/Runtime: C# 12, .NET 8
- UI: WPF (MVVM shell for Windows)
- API: ASP.NET Core Minimal API with Swagger
- Tooling: xUnit tests, custom binary-heap priority queue
- Serialization/Storage: JSON DTOs + file repository

## Solution layout
- `src/GraphProcessingEngine.Core` — graph models, builder, BFS/DFS, Dijkstra/A*, heap PQ, caching decorator, DTO mapper, JSON repository.
- `src/GraphProcessingEngine.Cli` — commands to create graphs, add edges, list graphs, and run shortest-path queries.
- `src/GraphProcessingEngine.WebApi` — minimal API with Swagger for graph CRUD/paths.
- `src/GraphProcessingEngine.Gui.Wpf` — MVVM WPF shell that renders a demo graph.
- `tests/GraphProcessingEngine.Core.Tests` — xUnit coverage for core primitives and persistence.

## Prerequisites
- .NET 8 SDK (and runtime to run apps/tests). If only .NET 10 is installed, grab .NET 8 from https://dotnet.microsoft.com/download.
- Windows for the WPF app; Core/CLI/WebApi are cross-platform.

## Build & test
```bash
# restore + build everything
dotnet build GraphProcessingEngine.slnx

# run tests (needs .NET 8 runtime)
dotnet test GraphProcessingEngine.slnx
```

## Run the components
### CLI
```bash
dotnet run --project src/GraphProcessingEngine.Cli -- create demo --directed
dotnet run --project src/GraphProcessingEngine.Cli -- add-edge demo A B 1
dotnet run --project src/GraphProcessingEngine.Cli -- shortest-path demo A B --algorithm=dijkstra
dotnet run --project src/GraphProcessingEngine.Cli -- list
```
Graphs persist as JSON under `./graphs` (ignored by git).

### Web API
```bash
dotnet run --project src/GraphProcessingEngine.WebApi
```
Open Swagger at the printed URL (defaults to `http://localhost:5xxx/swagger`). Sample HTTP calls: `src/GraphProcessingEngine.WebApi/GraphProcessingEngine.WebApi.http`. Data files persist under `./data` (ignored by git).

### WPF preview
```bash
dotnet run --project src/GraphProcessingEngine.Gui.Wpf
```
Shows a demo graph on a canvas. Wire it to the repository/core services to make it fully interactive.

## Publish (deployment examples)
- CLI single-folder:
```bash
dotnet publish src/GraphProcessingEngine.Cli -c Release -o publish/cli
```
- Web API self-contained (win-x64 example):
```bash
dotnet publish src/GraphProcessingEngine.WebApi -c Release -r win-x64 --self-contained true -o publish/webapi
```
- WPF app (win-x64):
```bash
dotnet publish src/GraphProcessingEngine.Gui.Wpf -c Release -r win-x64 --self-contained true -o publish/wpf
```
