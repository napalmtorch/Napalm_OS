using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.Core;
using NapalmOS.Core;
using NapalmOS.Graphics;
using NapalmOS.GUI;

namespace NapalmOS.Hardware
{
	public static unsafe class VGADriver
	{
		// write ports
		private static readonly IOPortWrite AttributeWrite = new IOPortWrite(0x3C0);
		private static readonly IOPortWrite MiscWrite = new IOPortWrite(0x3C2);
		private static readonly IOPortWrite SequencerWrite = new IOPortWrite(0x3C4);
		private static readonly IOPortWrite DACWrite = new IOPortWrite(0x3C8);
		private static readonly IOPortWrite DACData = new IOPortWrite(0x3C9);
		private static readonly IOPortWrite GFXWrite = new IOPortWrite(0x3CE);
		private static readonly IOPortWrite CRTCWrite = new IOPortWrite(0x3D4);

		// read ports
		private static readonly IOPortRead AttributeRead = new IOPortRead(0x3C1);
		private static readonly IOPortRead DACRead = new IOPortRead(0x3C7);
		private static readonly IOPortRead InstatRead = new IOPortRead(0x3DA);

		// 2 way ports
		private static readonly IOPort SequencerData = new IOPort(0x3C5);
		private static readonly IOPort GFXData = new IOPort(0x3CF);
		private static readonly IOPort CRTCData = new IOPort(0x3D5);
		private static readonly IOPort MaskData = new IOPort(0x3C6);

		// task
		public static Task Task = new Task("VGA Driver", "vgadrv.sys");

		// properties
		public static VideoMode Mode { get; private set; }
		public static byte* Buffer = (byte*)0xB8000;
		private static int BufferSize;
		public static MemoryBlock BackBuffer;


		// color palette - graphics mode
		public static uint[] Palette256 = new uint[256]
		{
			0x000000, 0x010103, 0x030306, 0x040409, 0x06060C, 0x07070F, 0x090913, 0x0B0B16, 0x0C0C19, 0x0E0E1C, 0x0F0F1F, 0x111123, 0x131326, 0x141429, 0x16162C, 0x17172F,
			0x000000, 0x010301, 0x030603, 0x040904, 0x060C06, 0x070F07, 0x091309, 0x0B160B, 0x0C190C, 0x0E1C0E, 0x0F1F0F, 0x112311, 0x132613, 0x142914, 0x162C16, 0x172F17,
			0x000000, 0x030101, 0x060303, 0x090404, 0x0C0606, 0x0F0707, 0x130909, 0x160B0B, 0x190C0C, 0x1C0E0E, 0x1F0F0F, 0x231111, 0x261313, 0x291414, 0x2C1616, 0x2F1717,
			0x000000, 0x000103, 0x000306, 0x000409, 0x00060C, 0x00070F, 0x000913, 0x000B16, 0x000C19, 0x000E1C, 0x000F1F, 0x001123, 0x001326, 0x001429, 0x00162C, 0x00172F,
			0x000000, 0x010003, 0x030006, 0x040009, 0x06000C, 0x07000F, 0x090013, 0x0B0016, 0x0C0019, 0x0E001C, 0x0F001F, 0x110023, 0x130026, 0x140029, 0x16002C, 0x17002F,
			0x000000, 0x000301, 0x000603, 0x000904, 0x000C06, 0x000F07, 0x001309, 0x00160B, 0x00190C, 0x001C0E, 0x001F0F, 0x002311, 0x002613, 0x002914, 0x002C16, 0x002F17,
			0x000000, 0x010300, 0x030600, 0x040900, 0x060C00, 0x070F00, 0x091300, 0x0B1600, 0x0C1900, 0x0E1C00, 0x0F1F00, 0x112300, 0x132600, 0x142900, 0x162C00, 0x172F00,
			0x000000, 0x030001, 0x060003, 0x090004, 0x0C0006, 0x0F0007, 0x130009, 0x16000B, 0x19000C, 0x1C000E, 0x1F000F, 0x230011, 0x260013, 0x290014, 0x2C0016, 0x2F0017,
			0x000000, 0x030100, 0x060300, 0x090400, 0x0C0600, 0x0F0700, 0x130900, 0x160B00, 0x190C00, 0x1C0E00, 0x1F0F00, 0x231100, 0x261300, 0x291400, 0x2C1600, 0x2F1700,
			0x000000, 0x030003, 0x060006, 0x090009, 0x0C000C, 0x0F000F, 0x130013, 0x160016, 0x190019, 0x1C001C, 0x1F001F, 0x230023, 0x260026, 0x290029, 0x2C002C, 0x2F002F,
			0x000000, 0x000303, 0x000606, 0x000909, 0x000C0C, 0x000F0F, 0x001313, 0x001616, 0x001919, 0x001C1C, 0x001F1F, 0x002323, 0x002626, 0x002929, 0x002C2C, 0x002F2F,
			0x000000, 0x030300, 0x060600, 0x090900, 0x0C0C00, 0x0F0F00, 0x131300, 0x161600, 0x191900, 0x1C1C00, 0x1F1F00, 0x232300, 0x262600, 0x292900, 0x2C2C00, 0x2F2F00,
			0x000000, 0x000003, 0x000006, 0x000009, 0x00000C, 0x00000F, 0x000013, 0x000016, 0x000019, 0x00001C, 0x00001F, 0x000023, 0x000026, 0x000029, 0x00002C, 0x00002F,
			0x000000, 0x000300, 0x000600, 0x000900, 0x000C00, 0x000F00, 0x001300, 0x001600, 0x001900, 0x001C00, 0x001F00, 0x002300, 0x002600, 0x002900, 0x002C00, 0x002F00,
			0x000000, 0x030000, 0x060000, 0x090000, 0x0C0000, 0x0F0000, 0x130000, 0x160000, 0x190000, 0x1C0000, 0x1F0000, 0x230000, 0x260000, 0x290000, 0x2C0000, 0x2F0000,
			0x000000, 0x030303, 0x060606, 0x090909, 0x0C0C0C, 0x0F0F0F, 0x131313, 0x161616, 0x191919, 0x1C1C1C, 0x1F1F1F, 0x232323, 0x262626, 0x292929, 0x2C2C2C, 0x3F3F3F,
		};

