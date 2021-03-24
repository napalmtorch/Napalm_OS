using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;

namespace NapalmOS.Hardware
{
    public unsafe class MemoryBlock
    {
        // properties
        public byte* Base { get; private set; }
        public uint Size { get; private set; }
        public uint Index { get; set; }
        public bool Allocated { get; set; }

        // memory
        public uint AmountFree { get; private set; }
        public uint AmountUsed { get; private set; }

        // usage
        private bool changed = false;
        private uint free, used;

        // constructors
        public MemoryBlock(uint offset, uint size)
        {
            this.Base = (byte*)offset;
            this.Size = size;
            this.Allocated = false;
        }
        
        // fill
        public void Fill(byte data) { for (int i = 0; i < Size; i++) { Base[i] = data; } }

        // copy data to different address
        public void Copy(byte* dest, uint index, uint len)
        {
            for (uint i = 0; i < len; i++)
            { *(dest + i) = *(Base + index + i); }
        }

        // write 8-bit integer
        public bool WriteInt8(uint offset, byte data)
        {
            if (offset >= Size) { ExceptionHandler.Log("Tried to write out of memory bounds: " + offset.ToString()); return false; }
            Base[offset] = data;
            changed = true;
            return true;
        }

        // write 16-bit integer
        public bool WriteInt16(uint offset, ushort data)
        {
            if (offset + 1 >= Size) { ExceptionHandler.Log("Tried to write out of memory bounds: " + (offset + 1).ToString()); return false; }
            Base[offset] = (byte)((data & 0xFF00) >> 8);
            Base[offset + 1] = (byte)(data & 0x00FF);
            changed = true;
            return true;
        }

        // write 32-bit integer
        public bool WriteInt32(uint offset, uint data)
        {
            if (offset + 3 >= Size) { ExceptionHandler.Log("Tried to write out of memory bounds: " + (offset + 3).ToString()); return false; }
            Base[offset] = (byte)((data & 0xFF000000) >> 24);
            Base[offset + 1] = (byte)((data & 0x00FF0000) >> 16);
            Base[offset + 2] = (byte)((data & 0x0000FF00) >> 8);
            Base[offset + 3] = (byte)(data & 0x000000FF);
            changed = true;
            return true;
        }

        // write character
        public bool WriteChar(uint offset, char data) { return WriteInt8(offset, (byte)data); }

        // write string
        public bool WriteString(uint offset, string data)
        {
            if (offset + data.Length + 1 >= Size) { return false; }
            for (uint i = 0; i < data.Length; i++) { Base[offset + i] = (byte)data[(int)i]; }
            changed = true;
            return true;
        }

        // read 8-bit integer
        public byte ReadInt8(uint offset)
        {
            if (offset >= Size) { ExceptionHandler.Log("Tried to read out of memory bounds: " + offset.ToString()); return 0; }
            byte data = Base[offset];
            return data;
        }

        // read 16-bit integer
        public ushort ReadInt16(uint offset)
        {
            if (offset + 1 >= Size) { ExceptionHandler.Log("Tried to read out of memory bounds: " + (offset + 1).ToString()); return 0; }
            ushort data = (ushort)((Base[offset] << 8) | Base[offset + 1]);
            return data;
        }

        // read 32-bit integer
        public uint ReadInt32(uint offset)
        {
            if (offset + 3 >= Size) { ExceptionHandler.Log("Tried to read out of memory bounds: " + (offset + 3).ToString()); return 0; }
            uint data = (uint)((Base[offset] << 24) | (Base[offset + 1] << 16) | Base[offset + 2] << 8 | Base[offset + 3]);
            return data;
        }

        // read character
        public char ReadChar(uint offset) { return (char)ReadInt8(offset); }

        // read string
        public string ReadString(uint offset, uint len)
        {
            if (offset + len >= Size) { ExceptionHandler.Log("Tried to read out of memory bounds: " + (offset + len).ToString()); return string.Empty; }
            string text = "";
            for (uint i = 0; i < len; i++) { text += Base[offset + i]; }
            return text;
        }

        // update usage stats
        public void UpdateUsage(bool onChange)
        {
            if (onChange)
            {
                if (changed)
                {
                    free = 0;
                    used = 0;
                    for (uint i = 0; i < Size; i++) { if (Base[i] == 0) { free++; } else { used++; } }
                    changed = false;
                }
            }
            else
            {
                free = 0;
                used = 0;
                for (uint i = 0; i < Size; i++) { if (Base[i] == 0) { free++; } else { used++; } }
                changed = false;
            }
        }

        // get amount free
        public uint GetFree() { return free; }

        // get amount used
        public uint GetUsed() { return used; }

        // get amount total
        public uint GetTotal() { return Size; }
    }
}
