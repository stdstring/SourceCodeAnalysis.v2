namespace SourceCodeCheckApp.Utils
{
    internal static class EnumerableUtils
    {
        public static Boolean IsEmpty<TElement>(this IEnumerable<TElement> source)
        {
            return !source.Any();
        }

        public static Boolean IsEmpty<TElement>(this ICollection<TElement> source)
        {
            return source.Count == 0;
        }
    }
}
