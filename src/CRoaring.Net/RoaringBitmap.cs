using System;
using System.Collections;
using System.Collections.Concurrent;
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
        

        //Other

        public bool Equals(RoaringBitmap bitmap)
        {
            if (bitmap == null) return false;
            return NativeMethods.roaring_bitmap_equals(_pointer, bitmap._pointer);
        }

        public RoaringBitmap Clone()
        {
            return new RoaringBitmap(NativeMethods.roaring_bitmap_copy(_pointer));
        }

        /*public void Print()
            => NativeMethods.roaring_bitmap_printf(_pointer);*/
        public Statistics GetStatistics()
        {
            Statistics stats;
            NativeMethods.roaring_bitmap_statistics(_pointer, out stats);
            return stats;
        }

        // Enumerators
        public IEnumerator<uint> GetEnumerator()
        {
            var enumerator = new Enumerator();
            Task.Run(() =>
            {
                try { NativeMethods.roaring_iterate(_pointer, enumerator.AddValue, IntPtr.Zero); }
                finally { enumerator.Complete(); }
            });
            return enumerator;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class Enumerator : IEnumerator<uint>
        {
            private const int BufferSize = 1000;

            private BlockingCollection<uint> _buffer;
            private uint _value;
            private bool _isDisposed;
            private ManualResetEventSlim _completeEvent;

            public uint Current
            {
                get
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(nameof(Enumerator));
                    return _value;
                }
            }
            object IEnumerator.Current => Current;

            public Enumerator()
            {
                _buffer = new BlockingCollection<uint>(BufferSize);
                _completeEvent = new ManualResetEventSlim(false);
            }

            public void Complete()
            {
                _buffer.CompleteAdding();
                _completeEvent.Wait();
            }
            public void Dispose()
            {
                _isDisposed = true;
                _buffer.Dispose();
                _completeEvent.Set();
            }

            public bool MoveNext()
            {
                if (!_buffer.TryTake(out _value, -1))
                {
                    _completeEvent.Set();
                    return false;
                }
                return true;
            }
            public void Reset() { throw new NotSupportedException(); }

            public bool AddValue(uint value, IntPtr tag)
            {
                try
                {
                    if (_isDisposed) return false;
                    _buffer.Add(value);
                    return !_isDisposed;
                }
                catch { return false; }
            }
        }
    }
}
