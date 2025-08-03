using System.Net;
using System.Text;

using Nastar.Routing;

namespace Nastar;

/// <summary>
/// 
/// </summary>
public class NastarApp
{
    private readonly HttpListener _listener = new();

    private readonly Router _router = new();

    /// <summary>
    /// 
    /// </summary>
    public string Host { get; set; } = "localhost";

    /// <summary>
    /// 
    /// </summary>
    public int Port { get; set; } = 3000;

    private void AddRoute(string method, string path, RouteHandler handler)
    {
        _router.Add(path, new(method, handler));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Get(string path, RouteHandler handler)
    {
        AddRoute("GET", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Post(string path, RouteHandler handler)
    {
        AddRoute("POST", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Put(string path, RouteHandler handler)
    {
        AddRoute("PUT", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Patch(string path, RouteHandler handler)
    {
        AddRoute("PATCH", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Delete(string path, RouteHandler handler)
    {
        AddRoute("DELETE", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Options(string path, RouteHandler handler)
    {
        AddRoute("OPTIONS", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Head(string path, RouteHandler handler)
    {
        AddRoute("HEAD", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Trace(string path, RouteHandler handler)
    {
        AddRoute("TRACE", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Connect(string path, RouteHandler handler)
    {
        AddRoute("CONNECT", path, handler);
    }

    private async Task Dispatch(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        response.ContentType = "text/plain";
        response.ContentEncoding = Encoding.UTF8;

        Uri url = request.Url!;
        string path = url.AbsolutePath;

        Route? route = _router.Match(path, out Dictionary<string, string> parameters);

        string responseText = "OK";

        if (route != null && route.Method.Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase))
        {
            responseText = await route.Handler();
            response.StatusCode = 200;
        }
        else
        {
            responseText = await NotFound();
            response.StatusCode = 404;
        }

        response.ContentLength64 = responseText.Length;

        await response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(responseText).AsMemory(0, responseText.Length));
    }

    private async Task Handle()
    {
        while (true)
        {
            HttpListenerContext context = await _listener.GetContextAsync();

            await Dispatch(context);

            context.Response.Close();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Run()
    {
        _listener.Prefixes.Add($"http://{Host}:{Port}/");
        _listener.Start();

        Task[] tasks = new Task[Environment.ProcessorCount];
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Handle();
        }
        Task.WaitAll(tasks);

        _listener.Close();
    }

    private static Task<string> NotFound()
    {
        return Task.FromResult("Not Found");
    }
}