		// color palette - text mode
		public static uint[] Palette16 = new uint[16]
		{
			0x000000, 0x00001F, 0x001F00, 0x001F1F, 0x1F0000, 0x1F001F, 0x2F1F00, 0x2F2F2F, 0x1F1F1F, 0x00103F, 0x003F00, 0x003F3F, 0x3F0000, 0x3F003F, 0x3F3F00, 0x3F3F3F,
		};

		// initialization
		public static void Initialize(VideoMode mode)
		{
			try
			{  
				// initialize fonts
				Font.LoadFonts();

				// set mode
				SetMode(mode);

				// register task
				TaskManager.RegisterTask(Task);
			}
			catch (Exception ex) { ExceptionHandler.ThrowFatal("INIT_VGA", ex.Message); }
		}

		// allocate buffer
		public static void AllocateBuffer() { BackBuffer = MemoryManager.AllocateBlock(0x10000); }

		// set video mode based on id
		public static void SetMode(byte id)
		{
			if (id == VideoMode.Text80x25_ID) { SetMode(VideoMode.Text80x25); }
			else if (id == VideoMode.Text80x50_ID) { SetMode(VideoMode.Text80x50); }
			else if (id == VideoMode.Text90x60_ID) { SetMode(VideoMode.Text90x60_ID); }
			else if (id == VideoMode.Pixel320x200x256_ID) { SetMode(VideoMode.Pixel320x200x256); }
			else if (id == VideoMode.Pixel320x200x256DB_ID) { SetMode(VideoMode.Pixel320x200x256DB); }
		}

