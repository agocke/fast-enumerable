using System.Collections;
using System.Runtime.CompilerServices;

namespace FastList
{
    public class MyFastListEnumerator<T>
    {
        private readonly T[] _backing;

        public MyFastListEnumerator(int count)
        {
            _backing = new T[count];
        }

        public struct FastListEnumerator : IFastEnumerator<T>
        {
            private int _index;
            private readonly T[] _backing;

            public FastListEnumerator(T[] backing)
            {
                _index = 0;
                _backing = backing;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T TryGetNext(out bool remaining)
            {
                if (_index < _backing.Length)
                {
                    remaining = true;
                    return _backing[_index++];
                }
                remaining = false;
                return default(T);
            }

            public void Reset() => _index = 0;
        }

        public FastListEnumerator Enumerator => new FastListEnumerator(_backing);

        public T this[int index]
        {
            get { return _backing[index]; }
            set { _backing[index] = value; }
        }
    }
}
