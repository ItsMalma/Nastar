namespace Nastar.Routing;

internal class Router
{
    private readonly Route _root = new()
    {
        Path = "/"
    };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Get(string path, RouteHandler handler)
    {
        _root.Insert("GET", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Post(string path, RouteHandler handler)
    {
        _root.Insert("POST", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Put(string path, RouteHandler handler)
    {
        _root.Insert("PUT", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Patch(string path, RouteHandler handler)
    {
        _root.Insert("PATCH", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Delete(string path, RouteHandler handler)
    {
        _root.Insert("DELETE", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Options(string path, RouteHandler handler)
    {
        _root.Insert("OPTIONS", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Head(string path, RouteHandler handler)
    {
        _root.Insert("HEAD", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Trace(string path, RouteHandler handler)
    {
        _root.Insert("TRACE", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void Connect(string path, RouteHandler handler)
    {
        _root.Insert("CONNECT", path, handler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    public void All(string path, RouteHandler handler)
    {
        _root.Insert("*", path, handler);
    }

    public RouteHandler? Match(string method, string path, out Dictionary<string, string> parameters)
    {
        return _root.Search(method, path, out parameters);
    }
}