		// set video mode
		public static void SetMode(VideoMode mode)
		{
			// set property
			Mode = mode;

			// set buffer size
			if (mode.IsTextMode) { BufferSize = (mode.Width * mode.Height) * 2; }
			else { BufferSize = (mode.Width * mode.Height); }

			// set mode data
			switch (mode.ID)
			{
				// text modes
				case VideoMode.Text80x25_ID: { Mode.SetData(VideoModeInitializers.Mode80x25_Text); break; }
				case VideoMode.Text80x50_ID: { Mode.SetData(VideoModeInitializers.Mode80x50_Text); break; }
				case VideoMode.Text90x60_ID: { Mode.SetData(VideoModeInitializers.Mode90x60_Text); break; }
				// graphics modes
				case VideoMode.Pixel320x200x256_ID: { Mode.SetData(VideoModeInitializers.Mode320x200x256_Pixel); break; }
				case VideoMode.Pixel320x200x256X_ID: { Mode.SetData(VideoModeInitializers.Mode320x200x256X_Pixel); break; }
				case VideoMode.Pixel320x200x256DB_ID: { Mode.SetData(VideoModeInitializers.Mode320x200x256_Pixel); break; }
				default: { Mode.SetData(VideoModeInitializers.Mode80x25_Text); break; }
			}
			fixed (byte* fixedPtr = Mode.Data) { WriteRegisters(fixedPtr); }
			SetBuffer(GetFrameBufferSegment());

			// set font
			switch (mode.Font)
			{
				case Font.Font8x8_ID: { SetFont(Font.Font8x8_Data, 8); break; }
				case Font.Font8x16_ID: { SetFont(Font.Font8x16_Data, 16); break; }
				default: { SetFont(Font.Font8x16_Data, 16); break; }
			}

			// set mouse resolution
			Cosmos.System.MouseManager.ScreenWidth = (uint)Mode.Width;
			Cosmos.System.MouseManager.ScreenHeight = (uint)Mode.Height;

			// set palette
			if (!mode.IsTextMode)
			{
				ClearColorPalette();
				SetColorPalette(Palette256);
			}
			else
			{
				ClearColorPalette();
				SetColorPalette(Palette16);
			}

			// clear screen
			Clear(Color.Black);
		}

		// clear the screen
		public static void Clear(Color color)
		{
			// text mode
			if (Mode.IsTextMode)
			{
				// clear buffer
				Color fg = (Color)((Buffer[1] & 0xF0) >> 4);
				for (int i = 0; i < (Mode.Width * Mode.Height * 2); i += 2) { Buffer[i] = (byte)' '; Buffer[i + 1] = ToAttribute(fg, color); }

				// reset cursor
				SetCursorPos(0, 0);

			}
			// graphics mode
			else
			{
				if (Mode.ID == VideoMode.Pixel320x200x256DB_ID) { BackBuffer.Fill((byte)color); }
				else { for (int i = 0; i < Mode.Width * Mode.Height; i++) { DrawPixel(i % Mode.Width, i / Mode.Width, color); } }
			}
		}

		// swap back buffer
		public static void Display()
		{
			// check mode
			if (Mode.ID != VideoMode.Pixel320x200x256DB_ID) { return; }
			MemoryManager.Copy((byte*)BackBuffer.Base, Buffer, (uint)(Mode.Width * Mode.Height));

			// update memory usage for task
			Task.MemoryUsed = BackBuffer.GetUsed();
			Task.MemoryFree = BackBuffer.GetFree();
			Task.MemoryTotal = BackBuffer.GetTotal();
		}

		public static void DisplayAlt()
		{ 
			if (Mode.ID != VideoMode.Pixel320x200x256DB_ID) { return; }
			MemoryManager.Swap(Buffer, BackBuffer.Base, (uint)(Mode.Width * Mode.Height));
		}

		// draw pixel
		public static void DrawPixel(int x, int y, byte color)
		{
			if (x < 0 || x >= Mode.Width || y < 0 || y >= Mode.Height) { return; }
			if (Mode.IsTextMode)
			{
				uint offset = (uint)(x + (y * Mode.Width)) * 2;
				Buffer[offset] = (byte)' ';
				Buffer[offset + 1] = ToAttribute((Color)color, (Color)color);
			}
			else
			{
				// double buffered
				if (Mode.ID == VideoMode.Pixel320x200x256DB_ID)
				{
					BackBuffer.Base[(uint)(x + (y * Mode.Width))] = (byte)color;
				}
				else
				{
					uint offset = (uint)(x + (y * Mode.Width));
					Buffer[offset] = color;
				}
			}
		}

