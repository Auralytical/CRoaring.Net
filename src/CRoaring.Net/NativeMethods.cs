using System;
using System.Runtime.InteropServices;

namespace Roaring
{
    internal static class NativeMethods
    {
        //Creation/Destruction

        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_create();
        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_create_with_capacity(uint capacity);
        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_from_range(uint min, uint max, uint step);
        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_of_ptr(uint count, uint[] values);
        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_copy(uint count, uint[] values);

        [DllImport("CRoaring")]
        public static extern void roaring_bitmap_free(IntPtr bitmap);

        //Properties

        [DllImport("CRoaring")]
        public static extern ulong roaring_bitmap_get_cardinality(IntPtr bitmap);
        [DllImport("CRoaring")]
        public static extern bool roaring_bitmap_is_empty(IntPtr bitmap);
        
        //List operations

        [DllImport("CRoaring")]
        public static extern void roaring_bitmap_add(IntPtr bitmap, uint value);
        [DllImport("CRoaring")]
        public static extern void roaring_bitmap_remove(IntPtr bitmap, uint value);

        [DllImport("CRoaring")]
        public static extern bool roaring_bitmap_contains(IntPtr bitmap, uint value);

        //Bitmap operations

        [DllImport("CRoaring")]
        public static extern bool roaring_bitmap_select(IntPtr bitmap1, uint rank, out uint element);

        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_flip(IntPtr bitmap1, ulong start, ulong end);
        [DllImport("CRoaring")]
        public static extern void roaring_bitmap_flip_inplace(IntPtr bitmap1, ulong start, ulong end);

        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_and(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("CRoaring")]
        public static extern void roaring_bitmap_and_inplace(IntPtr bitmap1, IntPtr bitmap2);

        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_andnot(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("CRoaring")]
        public static extern void roaring_bitmap_andnot_inplace(IntPtr bitmap1, IntPtr bitmap2);

        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_or(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("CRoaring")]
        public static extern void roaring_bitmap_or_inplace(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_or_many(uint count, IntPtr[] bitmaps);
        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_or_many_heap(uint count, IntPtr[] bitmaps);

        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_xor(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("CRoaring")]
        public static extern void roaring_bitmap_xor_inplace(IntPtr bitmap1, IntPtr bitmap2);
        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_xor_many(uint count, IntPtr[] bitmaps);

        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_lazy_or(IntPtr bitmap1, IntPtr bitmap2, bool bitsetConversion);
        [DllImport("CRoaring")]
        public static extern void roaring_bitmap_lazy_or_inplace(IntPtr bitmap1, IntPtr bitmap2, bool bitsetConversion);
        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_lazy_xor(IntPtr bitmap1, IntPtr bitmap2, bool bitsetConversion);
        [DllImport("CRoaring")]
        public static extern void roaring_bitmap_lazy_xor_inplace(IntPtr bitmap1, IntPtr bitmap2, bool bitsetConversion);
        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_repair_after_lazy(IntPtr bitmap);

        //Optimization

        [DllImport("CRoaring")]
        public static extern bool roaring_bitmap_run_optimize(IntPtr bitmap);
        [DllImport("CRoaring")]
        public static extern bool roaring_bitmap_remove_run_compression(IntPtr bitmap);

        //Serialization

        [DllImport("CRoaring")]
        public static extern int roaring_bitmap_size_in_bytes(IntPtr bitmap);
        [DllImport("CRoaring")]
        public static extern int roaring_bitmap_portable_size_in_bytes(IntPtr bitmap);

        [DllImport("CRoaring")]
        public static extern void roaring_bitmap_to_uint32_array(IntPtr bitmap, uint[] values);

        [DllImport("CRoaring")]
        public static extern int roaring_bitmap_serialize(IntPtr bitmap, byte[] buffer);
        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_deserialize(byte[] buffer);

        [DllImport("CRoaring")]
        public static extern int roaring_bitmap_portable_serialize(IntPtr bitmap, byte[] buffer);
        [DllImport("CRoaring")]
        public static extern IntPtr roaring_bitmap_portable_deserialize(byte[] buffer);

        //Other

        [DllImport("CRoaring")]
        public static extern bool roaring_bitmap_equals(IntPtr bitmap1, IntPtr bitmap2);

        [DllImport("CRoaring")]
        public static extern bool roaring_iterate(IntPtr bitmap, IteratorDelegate iterator, IntPtr tag);
        public delegate bool IteratorDelegate(uint value, IntPtr tag);

        [DllImport("CRoaring")]
        public static extern void roaring_bitmap_printf(IntPtr bitmap);
        [DllImport("CRoaring")]
        public static extern void roaring_bitmap_statistics(IntPtr bitmap, out Statistics stats);
    }
}
