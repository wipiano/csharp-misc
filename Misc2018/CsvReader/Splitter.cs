using System;

namespace CsvReader
{
    // 写経: https://github.com/ufcpp/UfcppSample/blob/master/Demo/2018/IOPerformance/App/Splitter.cs
    internal readonly ref struct Splitter
    {
        private readonly Span<byte> _data;
        private readonly Span<int> _indexes;

        public Splitter(Span<byte> data, Span<int> indexBuffer, ReadOnlySpan<byte> delimiter)
        {
            _data = data;
            _indexes = indexBuffer;

            var index = 0;

            for (int i = 0; i < indexBuffer.Length; i++)
            {
                if (index != data.Length)
                {
                    // 次の delimiter の位置をさがす
                    var next = data.Slice(index + 1).IndexOf(delimiter);
                    index = next < 0 ? data.Length : index + 1 + next;
                }
                
                // indexBuffer に追加
                indexBuffer[i] = index;
            }
        }

        public Span<byte> this[int index]
        {
            get
            {
                if ((uint) index >= _indexes.Length) throw new IndexOutOfRangeException();
                var start = index == 0 ? 0 : _indexes[index - 1] + 1;
                var end = _indexes[index];
                if (end <= start) return default;
                // impure method called ??? rider のバグぽい?
                return _data.Slice(start, end - start);
            }
        }

    }
}