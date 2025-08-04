using System.Net;
using System.Text;
using System.Text.Json;

using Nastar.Routing;

namespace Nastar;

/// <summary>
/// 
/// </summary>
public class NastarApp
{
    /// <summary>
    /// 
    /// </summary>
    public string Host { get; set; } = "localhost";

    /// <summary>
    /// 
    /// </summary>
    public int Port { get; set; } = 3000;

    /// <summary>
    /// 
    /// </summary>
    public int MaxConcurrency { get; set; } = 100;

    /// <summary>
    /// 
    /// </summary>
    public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(10);

    private readonly HttpListener _listener = new();

    private readonly Router _router = new();

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private SemaphoreSlim _concurrencyLimiter;

    /// <summary>
    /// 
    /// </summary>
    public NastarApp()
    {
        _concurrencyLimiter = new(MaxConcurrency);
    }

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

    private object? Dispatch(HttpListenerRequest request)
    {
        Uri url = request.Url!;
        string path = url.AbsolutePath;

        Route? route = _router.Match(path, out Dictionary<string, string> parameters);

        return route != null && route.Method.Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase)
            ? route.Handler(request)
            : null;
    }

    private async Task Filter(HttpListenerResponse response, object? result, CancellationToken cancellationToken)
    {
        response.ContentEncoding = Encoding.UTF8;
        response.StatusCode = 200;

        if (result is string stringResult)
        {
            byte[] encodedStringResult = Encoding.UTF8.GetBytes(stringResult);

            response.ContentType = "text/plain";
            response.ContentLength64 = encodedStringResult.Length;
            await response.OutputStream.WriteAsync(encodedStringResult, cancellationToken);
        }
        else if (result != null)
        {
            string serializedResult = JsonSerializer.Serialize(result);
            byte[] encodedResult = Encoding.UTF8.GetBytes(serializedResult);

            response.ContentType = "application/json";
            response.ContentLength64 = encodedResult.Length;
            await response.OutputStream.WriteAsync(encodedResult, cancellationToken);
        }
        else
        {
            byte[] encodedResult = Encoding.UTF8.GetBytes("Not Found");

            response.StatusCode = 404;
            response.ContentType = "text/plain";
            response.ContentLength64 = encodedResult.Length;
            await response.OutputStream.WriteAsync(encodedResult, cancellationToken);
        }
    }

    private async Task HandleRequest(HttpListenerContext context)
    {
        await _concurrencyLimiter.WaitAsync(_cancellationTokenSource.Token);

        try
        {
            CancellationTokenSource timeoutCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token);
            timeoutCancellationTokenSource.CancelAfter(RequestTimeout);

            await Filter(context.Response, Dispatch(context.Request), timeoutCancellationTokenSource.Token)
                .WaitAsync(timeoutCancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            byte[] encodedResult = Encoding.UTF8.GetBytes("Request Timeout");

            context.Response.StatusCode = 408;
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.ContentType = "text/plain";
            context.Response.ContentLength64 = encodedResult.Length;
            await context.Response.OutputStream.WriteAsync(encodedResult, _cancellationTokenSource.Token);
        }
        catch (Exception)
        {
            byte[] encodedResult = Encoding.UTF8.GetBytes("Internal Server Error");

            context.Response.StatusCode = 500;
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.ContentType = "text/plain";
            context.Response.ContentLength64 = encodedResult.Length;
            await context.Response.OutputStream.WriteAsync(encodedResult, _cancellationTokenSource.Token);
        }
        finally
        {
            context.Response.Close();
            _concurrencyLimiter.Release();
        }
    }

    private async Task HandleRequests()
    {
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                HttpListenerContext context = await _listener.GetContextAsync();

                _ = Task.Run(async () => await HandleRequest(context), _cancellationTokenSource.Token);
            }
            catch (HttpListenerException ex) when (ex.ErrorCode == 995)
            {
                break;
            }
            catch (ObjectDisposedException)
            {
                break;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Run()
    {
        try
        {
            _concurrencyLimiter = new(MaxConcurrency);

            _listener.Prefixes.Add($"http://{Host}:{Port}/");
            _listener.Start();

            HandleRequests().GetAwaiter().GetResult();
        }
        finally
        {
            _cancellationTokenSource.Cancel();

            _listener.Stop();
            _listener.Close();

            _concurrencyLimiter.Dispose();
            _cancellationTokenSource.Dispose();
        }
    }
}