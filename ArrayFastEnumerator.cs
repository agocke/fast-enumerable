using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Collections
{
    public struct ArrayFastEnumerator<T> : IFastEnumerator<T>
    {
        private readonly List<T> _backing;
        private int _index;

        public ArrayFastEnumerator(List<T> backing)
        {
            _backing = backing;
            _index = 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T TryGetNext(out bool remaining)
        {
            if (_index < _backing.Count)
            {
                remaining = true;
                return  _backing[_index++];
            }
            remaining = false;
            return default(T);
        }
    }
}