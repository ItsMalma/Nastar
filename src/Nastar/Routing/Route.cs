using System.Text.RegularExpressions;

namespace Nastar.Routing;

internal partial class Route
{
    internal string Path { get; set; } = string.Empty;

    internal Dictionary<string, RouteHandler> Handlers { get; set; } = [];

    internal Dictionary<string, Route> Children { get; set; } = [];

    internal void Insert(string method, string path, RouteHandler handler)
    {
        Route current = this;

        if (path == "/")
        {
            current.Path = path;
            current.Handlers[method] = handler;
        }

        List<string> segments = RouteHelper.SplitPath(path);

        foreach (string segment in segments)
        {
            if (current.Children.TryGetValue(segment, out Route? next) && next != null)
            {
                current = next;
            }
            else
            {
                current.Children[segment] = new()
                {
                    Path = segment,
                    Handlers = {
                        [method] = handler
                    },
                    Children = [],
                };
                current = current.Children[segment];
            }
        }

        if (!current.Handlers.ContainsKey(method))
        {
            current.Handlers[method] = handler;
        }
    }

    internal RoutingResult? Search(string method, string path)
    {
        Route current = this;
        Dictionary<string, string> parameters = [];

        foreach (string segment in RouteHelper.SplitPath(path))
        {
            if (current.Children.TryGetValue(segment, out Route? next) && next != null)
            {
                current = next;
                continue;
            }

            if (current.Children.Count == 0)
            {
                if (current.Path != segment)
                {
                    return null;
                }
                break;
            }

            bool isParameterMatch = false;

            foreach (string key in current.Children.Keys)
            {
                if (key == "*")
                {
                    current = current.Children[key];
                    isParameterMatch = true;
                    break;
                }

                if (key.StartsWith(':'))
                {
                    Regex regexPattern = new(RouteHelper.GetParameterPattern(key));
                    Match match = regexPattern.Match(segment);

                    if (match.Success)
                    {
                        parameters[RouteHelper.GetParameterName(key)] = match.Value;
                        current = current.Children[key];
                        isParameterMatch = true;
                        break;
                    }

                    return null;
                }
            }

            if (!isParameterMatch)
            {
                return null;
            }
        }

        RouteHandler? handler = current.Handlers.GetValueOrDefault("*")
            ?? current.Handlers.FirstOrDefault(h => h.Key.Equals(method, StringComparison.OrdinalIgnoreCase)).Value;

        return handler != null
            ? new(handler, parameters)
            : null;
    }
}