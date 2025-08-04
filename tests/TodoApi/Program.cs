using Nastar;

var app = new NastarApp();
app.Get("/version", (request) => "1.0.0");

var todosRouter = app.Prefix("/todos");
todosRouter.Post((request) => "Todo created");
todosRouter.Get((request) => "List of todos");
todosRouter.Put((request) => "Todo updated");
todosRouter.Delete((request) => "Todo deleted");

app.Run();
