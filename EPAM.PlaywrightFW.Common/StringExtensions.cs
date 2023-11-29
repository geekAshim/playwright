using System.Text.RegularExpressions;

namespace EPAM.PlaywrightFW.Common;

public static class StringExtensions
{
    public static string ExtractPattern(this string source, string regex) => new Regex(regex).Match(source).Value;

}