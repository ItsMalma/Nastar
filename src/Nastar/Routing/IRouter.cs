namespace Nastar.Routing;

/// <summary>
/// 
/// </summary>
public interface IRouter
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public IPrefixedRouter Prefix(string path);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IRouter Get(string path, RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IRouter Post(string path, RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IRouter Put(string path, RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IRouter Patch(string path, RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IRouter Delete(string path, RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IRouter Head(string path, RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IRouter Options(string path, RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IRouter Trace(string path, RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IRouter Connect(string path, RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IRouter All(string path, RouteHandler handler);
}