using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using NapalmOS.GUI;
using NapalmOS.Graphics;
using NapalmOS.Hardware;

namespace NapalmOS.Core
{
    public class Kernel : Sys.Kernel
    {
        public static bool GUI = false;
        private static bool InitGUI = false;

        // boot sequence
        private void Boot(bool fast, bool gui)
        {
            // normal boot
            if (fast)
            {
                // initialize vga driver
                VGADriver.Initialize(VideoMode.Text80x25);
                Terminal.Clear();

                // boot message
                BootMessage();

                // allocate vga memory
                ExceptionHandler.ThrowOK("Initialized VGA driver");
                Terminal.WriteLine("Allocating memory for double buffer...", ConsoleColor.Gray);
                VGADriver.AllocateBuffer();

                // initialize ps/2
                KeyboardPS2.Initialize();
                MousePS2.Initialize((uint)VideoMode.Pixel320x200x256DB.Width, (uint)VideoMode.Pixel320x200x256DB.Height);
                ExceptionHandler.ThrowOK("Initialized PS/2 keyboard");
                ExceptionHandler.ThrowOK("Initialized PS/2 mouse");

                // initialize file system
                FileSystem.Initialize();

                // initialize gui
                VisualStyle.InitializeDefaults();
                ExceptionHandler.ThrowOK("Initialized visual style \"Default\"");
                WindowManager.Initialize();
                ExceptionHandler.ThrowOK("Initialized window manager");
                GUI = gui;
                InitGUI = false;
            }
            // slower boot - used mainly for debugging and fun xD
            else
            {
                // initialize vga driver
                VGADriver.Initialize(VideoMode.Text80x25);
                Terminal.Clear();

                // boot message
                BootMessage();

                // allocate vga memory
                WaitMS(300);
                ExceptionHandler.ThrowOK("Initialized VGA driver");
                Wait(1);
                Terminal.WriteLine("Allocating memory for double buffer...", ConsoleColor.Gray);
                VGADriver.AllocateBuffer();

                // initialize ps/2
                KeyboardPS2.Initialize();
                MousePS2.Initialize((uint)VideoMode.Pixel320x200x256DB.Width, (uint)VideoMode.Pixel320x200x256DB.Height);
                ExceptionHandler.ThrowOK("Initialized PS/2 keyboard");
                ExceptionHandler.ThrowOK("Initialized PS/2 mouse");

                // initialize file system
                WaitMS(500);
                FileSystem.Initialize();

                // initialize gui
                WaitMS(300);
                VisualStyle.InitializeDefaults();
                ExceptionHandler.ThrowOK("Initialized visual style \"Default\"");
                WaitMS(300);
                WindowManager.Initialize();
                ExceptionHandler.ThrowOK("Initialized window manager");
                GUI = gui;
                InitGUI = false;
                WaitMS(500);
            }
        }

        // init
        protected override void BeforeRun() { Boot(true, true); }

        // loop
        protected override void Run()
        {
            if (GUI)
            {
                // initialize gui system
                if (!InitGUI)
                {
                    // set video mode
                    VGADriver.SetMode(VideoMode.Pixel320x200x256DB);

                    // confirm initialization
                    InitGUI = true;

                    // initialize shell
                    Shell.Initialize();
                }

                // update
                Update();

                // draw
                Draw();
            }
            else
            {
                // todo - add text mode support
                Terminal.WriteLine("Ready."); Terminal.Write("> ");
                string input = Terminal.ReadLine();
                Terminal.WriteLine("You typed: " + input);
            }
        }

        // update
        private void Update()
        {
            // update shell
            Shell.Update();

            // update keyboard
            KeyboardPS2.Update();
        }

        // draw
        private void Draw()
        {
            // draw shell
            Shell.Draw();
        }

        // bootup message
        public static void BootMessage()
        {
            Terminal.WriteLine("Booting NapalmOS...");
            WaitMS(150);
        }

        // PIT wait wrappers
        public static void WaitNS(int ns) { Cosmos.HAL.Global.PIT.Wait((uint)ns); }
        public static void WaitMS(int ms) { Cosmos.HAL.Global.PIT.Wait((uint)ms); }
        public static void Wait(int secs) { for (int i = 0; i < secs; i++) { WaitMS(1000); } }
    }
}
