using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.Graphics;
using Sys = Cosmos.System;
using KB = Cosmos.System.KeyboardManager;
using KEY = Cosmos.System.KeyEvent;

namespace NapalmOS.Hardware
{
    public static class KeyboardPS2
    {
        // key flags
        public static bool CapsLockPressed = false;
        public static bool ShiftPressed = false;
        public static bool EnterPressed = false;
        public static bool EscapePressed = false;
        public static bool ControlPressed = false;
        public static bool AltPressed = false;

        // current key
        public static KEY CurrentKey;
        public static KEY PreviousKey;

        // caret image
        public static Image ImgCaret = new Image(2, 5);
        private static byte[] ImgCaretData = new byte[2 * 5]
        {
            0xFF, 0xFF,
            0xFF, 0x00,
            0xFF, 0x00,
            0xFF, 0x00,
            0xFF, 0xFF,
        };

        // task
        public static Task Task = new Task("KB Driver", "kbdrv.sys"); 

        // initialization
        public static void Initialize()
        {
            // create caret image
            ImgCaret.LoadData(2, 5, ImgCaretData);

            // register task
            TaskManager.RegisterTask(Task);
        }

        public static void Update()
        {
            // get current key down
            if (KB.KeyAvailable)
            {
                if (KB.TryReadKey(out CurrentKey))
                {
                    PreviousKey = CurrentKey;
                }
            }
            else { CurrentKey = null; }

            // check enter
            EnterPressed = (CurrentKey.Key == Sys.ConsoleKeyEx.Enter);

            // check esapce
            EscapePressed = (CurrentKey.Key == Sys.ConsoleKeyEx.Escape);

            // check control keys
            CapsLockPressed = KB.CapsLock;
            ShiftPressed = KB.ShiftPressed;
            ControlPressed = KB.ControlPressed;
            AltPressed = KB.AltPressed;
        }

        public static bool IsKeyDown(Sys.ConsoleKeyEx key)
        {
            if (CurrentKey.Key == key) { return true; }
            else { return false; }
        }

        public static bool IsKeyDown(char key)
        {
            if (CurrentKey.KeyChar == key) { return true; }
            else { return false; }
        }

        public static bool IsKeyUp(Sys.ConsoleKeyEx key)
        {
            if (CurrentKey.Key != key) { return true; }
            else { return false; }
        }
    }
}
