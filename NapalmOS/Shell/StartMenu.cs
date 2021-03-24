using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.GUI;
using NapalmOS.Graphics;
using NapalmOS.Hardware;
using NapalmOS.Programs;

namespace NapalmOS
{
    public static class StartMenu
    {
        // dimensions
        public static int Y { get; private set; }
        public static int Height { get; private set; }
        public const int Width = 64;
        public const int ItemHeight = 11;
        public static bool Visible = true;
        public static int HoverCount { get; private set; }

        // items
        public static List<ListItem> Items = new List<ListItem>();

        // initialization - must be done after window manager init
        public static void Initialize()
        {
            PopulateList();
        }

        // update
        public static void Update()
        {
            HoverCount = 0;
            if (Visible)
            {
                // set height
                Height = (Items.Count * ItemHeight) + 2;

                // update items
                int xx = 1, yy = VGADriver.Mode.Height - Taskbar.Height - (Height - 1);
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].X = xx;
                    Items[i].Y = yy;

                    Items[i].Hover = MathUtil.RectangleContains(xx, yy, Width, ItemHeight, MousePS2.X, MousePS2.Y);
                    Items[i].Down = Items[i].Hover && MousePS2.State == Cosmos.System.MouseState.Left;

                    if (Items[i].Hover) { HoverCount++; }

                    // on item click
                    if (Items[i].Down)
                    {
                        // reboot
                        if (Items[i].Text == "REBOOT")
                        {

                        }
                        // power off
                        else if (Items[i].Text == "POWER OFF")
                        {

                        }
                        // program
                        else { WindowManager.Start(Items[i].Tag1, null); }

                        // close menu
                        Items[i].Hover = false;
                        Items[i].Down = false;
                        Visible = false;
                    }

                    // increment height
                    yy += ItemHeight;
                }

                // hide if clicked away
                if (HoverCount == 0 && MousePS2.State == Cosmos.System.MouseState.Left && !Taskbar.BtnStart.MouseFlags.Hover) { Visible = false; }
            }
        }

        // draw
        public static void Draw()
        {
            if (Visible)
            {
                // draw border
                Renderer.DrawRect3D(0, VGADriver.Mode.Height - Taskbar.Height - Height, Width + 2, Height, VisualStyle.Button.Colors[2], VisualStyle.Button.Colors[3], false);

                // draw items
                for (int i = 0; i < Items.Count; i++)
                {
                    Color bg = VisualStyle.Button.Colors[0];
                    Color fg = VisualStyle.Button.Colors[1];
                    if (Items[i].Hover) { bg = VisualStyle.Window.Colors[4]; fg = VisualStyle.Window.Colors[5]; }

                    Renderer.DrawFilledRect(Items[i].X, Items[i].Y, Width, ItemHeight, bg);
                    Renderer.DrawString(Items[i].X + 2, Items[i].Y + 3, Items[i].Text, fg, Font.Font3x5);
                }
            }
        }

        // populate list
        public static void PopulateList()
        {
            Items.Clear();
            for (int i = 0; i < WindowManager.Windows.Count; i++)
            {
                ListItem item = new ListItem(WindowManager.Windows[i].Title);
                item.Tag1 = WindowManager.Windows[i].ID;
                item.Tag2 = WindowManager.Windows[i].Index.ToString();
                Items.Add(item);
            }

            // add non-program items
            Items.Add(new ListItem("REBOOT"));
            Items.Add(new ListItem("POWER OFF"));
        }
    }
}
