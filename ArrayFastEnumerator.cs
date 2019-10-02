using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Collections
{
    public struct ArrayFastEnumerator<T> : IFastEnumerator<T>
    {
        private readonly T[] _backing;
        private int _index;

        public ArrayFastEnumerator(T[] backing)
        {
            _backing = backing;
            _index = 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T TryGetNext(out bool remaining)
        {
            if (_index < _backing.Length)
            {
                remaining = true;
                return  _backing[_index++];
            }
            remaining = false;
            return default(T);
        }

        public void Reset() => _index = 0;
    }
}