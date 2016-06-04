namespace System.Collections
{
    public interface IFastEnumerable<T, TEnum>
    {
        TEnum Start { get; }
        T TryGetNext(ref TEnum enumerator, out bool remaining);
    }
}
