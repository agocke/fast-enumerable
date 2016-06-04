using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Name
{
    public class Program
    {
        private readonly List<int> _list;
        
        public Program()
        {
            var count = 200000;
            _list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                _list.Add(i + 1);
            } 
        }

        public static void Main()
        {
            var test = new Program();
            Console.Out.WriteLine(test.ForLoop());
            Console.Out.WriteLine(test.FastEnumerable());
            Console.Out.WriteLine(test.IFastEnumerable());
            Console.Out.WriteLine(test.IFastEnumerableGeneric());
            Console.Out.WriteLine(test.IFastEnumerator());
            Console.Out.WriteLine(test.IFastEnumeratorGeneric());

            var summary = BenchmarkRunner.Run<Program>();
        }
        
        [Benchmark]
        public long ForLoop()
        {
            long total = 0;
            for (int i = 0; i < _list.Count; i++)
            {
                total += _list[i];
            }
            return total;
        }

        [Benchmark]
        public long ForeachIEnumerable()
        {
            IEnumerable<int> ieList = _list;
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
            var afeList = _list.GetFastEnumerable();
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
        public long IFastEnumerable()
        {
            IFastEnumerable<int, int> ife = _list.GetFastEnumerable();
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
        public long IFastEnumerableGeneric() => IfeHelper(_list.GetFastEnumerable());

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
            var enumerator = _list.GetFastEnumerator();
            bool remaining = true;
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
            IFastEnumerator<int> ife = _list.GetFastEnumerator();
            long total = 0;
            bool remaining = true;
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
            var e = _list.GetFastEnumerator();
            return IfeHelper2(ref e);
        }
        
        private long IfeHelper2<TEnum>(ref TEnum fastEnum) where TEnum : IFastEnumerator<int>
        {
            long total = 0;
            bool remaining = true;
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