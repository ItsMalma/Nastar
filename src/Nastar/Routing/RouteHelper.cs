using System.Text.RegularExpressions;

namespace Nastar.Routing;

internal static partial class RouteHelper
{
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

    [GeneratedRegex(@"^\:.+?\{(.+)\}$")]
    private static partial Regex GetParameterPatternRegex();

    internal static string GetParameterPattern(string path)
    {
        Match match = GetParameterPatternRegex().Match(path);

        return match.Success
            ? $"({match.Value})"
            : "(.+)";
    }

    [GeneratedRegex(@"^\:([^\{\}]+)")]
    private static partial Regex GetParameterNameRegex();

    internal static string GetParameterName(string path)
    {
        return GetParameterNameRegex().Match(path).Value;
    }
}