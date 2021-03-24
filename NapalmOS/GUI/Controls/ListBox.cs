using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Graphics;
using NapalmOS.Hardware;

namespace NapalmOS.GUI
{
    // structure for list items
    public class ListItem
    {
        // dimensions
        public const int Height = 7;
        public int X, Y;

        // properties
        public string Text;
        public string Tag1, Tag2, Tag3;
        public bool Down, Hover;

        // constructor
        public ListItem(string text) { this.Text = text; }
    }

    // list item control
    public class ListBox : Control
    {
        public List<ListItem> Items { get; private set; } = new List<ListItem>();
        public int SelectedIndex = 0;
        private bool clickFlag = false;

        // constructor
        public ListBox(int x, int y) : base(x, y, 64, 48, ControlType.ListBox)
        {
            // set properties
            this.Style.Copy(VisualStyle.TextBox);
        }

        // update
        public override void Update(Window window)
        {
            // update base
            base.Update(window);

            // apply style if not already
            if (Style.Name == "Unassigned") { Style.Copy(VisualStyle.TextBox); }

            // check default event flags
            CheckDefaultEvents(window);

            // update items
            UpdateItems(window);
        }

        // update items
        private void UpdateItems(Window window)
        {
            int xx = X + 1, yy = Y + 1;
            for (int i = 0; i < (Height - 2) / ListItem.Height; i++)
            {
                if (i < Items.Count)
                {
                    Items[i].Hover = MathUtil.RectangleContains(window.ClientBounds.X + Items[i].X, window.ClientBounds.Y + Items[i].Y, Width - 2, ListItem.Height, MousePS2.X, MousePS2.Y) && MouseFlags.Hover;
                    Items[i].Down = Items[i].Hover && MousePS2.State == Cosmos.System.MouseState.Left;

                    if (Items[i].Down && !clickFlag) { SelectedIndex = i; clickFlag = true; }
                }
            }

            if (MousePS2.State == Cosmos.System.MouseState.None) { clickFlag = false; }
        }

        // draw
        public override void Draw(Window window)
        {
            // draw base
            base.Draw(window);

            // draw background
            if (window != null)
            {
                if (window.Style.Colors[0] != Style.Colors[0])
                { WindowRenderer.DrawFilledRect(X + 1, Y + 1, Width - 2, Height - 2, Style.Colors[0], window); }
            }
            else { WindowRenderer.DrawFilledRect(X + 1, Y + 1, Width - 2, Height - 2, Style.Colors[0], window); }

            // draw border
            WindowRenderer.DrawRect3D(X, Y, Width, Height, Style.Colors[2], Style.Colors[3], false, window);

            // draw items
            DrawItems(window);
        }

        private void DrawItems(Window window)
        {
            int xx = X + 1, yy = Y + 1;
            for (int i = 0; i < (Height - 2) / ListItem.Height; i++)
            {
                if (i < Items.Count)
                {
                    Items[i].X = xx;
                    Items[i].Y = yy;

                    yy += ListItem.Height;

                    if (SelectedIndex == i)
                    {
                        WindowRenderer.DrawFilledRect(Items[i].X, Items[i].Y, Width - 2, ListItem.Height, window.Style.Colors[4], window);
                        WindowRenderer.DrawString(Items[i].X + 1, Items[i].Y + 1, Items[i].Text, window.Style.Colors[5], Font.Font3x5, window);
                    }
                    else { WindowRenderer.DrawString(Items[i].X + 1, Items[i].Y + 1, Items[i].Text, Style.Colors[1], Font.Font3x5, window); }
                }
            }
        }
    }
}
