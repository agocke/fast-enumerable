using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FastList;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Name
{
    public class Program
    {
        private readonly int[] _array;
        private readonly MyFastListEnumerable<int> _array2;
        private readonly MyFastListEnumerator<int> _array3;
        
        public Program()
        {
            const int count = 200000;
            _array = new int[count];
            _array2 = new MyFastListEnumerable<int>(count);
            _array3 = new MyFastListEnumerator<int>(count);
            for (int i = 0; i < count; i++)
            {
                var value = (i + 1) * ((i & 1) == 0 ? 1 : -1); // +1/-1 to avoid Sum overflow

                _array[i] = value;
                _array2[i] = _array3[i] = value;
            } 
        }

        public static void Main()
        {
            var test = new Program();
            Console.Out.WriteLine(test.CisternLinq());
            Console.Out.WriteLine(test.SystemLinq());
            Console.Out.WriteLine(test.ForLoop());
            Console.Out.WriteLine(test.ForEachLoop());
            Console.Out.WriteLine(test.FastEnumerable());
            Console.Out.WriteLine(test.IFastEnumerable());
            Console.Out.WriteLine(test.IFastEnumerableGeneric());
            Console.Out.WriteLine(test.IFastEnumerator());
            Console.Out.WriteLine(test.IFastEnumeratorGeneric());
            Console.Out.WriteLine(test.MyListFastEnumerable());
            Console.Out.WriteLine(test.MyListFastEnumerator());

            var summary = BenchmarkRunner.Run<Program>();
        }
        
        [Benchmark]
        public long ForLoop()
        {
            long total = 0;
            for (int i = 0; i < _array.Length; i++)
            {
                total += _array[i];
            }
            return total;
        }

        [Benchmark]
        public long ForEachLoop()
        {
            long total = 0;
            foreach (var i in _array)
            {
                total += i;
            }
            return total;
        }

        [Benchmark]
        public long CisternLinq() => Cistern.Linq.Enumerable.Sum(_array);


        [Benchmark]
        public long SystemLinq() => System.Linq.Enumerable.Sum(_array);

        [Benchmark]
        public long ForeachIEnumerable()
        {
            IEnumerable<int> ieList = _array;
            long total = 0;
            var enumerator = ieList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                total += enumerator.Current;
            }
            return total;
        }

        [Benchmark]
        public long FastEnumerable()
        {
            var afeList = _array.GetFastEnumerable();
            long total = 0;
            var enumerator = afeList.Start;
            bool remaining = true;
            loop:
            var i = afeList.TryGetNext(ref enumerator, out remaining);
            if (remaining)
            {
                total += i;
                goto loop;
            }
            return total;
        }

        [Benchmark]
        public long MyListFastEnumerable()
        {
            long total = 0;
            var enumerator = _array2.Start;
            bool remaining = true;
            loop:
            var i = _array2.TryGetNext(ref enumerator, out remaining);
            if (remaining)
            {
                total += i;
                goto loop;
            }
            return total;
        }

        [Benchmark]
        public long IFastEnumerable()
        {
            IFastEnumerable<int, int> ife = _array.GetFastEnumerable();
            long total = 0;
            var enumerator = ife.Start;
            bool remaining = true;
            loop:
            var i = ife.TryGetNext(ref enumerator, out remaining);
            if (remaining)
            {
                total += i;
                goto loop;
            }
            return total;
        }

        [Benchmark] 
        public long IFastEnumerableGeneric() => IfeHelper<ArrayFastEnumerable<int>, int>(_array.GetFastEnumerable());

        private long IfeHelper<IFE, TEnum>(IFE ife) where IFE : IFastEnumerable<int, TEnum>
        {
            long total = 0;
            var enumerator = ife.Start;
            bool remaining = true;
            loop:
            var i = ife.TryGetNext(ref enumerator, out remaining);
            if (remaining)
            {
                total += i;
                goto loop;
            }
            return total;
        }

        [Benchmark]
        public long FastEnumerator()
        {
            long total = 0;
            var enumerator = _array.GetFastEnumerator();
            bool remaining = true;
            enumerator.Reset();
            loop:
            var i = enumerator.TryGetNext(out remaining);
            if (remaining)
            {
                total += i;
                goto loop;
            }
            return total;
        }

        [Benchmark]
        public long MyListFastEnumerator()
        {
            long total = 0;
            var enumerator = _array3.Enumerator;
            bool remaining = true;
            enumerator.Reset();
            loop:
            var i = enumerator.TryGetNext(out remaining);
            if (remaining)
            {
                total += i;
                goto loop;
            }
            return total;
        }

        [Benchmark]
        public long IFastEnumerator()
        {
            IFastEnumerator<int> ife = _array.GetFastEnumerator();
            long total = 0;
            bool remaining = true;
            ife.Reset();
            loop:
            var i = ife.TryGetNext(out remaining);
            if (remaining)
            {
                total += i;
                goto loop;
            }
            return total;
        }

        [Benchmark]
        public long IFastEnumeratorGeneric()
        {
            var e = _array.GetFastEnumerator();
            return IfeHelper2(ref e);
        }
        
        private long IfeHelper2<TEnum>(ref TEnum fastEnum) where TEnum : IFastEnumerator<int>
        {
            long total = 0;
            bool remaining = true;
            fastEnum.Reset();
            loop:
            var i = fastEnum.TryGetNext(out remaining);
            if (remaining)
            {
                total += i;
                goto loop;
            }
            return total;
        }
    }
}