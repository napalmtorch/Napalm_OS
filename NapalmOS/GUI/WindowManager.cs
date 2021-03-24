using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.Hardware;
using NapalmOS.Programs;

namespace NapalmOS.GUI
{
    public static class WindowManager
    {
        // instances
        public static List<Window> Windows { get; private set; } = new List<Window>();

        // counters
        public static int RunningCount { get; private set; }
        public static int MovingCount { get; private set; }
        public static int HoverCount { get; private set; }
        public static int ResizeCount { get; private set; }
        public static int Count { get { return Windows.Count; } }

        // properties
        public static int ActiveIndex = -1;
        public static int FullscreenIndex = 0;
        public static bool Fullscreen = false;
        public static bool DialogOpen = false;
        private static bool clickFlag = false;

        // task
        public static Task Task = new Task("Window Manager", "wmhost.sys");

        // initialization
        public static void Initialize()
        {
            ActiveIndex = -1;
            RunningCount = 0;

            // register task
            TaskManager.RegisterTask(Task);

            // add default applications
            Add(new Programs.WinDemo());
            Add(new Programs.WinTaskMan());
            Add(new Programs.WinTerm());
        }

        // update
        public static void Update()
        {
            // update counters
            UpdateCounters();
            Fullscreen = false;

            // draw all windows normally
            // update window flags
            for (int i = 0; i < Windows.Count; i++)
            {
                // set index
                Windows[i].Index = i;

                // while running
                if (Windows[i].Flags.Running)
                {
                    // check window state
                    Windows[i].MouseFlags.Hover = MathUtil.RectangleContains(Windows[i].Bounds, MousePS2.X, MousePS2.Y);
                }
            }

            // determine active window and update
            for (int i = 0; i < Windows.Count; i++)
            {
                // no windows already moving
                if (MovingCount <= 0)
                {
                    if (MousePS2.State == Cosmos.System.MouseState.Left && !clickFlag && Windows[i].MouseFlags.Hover && i != ActiveIndex && Windows[i].Flags.Running && Windows[i].Flags.State != WindowState.Minimized)
                    {
                        if (ActiveIndex >= 0 && ActiveIndex < Windows.Count)
                        {
                            if (HoverCount < 2 && !Windows[ActiveIndex].Flags.Moving && !Windows[ActiveIndex].Flags.Resizing)
                            {
                                ActiveIndex = i;
                                clickFlag = true;
                            }
                        }
                        else { if (MovingCount == 0) { ActiveIndex = i; clickFlag = true; } }
                    }
                }

                // update window
                if (Windows[i].Flags.Running) { Windows[i].Update(); }
            }

            if (MousePS2.State == Cosmos.System.MouseState.None) { clickFlag = false; }
        }

        // update counters
        private static void UpdateCounters()
        {
            // reset counters
            RunningCount = 0;
            MovingCount = 0;
            HoverCount = 0;
            ResizeCount = 0;
            FullscreenIndex = -1;

            // loop through all windows
            for (int i = 0; i < Windows.Count; i++)
            {
                if (Windows[i].Flags.Running) { RunningCount++; }
                if (Windows[i].Flags.Moving) { MovingCount++; }
                if (Windows[i].Flags.Resizing) { ResizeCount++; }
                if (Windows[i].MouseFlags.Hover) { HoverCount++; }
                if (Windows[i].Flags.State == WindowState.Fullscreen) { FullscreenIndex = i; }
            }
        }

        // draw
        public static void Draw()
        {
            // loop through and draw inactive windows
            for (int i = 0; i < Windows.Count; i++)
            {
                // check draw flags
                if (Windows[i].Flags.Running && Windows[i].Flags.State != WindowState.Minimized)
                {
                    if (i != ActiveIndex)
                    {
                        // draw
                        Windows[i].Draw();
                    }
                }
            }

            // draw active windows
            DrawActive();
        }

        // draw active window
        private static void DrawActive()
        {
            if (ActiveIndex >= 0 && ActiveIndex < Windows.Count)
            {
                if (Windows[ActiveIndex].Flags.Running)
                {
                   if (Windows[ActiveIndex].Flags.Running && Windows[ActiveIndex].Flags.State != WindowState.Minimized)
                    { Windows[ActiveIndex].Draw(); }
                }
            }
        }

        // add window
        public static void Add(Window win) { Windows.Add(win); }

        // start window based on id
        public static bool Start(string id, string[] args)
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                // found match
                if (Windows[i].ID == id)
                {
                    // window is already running set as active
                    if (Windows[i].Flags.Running) { Windows[i].Index = i; WinTaskMan.Changed = true; ActiveIndex = i; }
                    // start window
                    else { Windows[i].Index = i; Windows[i].Start(args); TaskManager.RegisterTask(Windows[i].Task); ActiveIndex = i; WinTaskMan.Changed = true; }
                    return true;
                }
            }
            return false;
        }

        // close window based on id
        public static bool Close(string id)
        {
            // loop through windows
            for (int i = 0; i < Windows.Count; i++)
            {
                // found match
                if (Windows[i].ID == id)
                {
                    // window is running
                    if (Windows[i].Flags.Running) { WinTaskMan.Changed = true; Windows[i].Close(); TaskManager.EndTask(Windows[i].Task); ActiveIndex = -1; return true; }
                    // window already closed
                    else { return false; }
                }
            }
            // failure
            return false;
        }

        // get window by id
        public static Window GetWindow(string id)
        {
            for (int i = 0; i < Windows.Count; i++) { if (Windows[i].ID == id) { return Windows[i]; } }
            return null;
        }
    }
}
