using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Graphics;
using NapalmOS.Hardware;

namespace NapalmOS.GUI
{
    public class TextBox : Control
    {
        // input
        public KeyboardStream InputStream { get; private set; }
        private int txtW, maxLen;
        private string drawText;
        public bool ReadOnly = false;

        // cursor flash
        private bool cursor;
        private int time, timeOld;

        // constructor
        public TextBox(int x, int y, string text) : base(x, y, 80, 9, ControlType.TextBox)
        {
            // set properties
            this.SetText(text);
            this.Style.Copy(VisualStyle.TextBox);
            this.InputStream = new KeyboardStream();
            this.InputStream.Output = text;
        }

        // update
        public override void Update(Window window)
        {
            // update base
            base.Update(window);

            // force height
            Bounds.Height = 9;

            // apply style if not already
            if (Style.Name == "Unassigned") { Style.Copy(VisualStyle.TextBox); }

            // get font width
            txtW = Font.Font3x5.GetStringWidth(Text);
            maxLen = ((Width - 2) / 4) - 1;

            // check default event flags
            CheckDefaultEvents(window);

            // get keyboard input
            if (Active)
            {
                if (!ReadOnly)
                {
                    InputStream.Read();
                    this.Text = InputStream.Output;
                }

                // flash cursor
                time = Cosmos.HAL.RTC.Second;
                if (time != timeOld)
                {
                    cursor = !cursor;
                    timeOld = time;
                }
            }
            // disable cursor
            else { cursor = false; }
        }

        // draw
        public override void Draw(Window window)
        {
            // draw base
            base.Draw(window);

            // draw box inside
            if (window != null)
            {
                if (window.Style.Colors[0] != Style.Colors[0])
                { WindowRenderer.DrawFilledRect(X + 1, Y + 1, Width - 2, Height - 2, Style.Colors[0], window); }
            }
            else { WindowRenderer.DrawFilledRect(X + 1, Y + 1, Width - 2, Height - 2, Style.Colors[0], window); }

            // draw box
            if (Style.BorderStyle == BorderStyle.Fixed3D) { WindowRenderer.DrawRect3D(Bounds, Style.Colors[2], Style.Colors[3], false, window); }
            else if (Style.BorderStyle == BorderStyle.FixedSingle) { WindowRenderer.DrawRect(Bounds, 1, Style.Colors[3], window); }
            else if (Style.BorderStyle == BorderStyle.None) { WindowRenderer.DrawRect(Bounds, 1, Style.Colors[0], window); }

            // draw text
            int cx = X + txtW + 3;
            drawText = Text;
            if (Text.Length > 0 && Text.Length >= maxLen) 
            {
                if (Active) { drawText = Text.Substring(Text.Length - maxLen, maxLen); cx = Width - 1; }
                else { drawText = Text.Substring(0, maxLen); }
            }

            WindowRenderer.DrawString(X + 2, Y + 2, drawText, Style.Colors[1], Font.Font3x5, window);

            // draw cursor
            if (cursor) { WindowRenderer.DrawImage(cx, Y + 2, Style.Colors[1], Color.Black, KeyboardPS2.ImgCaret, window); }
        }

        // set text
        public void SetText(string text) { this.Text = text; InputStream.Output = text; }
    }
}
