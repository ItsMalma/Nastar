using Nastar;

var app = new NastarApp()
   .Post("/text", (request) => "Hello, World!")
   .Get("/json", (request) => new { Message = "Hello, World!" });

app.Run();
