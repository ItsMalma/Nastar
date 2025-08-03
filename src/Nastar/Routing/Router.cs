namespace Nastar.Routing;

internal class Router
{
    private readonly RouteNode _root = new()
    {
        Path = "/"
    };

    public void Add(string path, Route route)
    {
        _root.Insert(path, route);
    }

    public Route? Match(string path, out Dictionary<string, string> parameters)
    {
        return _root.Search(path, out parameters);
    }
}