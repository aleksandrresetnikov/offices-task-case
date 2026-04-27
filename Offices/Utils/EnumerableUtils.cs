namespace Offices.Utils;

public static class EnumerableUtils
{
    public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (action == null) throw new ArgumentNullException(nameof(action));

        foreach (T item in source)
        {
            action(item);
        }
    }
    
    public static async Task ForeachAsync<T>(this IEnumerable<T> source, Func<T, Task> action, 
        CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (action == null) throw new ArgumentNullException(nameof(action));

        foreach (T item in source)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await action(item);
        }
    }
}