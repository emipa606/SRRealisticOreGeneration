namespace RabiSquare.RealisticOreGeneration;

public class BaseSingleTon<T> where T : class, new()
{
    public static T Instance => Inner.InternalInstance;

    private static class Inner
    {
        internal static readonly T InternalInstance = new T();
    }
}