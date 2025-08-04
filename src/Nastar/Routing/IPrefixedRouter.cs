namespace Nastar.Routing;

/// <summary>
/// 
/// </summary>
public interface IPrefixedRouter
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IPrefixedRouter Get(RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IPrefixedRouter Post(RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IPrefixedRouter Put(RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IPrefixedRouter Patch(RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IPrefixedRouter Delete(RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IPrefixedRouter Head(RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IPrefixedRouter Options(RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IPrefixedRouter Trace(RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IPrefixedRouter Connect(RouteHandler handler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handler"></param>
    /// <returns></returns>
    public IPrefixedRouter All(RouteHandler handler);
}