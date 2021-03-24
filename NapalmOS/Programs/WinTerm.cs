using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.Graphics;
using NapalmOS.GUI;
using NapalmOS.Hardware;

namespace NapalmOS.Programs
{
    public class WinTerm : Window
    {
        // commands
        public static readonly List<Command> Commands = new List<Command>()
        {
            new Commands.Clear(),
            new Commands.Echo(),
            new Commands.Help(),
            new Commands.ListDir(),
            new Commands.ShellScript(),
        };

        // buffer
        public const int BufferWidth = 50;
        public const int BufferHeight = 18;
        public MemoryBlock Buffer, TempBuffer;
        public static string CurrentPath = "0:\\";
        public static Color TextColor = Color.White;
        public static Color BackColor = Color.Black;

        // cursor
        public static bool CursorVisible = true;
        public static int CursorX { get; private set; }
        public static int CursorY { get; private set; }
        private static bool cursorFlash;
        private static int time, timeOld;

        // input
        KeyboardStream kbReader = new KeyboardStream();
        public string input = "";

        // constructor
        public WinTerm() : base(32, 32, 204, 150, "TERMINAL", "term.app")
        {
            // update client bounds
            UpdateClientBounds();

            // allocate memory
            Buffer = MemoryManager.AllocateBlock(4096);
            TempBuffer = MemoryManager.AllocateBlock(4096);
            Clear(TextColor, BackColor);
        }

        public override void Start(string[] args)
        {
            base.Start(args);
            this.Bounds.Width = 204; this.Bounds.Height = 150;
            this.StartBounds.Width = this.Width;
            this.StartBounds.Height = this.Height;
            Clear(TextColor, BackColor);
            Write("Napalm OS Terminal\n", Color.Magenta);
            Write("version 1.2\n", Color.Silver);
            Write(CurrentPath + "> ", Color.Yellow);
            cursorFlash = true;
        }

        // update
        public override void Update()
        {
            base.Update();

            // retrieve memory usage
            Task.MemoryUsed = Buffer.GetUsed() + TempBuffer.GetUsed();
            Task.MemoryFree = Buffer.GetFree() + TempBuffer.GetFree();
            Task.MemoryTotal = Buffer.GetTotal() + TempBuffer.GetTotal();

            if (Flags.State != WindowState.Minimized)
            {
                // set back color
                Style.Colors[0] = BackColor;

                // get input
                if (WindowManager.ActiveIndex == Index)
                {
                    kbReader.Read();
                    if (KeyboardPS2.CurrentKey.Key == Cosmos.System.ConsoleKeyEx.Backspace && input.Length > 0) { Backspace(); }
                    input = kbReader.Output;
                    if (KeyboardPS2.CurrentKey != null)
                    {
                        if (KeyboardPS2.EnterPressed) { kbReader.Output = ""; NewLine(); ParseCommand(input); input = ""; Write(CurrentPath + "> ", Color.Yellow); }
                        else if (KeyboardPS2.CurrentKey.Key != Cosmos.System.ConsoleKeyEx.Backspace) { Write(input[input.Length - 1].ToString()); }
                    }

                    // flash cursor
                    time = Clock.GetSecond();
                    if (time != timeOld)
                    {
                        cursorFlash = !cursorFlash;
                        timeOld = time;
                    }

                }
            }
            this.SetSize((BufferWidth * 4) + 4, (BufferHeight * 6) + 4 + TitleBar.Height);
        }

        // draw
        public override void Draw()
        {
            base.Draw();

            if (Flags.State != WindowState.Minimized && !Flags.Moving && !Flags.Resizing)
            {
                DrawBuffer();

                // draw cursor
                if (cursorFlash)
                {
                    int cx = (CursorX * 4);
                    int cy = (CursorY * 6);
                    WindowRenderer.DrawChar(cx, cy, '_', TextColor, Font.Font3x5, this);
                }
            }
        }

