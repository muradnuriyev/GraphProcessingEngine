# Graph Processing Engine

Interactive graph playground built with C# 12 / .NET 8. The solution includes a reusable core library, a console CLI, a minimal Web API, and a WPF preview shell.

## Prerequisites
- .NET 8 SDK (and .NET 8 runtime for running tests/hosted apps). If you only have .NET 10 installed, install the .NET 8 runtime/SDK from https://dotnet.microsoft.com/download.
- Windows for WPF; CLI/WebApi/Core also run cross-platform.

## Project layout
- `src/GraphProcessingEngine.Core` – graph models, builder, BFS/DFS, Dijkstra/A*, heap priority queue, caching decorator, DTO mapper, JSON repository.
- `src/GraphProcessingEngine.Cli` – commands: create graph, add edge, list graphs, run shortest path.
- `src/GraphProcessingEngine.WebApi` – minimal API with Swagger for creating graphs and running shortest paths.
- `src/GraphProcessingEngine.Gui.Wpf` – MVVM WPF shell that renders a demo graph.
- `tests/GraphProcessingEngine.Core.Tests` – xUnit coverage for core primitives and persistence.

## Build & test (step-by-step)
1) Restore/build everything:
```bash
dotnet build GraphProcessingEngine.slnx
```
2) Run tests (requires .NET 8 runtime):
```bash
dotnet test GraphProcessingEngine.slnx
```

## Run components
### CLI
```bash
# create a directed graph named demo
dotnet run --project src/GraphProcessingEngine.Cli -- create demo --directed
# add edge A -> B with weight 1
dotnet run --project src/GraphProcessingEngine.Cli -- add-edge demo A B 1
# shortest path (Dijkstra by default)
dotnet run --project src/GraphProcessingEngine.Cli -- shortest-path demo A B --algorithm=dijkstra
# list saved graphs (stored under ./graphs)
dotnet run --project src/GraphProcessingEngine.Cli -- list
```
Graphs persist as JSON under `./graphs` (ignored by git).

### Web API
```bash
dotnet run --project src/GraphProcessingEngine.WebApi
```
Open Swagger UI at the printed URL (defaults to `http://localhost:5xxx/swagger`). Sample requests live in `src/GraphProcessingEngine.WebApi/GraphProcessingEngine.WebApi.http`.
Data files persist under `./data` (ignored by git).

### WPF preview
```bash
dotnet run --project src/GraphProcessingEngine.Gui.Wpf
```
Shows a demo graph rendered on a canvas. Hook it up to Core/Repository services to make it fully interactive.

## Deployment (publishing)
- CLI single-folder publish:
```bash
dotnet publish src/GraphProcessingEngine.Cli -c Release -o publish/cli
```
- Web API (self-contained win-x64 example):
```bash
dotnet publish src/GraphProcessingEngine.WebApi -c Release -r win-x64 --self-contained true -o publish/webapi
```
- WPF app (win-x64):
```bash
dotnet publish src/GraphProcessingEngine.Gui.Wpf -c Release -r win-x64 --self-contained true -o publish/wpf
```

## Git quickstart
```bash
git init
git add .
git commit -m "Initial graph engine scaffold"
# git remote add origin <your-repo-url>
# git push -u origin main
```

## Notes
- If tests complain about missing runtime, install .NET 8 runtime/SDK.
- Local graph data (`graphs/`, `data/`) are ignored by git to keep your repo clean.
