# Nastar

Minimal and easy to use web api framework for C#.

## Install

```sh
dotnet add package Nastar
```

## Hello World

```csharp
using Nastar;

var app = new NastarApp()
   .Post("/text", (request) => "Hello, World!")
   .Get("/json", (request) => new { Message = "Hello, World!" });

app.Run();
```
