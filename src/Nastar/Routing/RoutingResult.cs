namespace Nastar.Routing;

internal record RoutingResult
(
    RouteHandler Handler,
    Dictionary<string, string> Parameters
);