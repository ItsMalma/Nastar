namespace Nastar.Routing;

internal class Router : IRouter, IPrefixedRouter
{
    private readonly Route _root = new()
    {
        Path = "/"
    };

    private string _prefixPath = "/";

    public IPrefixedRouter Prefix(string path)
    {
        _prefixPath = path;
        return this;
    }

    public IRouter Get(string path, RouteHandler handler)
    {
        _root.Insert("GET", path, handler);
        return this;
    }

    public IPrefixedRouter Get(RouteHandler handler)
    {
        Get(_prefixPath, handler);
        return this;
    }

    public IRouter Post(string path, RouteHandler handler)
    {
        _root.Insert("POST", path, handler);
        return this;
    }

    public IPrefixedRouter Post(RouteHandler handler)
    {
        Post(_prefixPath, handler);
        return this;
    }

    public IRouter Put(string path, RouteHandler handler)
    {
        _root.Insert("PUT", path, handler);
        return this;
    }

    public IPrefixedRouter Put(RouteHandler handler)
    {
        Put(_prefixPath, handler);
        return this;
    }

    public IRouter Patch(string path, RouteHandler handler)
    {
        _root.Insert("PATCH", path, handler);
        return this;
    }

    public IPrefixedRouter Patch(RouteHandler handler)
    {
        Patch(_prefixPath, handler);
        return this;
    }

    public IRouter Delete(string path, RouteHandler handler)
    {
        _root.Insert("DELETE", path, handler);
        return this;
    }

    public IPrefixedRouter Delete(RouteHandler handler)
    {
        Delete(_prefixPath, handler);
        return this;
    }

    public IRouter Head(string path, RouteHandler handler)
    {
        _root.Insert("HEAD", path, handler);
        return this;
    }

    public IPrefixedRouter Head(RouteHandler handler)
    {
        Head(_prefixPath, handler);
        return this;
    }

    public IRouter Options(string path, RouteHandler handler)
    {
        _root.Insert("OPTIONS", path, handler);
        return this;
    }

    public IPrefixedRouter Options(RouteHandler handler)
    {
        Options(_prefixPath, handler);
        return this;
    }

    public IRouter Trace(string path, RouteHandler handler)
    {
        _root.Insert("TRACE", path, handler);
        return this;
    }

    public IPrefixedRouter Trace(RouteHandler handler)
    {
        Trace(_prefixPath, handler);
        return this;
    }

    public IRouter Connect(string path, RouteHandler handler)
    {
        _root.Insert("CONNECT", path, handler);
        return this;
    }

    public IPrefixedRouter Connect(RouteHandler handler)
    {
        Connect(_prefixPath, handler);
        return this;
    }

    public IRouter All(string path, RouteHandler handler)
    {
        _root.Insert("*", path, handler);
        return this;
    }

    public IPrefixedRouter All(RouteHandler handler)
    {
        All(_prefixPath, handler);
        return this;
    }

    internal RoutingResult? Match(string method, string path)
    {
        return _root.Search(method, path);
    }
}