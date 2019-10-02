using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace System.Collections
{
    public static class ArrayExtensions
    {
        public static ArrayFastEnumerable<T> GetFastEnumerable<T>(this T[] array)
        {
            return new ArrayFastEnumerable<T>(array);
        }
        
        public static ArrayFastEnumerator<T> GetFastEnumerator<T>(this T[] list) =>
            new ArrayFastEnumerator<T>(list);
    }
    
    public struct ArrayFastEnumerable<T> : IFastEnumerable<T, int>
    {
        private readonly T[] _backing;
        
        public ArrayFastEnumerable(T[] backing)
        {
            _backing = backing;
        }
        
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
        
        public int Start => 0;
    }
}