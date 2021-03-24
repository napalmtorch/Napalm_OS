using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.GUI;
using NapalmOS.Graphics;
using NapalmOS.Hardware;

namespace NapalmOS
{
    public static class Shell
    {
        // task
        public static Task Task = new Task("Shell", "wmshell.sys");

        // back color
        public static Color BackColor = Color.Cyan6;

        // initialization
        public static void Initialize()
        {
            // register task
            TaskManager.RegisterTask(Task);

            // initialize taskbar
            Taskbar.Initialize();

            // initialize start menu
            StartMenu.Initialize();

            // start programs
            WindowManager.Start("taskman.app", null);
            WindowManager.Start("term.app", null);

            
        }

        // update
        public static void Update()
        {
            // update taskbar and start menu
            Taskbar.Update();
            StartMenu.Update();

            // update windows
            WindowManager.Update();

            // update performance info
            Renderer.UpdatePerformanceInfo();
        }

        // draw
        public static void Draw()
        {
            // clear usable space
            Clear();

            // draw windows
            WindowManager.Draw();

            if (!WindowManager.Fullscreen)
            {
                // draw taskbar and start menu
                Taskbar.Draw();
                StartMenu.Draw();

                // draw performance info
                Renderer.DrawPerformanceInfo(2, 2, Color.White, Font.Font3x5);

                // draw mouse
                MousePS2.Display();
            }

            // swap buffers
            Renderer.Swap(true);
        }

        // clear
        public static unsafe void Clear()
        {
            for (int i = 0; i < (VGADriver.Mode.Width * VGADriver.Mode.Height) - (VGADriver.Mode.Width * (Taskbar.Height + 1)); i++)
            {
                VGADriver.BackBuffer.Base[i] = (byte)BackColor;
            }
        }
    }
}
