namespace System.Collections
{
    public interface IFastEnumerator<T>
    {
        T TryGetNext(out bool remaining);

        void Reset();
    }
}