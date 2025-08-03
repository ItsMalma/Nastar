namespace Nastar.Routing;

/// <summary>
/// 
/// </summary>
public class Route(string method, RouteHandler handler)
{
    /// <summary>
    /// 
    /// </summary>
    public string Method { get; set; } = method;

    /// <summary>
    /// 
    /// </summary>
    public RouteHandler Handler { get; set; } = handler;
}