		// draw pixel
		public static void DrawPixel(int x, int y, Color color)
		{
			if (x < 0 || x >= Mode.Width || y < 0 || y >= Mode.Height) { return; }
			if (Mode.IsTextMode)
			{
				uint offset = (uint)(x + (y * Mode.Width)) * 2;
				Buffer[offset] = (byte)' ';
				Buffer[offset + 1] = ToAttribute(color, color);
			}
			else
			{
				// double buffered
				if (Mode.ID == VideoMode.Pixel320x200x256DB_ID)
				{
					BackBuffer.WriteInt8((uint)(x + (y * Mode.Width)), (byte)color);
				}
				else
				{
					uint offset = (uint)(x + (y * Mode.Width));
					Buffer[offset] = (byte)color;
				}
			}
		}

		// convert colors to attribute
		public static byte ToAttribute(Color fg, Color bg) { return (byte)((byte)fg | (byte)bg << 4); }

		// set buffer address
		private static void SetBuffer(byte* addr) { Buffer = addr; }

		// set cursor position
		public static void SetCursorPos(int x, int y)
		{
			if (Mode.IsTextMode)
			{
				uint offset = (uint)(x + (y * Mode.Width));
				CRTCWrite.Byte = 14;
				CRTCData.Byte = (byte)((offset & 0xFF00) >> 8);
				CRTCWrite.Byte = 15;
				CRTCData.Byte = (byte)(offset & 0x00FF);
			}
		}

		// get value at position in video memory
		public static byte GetValue(uint addr)
		{
			byte val = 0;
			// double buffered
			if (Mode.ID == VideoMode.Pixel320x200x256DB_ID) { val = BackBuffer.ReadInt8(addr); }
			else { val = Buffer[addr]; }
			return val;
		}

		// get pixel at position
		public static Color GetPixel(int x, int y)
		{
			if (Mode.ID != VideoMode.Pixel320x200x256DB_ID) { return Color.Black; }
			return (Color)BackBuffer.ReadInt8((uint)(x + (y * Mode.Width)));
		}

		// disable cursor
		public static void DisableCursor()
		{
			CRTCWrite.Byte = 0x0A;
			CRTCData.Byte = 0x20;
		}

		// set color palette
		public static void SetColorPalette(uint[] colors)
		{
			// set palette
			for (int i = 0; i < colors.Length; i++)
			{
				if (!Mode.IsTextMode) { MaskData.Byte = 0xFF; } else { MaskData.Byte = 0x0F; }
				DACWrite.Byte = (byte)i;
				DACData.Byte = (byte)((colors[i] & 0xFF0000) >> 16);
				DACData.Byte = (byte)((colors[i] & 0x00FF00) >> 8);
				DACData.Byte = (byte)(colors[i] & 0x0000FF);
			}
		}

		// clear color palette
		public static void ClearColorPalette()
		{
			// set palette
			for (int i = 0; i < 256; i++)
			{
				if (!Mode.IsTextMode) { MaskData.Byte = 0xFF; } else { MaskData.Byte = 0x0F; }
				DACWrite.Byte = (byte)i;
				DACData.Byte = 0;
				DACData.Byte = 0;
				DACData.Byte = 0;
			}
		}

		// get frame buffer segment
		private static byte* GetFrameBufferSegment()
		{
			GFXWrite.Byte = 0x06;
			byte segmentNumber = (byte)(GFXData.Byte & (3 << 2));
			switch (segmentNumber)
			{
				default:
				case 0 << 2: return (byte*)0x00000;
				case 1 << 2: return (byte*)0xA0000;
				case 2 << 2: return (byte*)0xB0000;
				case 3 << 2: return (byte*)0xB8000;
			}
		}

