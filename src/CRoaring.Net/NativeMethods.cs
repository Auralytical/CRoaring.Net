using System;
using System.Runtime.InteropServices;

namespace CRoaring
{
    internal static unsafe class NativeMethods
    {
        //Creation/Destruction

        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_create();
        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_create_with_capacity(uint capacity);
        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_from_range(uint min, uint max, uint step);
        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_of_ptr(uint count, uint* values);
        //[DllImport("roaring")]
        //public static extern IntPtr roaring_bitmap_of(uint[] values);
        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_copy(IntPtr bitmap);

        [DllImport("roaring")]
        public static extern void roaring_bitmap_free(IntPtr bitmap);

        //Properties

        [DllImport("roaring")]
        public static extern ulong roaring_bitmap_get_cardinality(IntPtr bitmap);
        [DllImport("roaring")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool roaring_bitmap_is_empty(IntPtr bitmap);
        
        //List operations

        [DllImport("roaring")]
        public static extern void roaring_bitmap_add(IntPtr bitmap, uint value);
        [DllImport("roaring")]
        public static extern void roaring_bitmap_add_many(IntPtr bitmap, uint count, uint* values);
        [DllImport("roaring")]
        public static extern void roaring_bitmap_remove(IntPtr bitmap, uint value);

        [DllImport("roaring")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool roaring_bitmap_contains(IntPtr bitmap, uint value);
        [DllImport("roaring")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool roaring_bitmap_select(IntPtr bitmap, uint rank, out uint element);

        [DllImport("roaring")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool roaring_bitmap_equals(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("roaring")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool roaring_bitmap_is_subset(IntPtr bitmap1, IntPtr bitmap2);
        public static bool roaring_bitmap_is_strict_subset(IntPtr bitmap1, IntPtr bitmap2)
        {
            return roaring_bitmap_get_cardinality(bitmap2) > roaring_bitmap_get_cardinality(bitmap1) &&
                roaring_bitmap_is_subset(bitmap1, bitmap2);
        }

        [DllImport("roaring")]
        public static extern uint roaring_bitmap_minimum(IntPtr bitmap);
        [DllImport("roaring")]
        public static extern uint roaring_bitmap_maximum(IntPtr bitmap);

        //Bitmap operations

        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_flip(IntPtr bitmap, ulong start, ulong end);
        [DllImport("roaring")]
        public static extern void roaring_bitmap_flip_inplace(IntPtr bitmap, ulong start, ulong end);

        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_and(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("roaring")]
        public static extern void roaring_bitmap_and_inplace(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("roaring")]
        public static extern ulong roaring_bitmap_and_cardinality(IntPtr bitmap1, IntPtr bitmap2);

        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_andnot(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("roaring")]
        public static extern void roaring_bitmap_andnot_inplace(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("roaring")]
        public static extern ulong roaring_bitmap_andnot_cardinality(IntPtr bitmap1, IntPtr bitmap2);

        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_or(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("roaring")]
        public static extern void roaring_bitmap_or_inplace(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("roaring")]
        public static extern ulong roaring_bitmap_or_cardinality(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_or_many(uint count, IntPtr[] bitmaps);
        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_or_many_heap(uint count, IntPtr[] bitmaps);

        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_xor(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("roaring")]
        public static extern void roaring_bitmap_xor_inplace(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("roaring")]
        public static extern ulong roaring_bitmap_xor_cardinality(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_xor_many(uint count, IntPtr[] bitmaps);

        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_lazy_or(IntPtr bitmap1, IntPtr bitmap2, bool bitsetConversion);
        [DllImport("roaring")]
        public static extern void roaring_bitmap_lazy_or_inplace(IntPtr bitmap1, IntPtr bitmap2, bool bitsetConversion);
        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_lazy_xor(IntPtr bitmap1, IntPtr bitmap2, bool bitsetConversion);
        [DllImport("roaring")]
        public static extern void roaring_bitmap_lazy_xor_inplace(IntPtr bitmap1, IntPtr bitmap2, bool bitsetConversion);
        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_repair_after_lazy(IntPtr bitmap);

        [DllImport("roaring")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool roaring_bitmap_intersect(IntPtr bitmap1, IntPtr bitmap2);

        [DllImport("roaring")]
        public static extern double roaring_bitmap_jaccard_index(IntPtr bitmap1, IntPtr bitmap2);

        //Optimization

        [DllImport("roaring")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool roaring_bitmap_run_optimize(IntPtr bitmap);
        [DllImport("roaring")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool roaring_bitmap_remove_run_compression(IntPtr bitmap);
        [DllImport("roaring")]
        public static extern int roaring_bitmap_shrink_to_fit(IntPtr bitmap);

        //Serialization

        [DllImport("roaring")]
        public static extern int roaring_bitmap_size_in_bytes(IntPtr bitmap);
        [DllImport("roaring")]
        public static extern int roaring_bitmap_portable_size_in_bytes(IntPtr bitmap);

        [DllImport("roaring")]
        public static extern void roaring_bitmap_to_uint32_array(IntPtr bitmap, uint[] values);

        [DllImport("roaring")]
        public static extern int roaring_bitmap_serialize(IntPtr bitmap, byte[] buffer);
        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_deserialize(byte[] buffer);

        [DllImport("roaring")]
        public static extern int roaring_bitmap_portable_serialize(IntPtr bitmap, byte[] buffer);
        [DllImport("roaring")]
        public static extern IntPtr roaring_bitmap_portable_deserialize(byte[] buffer);

        //Iterators

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Iterator
        {
            public readonly IntPtr parent;
            public int container_index;
            public int in_container_index;
            public int run_index;
            public uint in_run_index;

            public uint current_value;
            public bool has_value;

            public readonly IntPtr container;
            public byte typecode;
            public uint highbits;
        }

        [DllImport("roaring")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool roaring_iterate(IntPtr bitmap, IteratorDelegate iterator, IntPtr tag);
        public static bool roaring_iterate(IntPtr bitmap, Func<uint, bool> iterator)
        {
            return roaring_iterate(bitmap, (v, t) => iterator(v), IntPtr.Zero);
        }
        public delegate bool IteratorDelegate(uint value, IntPtr tag);

        [DllImport("roaring")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool roaring_iterate64(IntPtr bitmap, IteratorDelegate iterator, ulong high_bits, IntPtr tag);
        public static bool roaring_iterate64(IntPtr bitmap, Func<uint, bool> iterator, ulong high_bits)
        {
            return roaring_iterate64(bitmap, (v, t) => iterator(v), high_bits, IntPtr.Zero);
        }
        public delegate bool IteratorDelegate64(uint value, IntPtr tag);

        [DllImport("roaring")]
        public static extern IntPtr roaring_create_iterator(IntPtr bitmap);
        [DllImport("roaring")]
        public static extern void roaring_init_iterator(IntPtr bitmap, IntPtr iterator);
        [DllImport("roaring")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool roaring_advance_uint32_iterator(IntPtr iterator);
        [DllImport("roaring")]
        public static extern IntPtr roaring_copy_uint32_iterator(IntPtr iterator);
        [DllImport("roaring")]
        public static extern void roaring_free_uint32_iterator(IntPtr iterator);

        // Other
        [DllImport("roaring")]
        public static extern void roaring_bitmap_printf(IntPtr bitmap);
        [DllImport("roaring")]
        public static extern void roaring_bitmap_printf_describe(IntPtr bitmap);
        [DllImport("roaring")]
        public static extern void roaring_bitmap_statistics(IntPtr bitmap, out Statistics stats);
    }
}
