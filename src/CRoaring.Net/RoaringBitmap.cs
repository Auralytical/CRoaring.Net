using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CRoaring
{
    public class RoaringBitmap : IDisposable, IEnumerable<uint>
    {
        private readonly IntPtr _pointer;
        private bool _isDisposed = false;

        public ulong Cardinality => NativeMethods.roaring_bitmap_get_cardinality(_pointer);
        public bool IsEmpty => NativeMethods.roaring_bitmap_is_empty(_pointer);
        //public int SerializedBytes => NativeMethods.roaring_bitmap_size_in_bytes(_pointer);
        //public int PortableSerializedBytes => NativeMethods.roaring_bitmap_portable_size_in_bytes(_pointer);

        //Creation/Destruction

        public RoaringBitmap()
        {
            _pointer = NativeMethods.roaring_bitmap_create();
        }
        public RoaringBitmap(uint capacity)
        {
            _pointer = NativeMethods.roaring_bitmap_create_with_capacity(capacity);
        }
        private RoaringBitmap(IntPtr pointer)
        {
            _pointer = pointer;
        }

        public static RoaringBitmap FromRange(uint min, uint max, uint step = 1)
            => new RoaringBitmap(NativeMethods.roaring_bitmap_from_range(min, max, step));
        public static RoaringBitmap FromValues(uint[] values)
            => new RoaringBitmap(NativeMethods.roaring_bitmap_of_ptr((uint)values.Length, values));

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                NativeMethods.roaring_bitmap_free(_pointer);
                _isDisposed = true;
            }
        }

        ~RoaringBitmap() { Dispose(false); }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public RoaringBitmap Clone()
        {
            return new RoaringBitmap(NativeMethods.roaring_bitmap_copy(_pointer));
        }

        //List operations

        public void Add(uint value)
            => NativeMethods.roaring_bitmap_add(_pointer, value);
        public void Add(params uint[] values)
        {
            for (int i = 0; i < values.Length; i++)
                NativeMethods.roaring_bitmap_add(_pointer, values[i]);
        }

        public void Remove(uint value)
            => NativeMethods.roaring_bitmap_remove(_pointer, value);
        public void Remove(params uint[] values)
        {
            for (int i = 0; i < values.Length; i++)
                NativeMethods.roaring_bitmap_remove(_pointer, values[i]);
        }

        public bool Contains(uint value)
            => NativeMethods.roaring_bitmap_contains(_pointer, value);

        public bool Equals(RoaringBitmap bitmap)
        {
            if (bitmap == null) return false;
            return NativeMethods.roaring_bitmap_equals(_pointer, bitmap._pointer);
        }

        //Bitmap operations

        public bool Select(uint rank, out uint element)
            => NativeMethods.roaring_bitmap_select(_pointer, rank, out element);

        public RoaringBitmap Not(ulong start, ulong end)
            => new RoaringBitmap(NativeMethods.roaring_bitmap_flip(_pointer, start, end));
        public void INot(ulong start, ulong end)
            => NativeMethods.roaring_bitmap_flip_inplace(_pointer, start, end);

        public RoaringBitmap And(RoaringBitmap bitmap)
            => new RoaringBitmap(NativeMethods.roaring_bitmap_and(_pointer, bitmap._pointer));
        public void IAnd(RoaringBitmap bitmap)
            => NativeMethods.roaring_bitmap_and_inplace(_pointer, bitmap._pointer);

        public RoaringBitmap AndNot(RoaringBitmap bitmap)
            => new RoaringBitmap(NativeMethods.roaring_bitmap_andnot(_pointer, bitmap._pointer));
        public void IAndNot(RoaringBitmap bitmap)
            => NativeMethods.roaring_bitmap_andnot_inplace(_pointer, bitmap._pointer);

        public RoaringBitmap Or(RoaringBitmap bitmap)
            => new RoaringBitmap(NativeMethods.roaring_bitmap_or(_pointer, bitmap._pointer));
        public void IOr(RoaringBitmap bitmap)
            => NativeMethods.roaring_bitmap_or_inplace(_pointer, bitmap._pointer);

        public RoaringBitmap Xor(RoaringBitmap bitmap)
            => new RoaringBitmap(NativeMethods.roaring_bitmap_xor(_pointer, bitmap._pointer));
        public void IXor(RoaringBitmap bitmap)
            => NativeMethods.roaring_bitmap_xor_inplace(_pointer, bitmap._pointer);

        public static RoaringBitmap OrMany(params RoaringBitmap[] bitmaps)
        {
            var pointers = new IntPtr[bitmaps.Length];
            for (int i = 0; i < pointers.Length; i++)
                pointers[i] = bitmaps[i]._pointer;
            return new RoaringBitmap(NativeMethods.roaring_bitmap_or_many((uint)bitmaps.Length, pointers));
        }
        public static RoaringBitmap OrManyHeap(params RoaringBitmap[] bitmaps)
        {
            var pointers = new IntPtr[bitmaps.Length];
            for (int i = 0; i < pointers.Length; i++)
                pointers[i] = bitmaps[i]._pointer;
            return new RoaringBitmap(NativeMethods.roaring_bitmap_or_many_heap((uint)bitmaps.Length, pointers));
        }
        public static RoaringBitmap XorMany(params RoaringBitmap[] bitmaps)
        {
            var pointers = new IntPtr[bitmaps.Length];
            for (int i = 0; i < pointers.Length; i++)
                pointers[i] = bitmaps[i]._pointer;
            return new RoaringBitmap(NativeMethods.roaring_bitmap_xor_many((uint)bitmaps.Length, pointers));
        }

        public RoaringBitmap LazyOr(RoaringBitmap bitmap, bool bitsetConversion)
            => new RoaringBitmap(NativeMethods.roaring_bitmap_lazy_or(_pointer, bitmap._pointer, bitsetConversion));
        public void ILazyOr(RoaringBitmap bitmap, bool bitsetConversion)
            => NativeMethods.roaring_bitmap_lazy_or_inplace(_pointer, bitmap._pointer, bitsetConversion);
        public RoaringBitmap LazyXor(RoaringBitmap bitmap, bool bitsetConversion)
            => new RoaringBitmap(NativeMethods.roaring_bitmap_lazy_xor(_pointer, bitmap._pointer, bitsetConversion));
        public void ILazyXor(RoaringBitmap bitmap, bool bitsetConversion)
            => NativeMethods.roaring_bitmap_lazy_xor_inplace(_pointer, bitmap._pointer, bitsetConversion);
        public void RepairAfterLazy()
            => NativeMethods.roaring_bitmap_repair_after_lazy(_pointer);

        //Optimization/Compression

        public bool Optimize()
            => NativeMethods.roaring_bitmap_run_optimize(_pointer);
        public bool RemoveRunCompression()
            => NativeMethods.roaring_bitmap_remove_run_compression(_pointer);
        public int ShrinkToFit()
            => NativeMethods.roaring_bitmap_shrink_to_fit(_pointer);

        //Serialization

        public void CopyTo(uint[] buffer)
            => NativeMethods.roaring_bitmap_to_uint32_array(_pointer, buffer);

        public byte[] Serialize(SerializationFormat format = SerializationFormat.Normal)
        {
            byte[] buffer;
            switch (format)
            {
                case SerializationFormat.Normal:
                default:
                    buffer = new byte[NativeMethods.roaring_bitmap_size_in_bytes(_pointer)];
                    NativeMethods.roaring_bitmap_serialize(_pointer, buffer);
                    break;
                case SerializationFormat.Portable:
                    buffer = new byte[NativeMethods.roaring_bitmap_portable_size_in_bytes(_pointer)];
                    NativeMethods.roaring_bitmap_portable_serialize(_pointer, buffer);
                    break;
            }
            return buffer;
        }
        public static RoaringBitmap Deserialize(byte[] buffer, SerializationFormat format = SerializationFormat.Normal)
        {
            switch (format)
            {
                case SerializationFormat.Normal:
                default:
                    return new RoaringBitmap(NativeMethods.roaring_bitmap_deserialize(buffer));
                case SerializationFormat.Portable:
                    return new RoaringBitmap(NativeMethods.roaring_bitmap_portable_deserialize(buffer));
            }
        }

        //Iterators

        public IEnumerator<uint> GetEnumerator()
        {
            ulong count = NativeMethods.roaring_bitmap_get_cardinality(_pointer);
            if (count < 262144) //1MB
            {
                uint[] values = new uint[count];
                NativeMethods.roaring_bitmap_to_uint32_array(_pointer, values);
                return (values as IEnumerable<uint>).GetEnumerator();
            }
            else
                return GetValues(bufferSize: 262144).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public uint[] ToArray()
        {
            ulong count = NativeMethods.roaring_bitmap_get_cardinality(_pointer);
            uint[] values = new uint[count];
            NativeMethods.roaring_bitmap_to_uint32_array(_pointer, values);
            return values;
        }
        public uint[] ToArray(ulong count)
        {
            ulong cardinality = NativeMethods.roaring_bitmap_get_cardinality(_pointer);
            if (cardinality < count)
                count = cardinality;

            uint[] values = new uint[count];
            NativeMethods.roaring_bitmap_to_uint32_array(_pointer, values);
            return values;
        }
        public IEnumerable<uint> GetValues(uint bufferSize = 262144)
            => GetValues(ulong.MaxValue, bufferSize);
        public IEnumerable<uint> GetValues(ulong count, uint bufferSize = 262144)
        {
            uint[] values = new uint[bufferSize]; //262144 = 1MB / sizeof(uint)
            uint iterationCount = 0;
            ulong totalCount = 0;
            var yieldEvent = new ManualResetEventSlim(false);
            var continueEvent = new ManualResetEventSlim(false);
            bool done = false;

            Task.Run(() =>
            {
                try
                {
                    if (count < bufferSize)
                        bufferSize = (uint)count;
                    NativeMethods.roaring_iterate(_pointer, (x, _) =>
                    {
                        values[iterationCount++] = x;
                        if (iterationCount == bufferSize)
                        {
                            totalCount += iterationCount;
                            if (totalCount == count)
                                return false;

                            yieldEvent.Set();
                            continueEvent.Wait();
                            continueEvent.Reset();

                            iterationCount = 0;
                            if ((count - totalCount) < bufferSize)
                                bufferSize = (uint)(count - totalCount);
                        }
                        return true;
                    }, IntPtr.Zero);
                }
                finally
                {
                    done = true;
                    yieldEvent.Set();
                }
            });

            while (!done)
            {
                yieldEvent.Wait();
                yieldEvent.Reset();
                for (int i = 0; i < iterationCount; i++)
                    yield return values[i];
                continueEvent.Set();
            }
        }

        //Other

        public Statistics GetStatistics()
        {
            NativeMethods.roaring_bitmap_statistics(_pointer, out var stats);
            return stats;
        }
    }
}