		// write data to devide registers
		private static void WriteRegisters(byte* regs)
		{
			// misc
			MiscWrite.Byte = *(regs++);

			// sequencer
			for (byte i = 0; i < 5; i++) { SequencerWrite.Byte = i; SequencerData.Byte = *(regs++); }

			// crtc
			CRTCWrite.Byte = 0x03;
			CRTCData.Byte = (byte)(CRTCData.Byte | 0x80);
			CRTCWrite.Byte = 0x11;
			CRTCData.Byte = (byte)(CRTCData.Byte & ~0x80);

			// registers
			regs[0x03] = (byte)(regs[0x03] | 0x80);
			regs[0x11] = (byte)(regs[0x11] & ~0x80);
			for (byte i = 0; i < 25; i++) { CRTCWrite.Byte = i; CRTCData.Byte = *(regs++); }

			// garbage collector
			for (byte i = 0; i < 9; i++) { GFXWrite.Byte = i; GFXData.Byte = *(regs++); }

			// attribute controller
			byte foo = 0;
			for (byte i = 0; i < 21; i++)
			{
				foo = InstatRead.Byte;
				AttributeWrite.Byte = i;
				AttributeWrite.Byte = *(regs++);
			}

			foo = InstatRead.Byte;
			AttributeWrite.Byte = 0x20;

		}

		// set plane data
		private static void SetPlane(byte ap)
		{
			byte mask;
			ap &= 3;
			mask = (byte)(1 << ap);

			// set read plane
			GFXWrite.Byte = 4;
			GFXData.Byte = ap;

			// set write plane
			SequencerWrite.Byte = 2;
			SequencerData.Byte = mask;
		}

		// set font
		private static void SetFont(byte[] font, int height)
		{
			byte seq2, seq4, gc4, gc5, gc6;

			// save registers
			SequencerWrite.Byte = 2;
			seq2 = SequencerData.Byte;
			SequencerWrite.Byte = 4;
			seq4 = SequencerData.Byte;

			// set flat addressing
			SequencerData.Byte = (byte)(seq4 | 0x04);
			GFXWrite.Byte = 4;
			gc4 = GFXData.Byte;
			GFXWrite.Byte = 5;
			gc5 = GFXData.Byte;

			// turn off even-odd addressing 5
			GFXData.Byte = (byte)(gc5 & ~0x10);
			GFXWrite.Byte = 6;
			gc6 = GFXData.Byte;

			// turn off even-odd addressing 6
			GFXData.Byte = (byte)(gc6 & ~0x02);

			// write font to plane 4
			SetPlane(2);

			// write font 0
			var seg = GetFrameBufferSegment();
			for (uint j = 0; j < height; j++)
			{ for (uint i = 0; i < 256; i++) { seg[(i * 32) + j] = font[(i * height) + j]; } }

			// restore registers
			SequencerWrite.Byte = 2;
			SequencerData.Byte = seq2;
			SequencerWrite.Byte = 4;
			SequencerData.Byte = seq4;
			GFXWrite.Byte = 4;
			GFXData.Byte = gc4;
			GFXWrite.Byte = 5;
			GFXData.Byte = gc5;
			GFXWrite.Byte = 6;
			GFXData.Byte = gc6;
		}
	}

	// register dumps for video modes
	public static class VideoModeInitializers
	{
		// text 80x25
		public static readonly byte[] Mode80x25_Text = new byte[]
		{
			/* MISC */
			0x67,
			/* SEQ */
			0x03, 0x00, 0x03, 0x00, 0x02,
			/* CRTC */
			0x5F, 0x4F, 0x50, 0x82, 0x55, 0x81, 0xBF, 0x1F,
			0x00, 0x4F, 0x0D, 0x0E, 0x00, 0x00, 0x00, 0x50,
			0x9C, 0x0E, 0x8F, 0x28, 0x1F, 0x96, 0xB9, 0xA3,
			0xFF,
			/* GC */
			0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x0E, 0x00,
			0xFF,
			/* AC */
			0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x14, 0x07,
			0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
			0x0C, 0x00, 0x0F, 0x08, 0x00
		};

		// text 80x50
		public static readonly byte[] Mode80x50_Text = new byte[]
		{
			/* MISC */
			0x67,
			/* SEQ */
			0x03, 0x00, 0x03, 0x00, 0x02,
			/* CRTC */
			0x5F, 0x4F, 0x50, 0x82, 0x55, 0x81, 0xBF, 0x1F,
			0x00, 0x47, 0x06, 0x07, 0x00, 0x00, 0x01, 0x40,
			0x9C, 0x8E, 0x8F, 0x28, 0x1F, 0x96, 0xB9, 0xA3,
			0xFF, 
			/* GC */
			0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x0E, 0x00,
			0xFF, 
			/* AC */
			0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x14, 0x07,
			0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
			0x0C, 0x00, 0x0F, 0x08, 0x00,
		};

