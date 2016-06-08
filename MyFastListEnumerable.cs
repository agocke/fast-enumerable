using System.Collections;
using System.Runtime.CompilerServices;

namespace FastList
{
    public class MyFastListEnumerable<T> : IFastEnumerable<T, int>
    {
        private readonly T[] _backing;

        public MyFastListEnumerable(int count)
        {
            _backing = new T[count];
        }

        public int Start => 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T TryGetNext(ref int enumerator, out bool remaining)
        {
            if (enumerator < _backing.Length)
            {
                remaining = true;
                return _backing[enumerator++];
            }
            remaining = false;
            return default(T);
        }

        public T this[int index]
        {
            get { return _backing[index]; }
            set { _backing[index] = value; }
        }
    }
}