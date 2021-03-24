using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;

namespace NapalmOS.Hardware
{
    public static unsafe class MemoryManager
    {
        // blocks
        public static List<MemoryBlock> Blocks { get; private set; } = new List<MemoryBlock>();
        public static int Count { get { return Blocks.Count; } }

        // properties
        public static uint BaseStart { get; private set; } = 0x400000;
        public static uint BaseEnd { get; private set; } = 0x100000;
        public static uint Size { get { return BaseStart - BaseEnd; } }
        public static uint Pointer { get; private set; } = BaseStart;

        // task
        public static Task Task = new Task("memmgr", "memmgr");

        // allocate memory
        public static MemoryBlock AllocateBlock(uint size)
        {
            // create block
            Pointer -= size;
            MemoryBlock block = new MemoryBlock(Pointer, size);
            block.Fill(0x00);
            block.Allocated = true;

            // add block
            Blocks.Add(block);

            // update indices
            UpdateIndexValues();

            // log
            ShowMessage("new", (int)block.Base, block.Size, true);

            // return 
            return block;
        }

        // unallocate memory
        public static bool Unallocate(MemoryBlock block)
        {
            // unable to unallocate
            if (!block.Allocated) { return false; }

            // clear region
            block.Fill(0x00);

            // remove and update indices
            Blocks.RemoveAt((int)block.Index);
            UpdateIndexValues();

            // log
            ShowMessage("del", (int)block.Base, block.Size, true);

            // return
            return true;

        }

        private static void ShowMessage(string op, int offset, uint size, bool log)
        {
            // text mode message
            if (VGADriver.Mode.IsTextMode)
            {
                // header
                Terminal.Write("[", ConsoleColor.White); Terminal.Write("MALLOC", ConsoleColor.Cyan); Terminal.Write("] ", ConsoleColor.White);

                // operation
                Terminal.Write("op=", ConsoleColor.Gray);
                Terminal.Write(op.ToUpper(), ConsoleColor.White);

                // offset
                Terminal.Write("  offset=", ConsoleColor.Gray);
                Terminal.Write(StringUtil.IntToHex(offset), ConsoleColor.White);

                // size
                Terminal.Write("  size=", ConsoleColor.Gray);
                Terminal.WriteLine(size.ToString(), ConsoleColor.White);
            }

            // write to log
            if (log) { ExceptionHandler.Log("[MALLOC] op=" + op.ToUpper() + "  offset=" + StringUtil.IntToHex(offset) + "  size=" + size.ToString()); }
        }

        // update indexes
        private static void UpdateIndexValues()
        {
            for (uint i = 0; i < Blocks.Count; i++) { Blocks[(int)i].Index = i; }
            if (Blocks.Count == 0) { Pointer = BaseStart; }
        }

        // swape range of memory
        public static void Swap(byte* dest, byte* source, uint len)
        {
            for (uint i = 0; i < len; i++)
                if (*(dest + i) != *(source + i))
                    *(dest + i) = *(source + i);
        }

        // swap range of memory using blocks
        public static void Swap(MemoryBlock dest, MemoryBlock src, uint len) { Swap(dest.Base, src.Base, len); }

        // copy range of memory
        public static void Copy(byte* src, byte* dest, uint len)
        {
            for (uint i = 0; i < len; i++) { *(dest + i) = *(src + i); }
        }


        // get free memory
        public static uint GetFree()
        {
            uint free = 0;
            for (int i = 0; i < Blocks.Count; i++) { free += Blocks[i].GetFree(); }
            return free;
        }

        // get used memory
        public static uint GetUsed()
        {
            uint used = 0;
            for (int i = 0; i < Blocks.Count; i++) { used += Blocks[i].GetUsed(); }
            return used;
        }

        // get total memory
        public static uint GetSize() { return Size; }
    }
}