		// text 90x60
		public static readonly byte[] Mode90x60_Text = new byte[]
		{
			/* MISC */
			0xE7,
			/* SEQ */
			0x03, 0x01, 0x03, 0x00, 0x02,
			/* CRTC */
			0x6B, 0x59, 0x5A, 0x82, 0x60, 0x8D, 0x0B, 0x3E,
			0x00, 0x47, 0x06, 0x07, 0x00, 0x00, 0x00, 0x00,
			0xEA, 0x0C, 0xDF, 0x2D, 0x08, 0xE8, 0x05, 0xA3,
			0xFF,
			/* GC */
			0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x0E, 0x00,
			0xFF,
			/* AC */
			0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x14, 0x07,
			0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
			0x0C, 0x00, 0x0F, 0x08, 0x00,
		};

		// pixel 320x200x256
		public static readonly byte[] Mode320x200x256_Pixel = new byte[]
		{
			/* MISC */
			0x63,
			/* SEQ */
			0x03, 0x01, 0x0F, 0x00, 0x0E,
			/* CRTC */
			0x5F, 0x4F, 0x50, 0x82, 0x54, 0x80, 0xBF, 0x1F,
			0x00, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0x9C, 0x0E, 0x8F, 0x28, 0x40, 0x96, 0xB9, 0xA3,
			0xFF,
			/* GC */
			0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x05, 0x0F,
			0xFF,
			/* AC */
			0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
			0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
			0x41, 0x00, 0x0F, 0x00, 0x00
		};

		// pixel 320x200x256 X
		public static readonly byte[] Mode320x200x256X_Pixel = new byte[]
		{
			/* MISC */
			0x63,
			/* SEQ */
			0x03, 0x01, 0x0F, 0x00, 0x06,
			/* CRTC */
			0x5F, 0x4F, 0x50, 0x82, 0x54, 0x80, 0xBF, 0x1F,
			0x00, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0x9C, 0x0E, 0x8F, 0x28, 0x00, 0x96, 0xB9, 0xE3,
			0xFF,
			/* GC */
			0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x05, 0x0F,
			0xFF,
			/* AC */
			0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
			0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
			0x41, 0x00, 0x0F, 0x00, 0x00
		};
	}

	// video mode class
	public class VideoMode
	{
		// properties
		public byte ID { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public ushort Depth { get; private set; }
		public byte[] Data { get; private set; }
		public bool IsTextMode { get; private set; }
		public byte Font { get; private set; }

		// constructor
		public VideoMode(byte id = 0, int width = 80, int height = 25, ushort depth = 8, bool textMode = true, byte font = 0)
		{
			this.ID = id;
			this.Width = width;
			this.Height = height;
			this.Depth = depth;
			this.IsTextMode = textMode;
			this.Data = new byte[0];
			this.Font = font;
		}

		// set mode data
		public void SetData(byte[] data) { this.Data = data; }

		// text modes
		public static VideoMode Text80x25 = new VideoMode(Text80x25_ID, 80, 25, 8, true, 0);
		public static VideoMode Text80x50 = new VideoMode(Text80x50_ID, 80, 50, 8, true, 1);
		public static VideoMode Text90x60 = new VideoMode(Text90x60_ID, 90, 60, 8, true, 1);

		// graphics modes
		public static VideoMode Pixel320x200x256 = new VideoMode(Pixel320x200x256_ID, 320, 200, 256, false, 1);
		public static VideoMode Pixel320x200x256X = new VideoMode(Pixel320x200x256X_ID, 320, 200, 256, false, 1);
		public static VideoMode Pixel320x200x256DB = new VideoMode(Pixel320x200x256DB_ID, 320, 200, 256, false, 1);

		// mode identifiers
		public const byte Text80x25_ID = 0;
		public const byte Text80x50_ID = 1;
		public const byte Text90x60_ID = 2;
		public const byte Pixel320x200x256_ID = 3;
		public const byte Pixel320x200x256DB_ID = 4;
		public const byte Pixel320x200x256X_ID = 5;
	}
}
