using System.Text.RegularExpressions;

namespace Nastar.Routing;

internal partial class RouteNode
{
    internal string Path { get; set; } = string.Empty;

    internal Route? Route { get; set; }

    internal Dictionary<string, RouteNode> Children { get; set; } = [];

    internal void Insert(string path, Route handler)
    {
        RouteNode current = this;

        if (path == "/")
        {
            current.Path = path;
            current.Route = handler;
        }

        List<string> segments = SplitPath(path);

        foreach (string segment in segments)
        {
            if (current.Children.TryGetValue(segment, out RouteNode? next) && next != null)
            {
                current = next;
            }
            else
            {
                current.Children[segment] = new()
                {
                    Path = segment,
                    Route = handler,
                    Children = []
                };
                current = current.Children[segment];
            }
        }
    }

    internal Route? Search(string path, out Dictionary<string, string> parameters)
    {
        RouteNode current = this;
        parameters = [];

        foreach (string segment in SplitPath(path))
        {
            if (current.Children.TryGetValue(segment, out RouteNode? next) && next != null)
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
                    Regex regexPattern = GetRegexPattern(key);
                    Match match = regexPattern.Match(segment);

                    if (match.Success)
                    {
                        parameters[GetParameterName(key)] = match.Value;
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

        return current.Route;
    }

    [GeneratedRegex(@"^\:.+?\{(.+)\}$")]
    internal static partial Regex SearchPathPatternRegex();

    [GeneratedRegex(@"(.+)")]
    internal static partial Regex NoPatternPathRegex();

    internal static Regex GetRegexPattern(string path)
    {
        Match match = SearchPathPatternRegex().Match(path);

        return match.Success
            ? new($"({match.Value})")
            : NoPatternPathRegex();
    }

    [GeneratedRegex(@"^\:([^\{\}]+)")]
    internal static partial Regex SearchParameterNameRegex();

    internal static string GetParameterName(string path)
    {
        return SearchParameterNameRegex().Match(path).Value;
    }

    internal static List<string> SplitPath(string path)
    {
        List<string> segments = [];

        foreach (string segment in path.Split('/'))
        {
            if (segment.Length > 0)
            {
                segments.Add(segment);
            }
        }

        return segments;
    }
}