# Nastar

Minimal and easy to use web api framework for C#.

## Install

```sh
dotnet add package Nastar
```

## Hello World

```csharp
using Nastar;

NastarApp app = new();
app.Get("/", () => Task.FromResult("Hello, World!"));
app.Run();
```