        // draw buffer
        private unsafe void DrawBuffer()
        {
            int xx = 0, yy = 0;
            char c = (char)0;
            Color color = Color.White;
            for (int i = 0; i < (BufferWidth * BufferHeight) * 2; i += 2)
            {
                xx = ((i / 2) % BufferWidth);
                yy = ((i / 2) / BufferWidth);
                c = (char)Buffer.Base[(uint)i];
                color = (Color)Buffer.Base[(uint)i + 1];
                if (c > 0 && c != 0x20)
                { WindowRenderer.DrawChar((xx * 4), (yy * 6), c, color, Font.Font3x5, this); }
            }
        }

        // clear
        public unsafe void Clear(Color fg, Color bg)
        {
            // set back color
            BackColor = bg;
            Style.Colors[0] = bg;

            // clear and set fore olor
            for (uint i = 0; i < (BufferWidth * BufferHeight) * 2; i += 2)
            {
                Buffer.Base[i] = 0x00;
                Buffer.Base[i + 1] = (byte)fg;
            }
            CursorX = 0; CursorY = 0;
        }

        // write string of text to buffer
        public unsafe void Write(string text, Color fg)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n') { NewLine(); }
                else
                {
                    uint pos = (uint)(CursorX + (CursorY * BufferWidth)) * 2;
                    Buffer.Base[pos] = (byte)text[i];
                    Buffer.Base[pos + 1] = (byte)fg;
                    CursorX++;
                    if (CursorX >= BufferWidth) { NewLine(); }
                }
            }
        }

        // write string of text to buffer with default color
        public void Write(string text) { Write(text, TextColor); }

        // write line of text to buffer
        public void WriteLine(string text, Color color) { Write(text + "\n", color); }
        public void WriteLine(string text) { Write(text + "\n"); }

        // put char at positoin
        public unsafe void PutChar(int x, int y, char c, Color color)
        {
            uint pos = (uint)(x + (y * BufferWidth)) * 2;
            if (pos >= BufferWidth * BufferHeight) { return; }
            Buffer.Base[pos] = (byte)c;
            Buffer.Base[pos + 1] = (byte)color;
        }

        // set cursor position
        public void SetCursorPos(int x, int y) { CursorX = x; CursorY = y; }

        // generate new line
        private void NewLine()
        {
            CursorX = 0; CursorY++;
            if (CursorY >= BufferHeight)
            {
                Scroll(1);
                SetCursorPos(0, BufferHeight - 1);
            }
        }

        // scroll by amount
        private unsafe void Scroll(int amount)
        {
            for (int t = 0; t < amount; t++)
            {
                // copy to temp
                for (uint i = 0; i < (BufferWidth * BufferHeight) * 2; i++)
                {
                    if (i >= (BufferWidth * 2)) { TempBuffer.Base[i - (BufferWidth * 2)] = Buffer.Base[i]; }
                }

                // clear buffer
                Buffer.Fill(0x20);

                // copy scrolled version back
                for (uint i = 0; i < (BufferWidth * BufferHeight) * 2; i++) { Buffer.Base[i] = TempBuffer.Base[i]; }
            }
        }

        // backspace by amount
        private unsafe void Backspace()
        {
            if (CursorX > 0) { CursorX--; Buffer.Base[(uint)(CursorX + (CursorY * BufferWidth)) * 2] = 0x20; }
            else if (CursorY > 0) { CursorX = BufferWidth - 1; CursorY--; Buffer.Base[(uint)(CursorX + (CursorY * BufferWidth)) * 2] = 0x20; }
        }

        // parse command
        public static void ParseCommand(string cmd)
        {
            string[] args = cmd.Split(' ');
            for (int i = 0; i < Commands.Count; i++)
            {
                if (args[0].ToUpper() == Commands[i].Name.ToUpper())
                {
                    Commands[i].Execute(cmd, args);
                    return;
                }
            }

            if (cmd.Length > 0) { ((WinTerm)WindowManager.GetWindow("term.app")).WriteLine("Invalid command", Color.Red); }
        }

        // parse shell script
        public static void ParseScript(string file)
        {
            if (FileSystem.FileExists(file))
            {
                string[] initFile = FileSystem.ReadAllLines(file);
                for (int i = 0; i < initFile.Length; i++)
                {
                    ParseCommand(initFile[i]);
                }
            }
        }
    }
}
