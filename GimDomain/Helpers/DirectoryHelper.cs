using System.IO;

namespace Gim.Domain.Helpers
{
    public class DirectoryHelper
    {
        private static readonly string separator
            = Path.DirectorySeparatorChar.ToString();

        public static bool IsSameOrSub(string current, string toCheck)
        {
            if (HasNoValue(current, toCheck)) { return false; }

            return IsSame(current, toCheck) || IsSub(current, toCheck);
        }

        public static bool IsSame(string current, string toCheck)
        {
            if (HasNoValue(current)) { return false; }

            return current.Equals(toCheck);
        }

        public static bool IsSub(string current, string toCheck)
        {
            if (HasNoValue(toCheck)) { return false; }

            return toCheck.StartsWith(current);
        }

        private static bool HasNoValue(string current, string toCheck)
        {
            return HasNoValue(current) || HasNoValue(toCheck);
        }

        private static bool HasNoValue(string value)
        {
            return string.IsNullOrEmpty(value)
                || string.IsNullOrWhiteSpace(value);
        }
    }
}
