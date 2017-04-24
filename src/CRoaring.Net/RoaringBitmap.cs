using System;
using System.Collections;
using System.Collections.Generic;

namespace CRoaring
{
    public unsafe class RoaringBitmap : IDisposable, IEnumerable<uint>
    {
        private readonly IntPtr _pointer;
        private bool _isDisposed = false;

        public ulong Cardinality => NativeMethods.roaring_bitmap_get_cardinality(_pointer);
        public bool IsEmpty => NativeMethods.roaring_bitmap_is_empty(_pointer);
        public uint Min => NativeMethods.roaring_bitmap_minimum(_pointer);
        public uint Max => NativeMethods.roaring_bitmap_maximum(_pointer);
        public int SerializedBytes => NativeMethods.roaring_bitmap_size_in_bytes(_pointer);
        public int PortableSerializedBytes => NativeMethods.roaring_bitmap_portable_size_in_bytes(_pointer);

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
        public static RoaringBitmap FromValues(params uint[] values)
            => FromValues(values, 0, values.Length);
        public static RoaringBitmap FromValues(uint[] values, int offset, int count)
        {
            fixed (uint* valuePtr = values)
                new RoaringBitmap(NativeMethods.roaring_bitmap_of_ptr((uint)count, valuePtr + offset));
        }

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
        public void AddMany(params uint[] values)
            => AddMany(values, 0, (uint)values.Length);
        public void AddMany(uint[] values, uint offset, uint count)
        {
            fixed (uint* valuePtr = values)
                NativeMethods.roaring_bitmap_add_many(_pointer, count, valuePtr + offset);
        }

        public void Remove(uint value)
            => NativeMethods.roaring_bitmap_remove(_pointer, value);
        public void RemoveMany(params uint[] values)
            => RemoveMany(values, 0, (uint)values.Length);
        public void RemoveMany(uint[] values, uint offset, uint count)
        {
            fixed (uint* valuePtr = values)
            {
                uint* ptr = valuePtr + offset;
                for (int i = 0; i < count; i++)
                    NativeMethods.roaring_bitmap_remove(_pointer, *ptr++);
            }
        }

        public bool Contains(uint value)
            => NativeMethods.roaring_bitmap_contains(_pointer, value);

        public bool Equals(RoaringBitmap bitmap)
        {
            if (bitmap == null) return false;
            return NativeMethods.roaring_bitmap_equals(_pointer, bitmap._pointer);
        }

        public bool IsSubset(RoaringBitmap bitmap, bool isStrict = false)
        {
            if (bitmap == null) return false;
            if (isStrict)
                return NativeMethods.roaring_bitmap_is_strict_subset(_pointer, bitmap._pointer);
            else
                return NativeMethods.roaring_bitmap_is_subset(_pointer, bitmap._pointer);
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
            return new Enumerator(_pointer);
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

        //Other

        public Statistics GetStatistics()
        {
            NativeMethods.roaring_bitmap_statistics(_pointer, out var stats);
            return stats;
        }

        private unsafe class Enumerator : IEnumerator<uint>
        {
            private readonly NativeMethods.Iterator* _iterator;
            private bool _isFirst, _isDisposed;
            
            public uint Current => _iterator->current_value;
            object IEnumerator.Current => Current;

            public Enumerator(IntPtr bitmap)
            {
                _iterator = (NativeMethods.Iterator*)NativeMethods.roaring_create_iterator(bitmap);
                _isFirst = true;
            }

            public bool MoveNext()
            {
                if (_isFirst)
                {
                    _isFirst = false;
                    return _iterator->has_value;
                }
                return NativeMethods.roaring_advance_uint32_iterator(new IntPtr(_iterator));
            }

            public void Reset()
            {
                throw new InvalidOperationException();
            }

            private void Dispose(bool isDisposing)
            {
                if (_isDisposed) return;

                NativeMethods.roaring_free_uint32_iterator(new IntPtr(_iterator));
                _isDisposed = true;
            }
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            ~Enumerator() => Dispose(false);
        }
    }
}
