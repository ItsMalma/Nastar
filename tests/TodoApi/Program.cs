using Nastar;

NastarApp app = new();
app.Get("/text", (request) => "Hello, World!");
app.Get("/json", (request) => new { Message = "Hello, World!" });
app.Run();
