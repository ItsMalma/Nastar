using Nastar;

NastarApp app = new();
app.Get("/", () => Task.FromResult("Hello, World!"));
app.Run();