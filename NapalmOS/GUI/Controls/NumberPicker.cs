using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Graphics;
using NapalmOS.Hardware;

namespace NapalmOS.GUI
{
    public class NumberPicker : Control
    {
        // numeric value
        public double Minimum = 0.00;
        public double Maximum = 10.00;
        public double Value = 0.00;
        public double Interval = 0.5;
        int txtW;

        // sub controls
        private TextBox TxtInput;
        private Button BtnDown, BtnUp;

        // constructor
        public NumberPicker(int x, int y, int val) : base(x, y, 58, 9, ControlType.NumberPicker)
        {
            // set properties
            this.Text = "NUM";
            this.Style.Copy(VisualStyle.TextBox);

            // buttons
            this.BtnDown = new Button(X + Width - 18, Y, "<");
            this.BtnUp = new Button(X + Width + 9, Y, ">");
            this.BtnDown.SetSize(9, 9);
            this.BtnUp.SetSize(9, 9);

            // input
            this.TxtInput = new TextBox(x, y, Value.ToString());
            this.TxtInput.SetSize(Width - 18, Height);
            
        }

        // update
        public override void Update(Window window)
        {
            // update base
            base.Update(window);

            // apply style if not already
            if (Style.Name == "Unassigned") { Style.Copy(VisualStyle.TextBox); }

            // get font width
            txtW = Font.Font3x5.GetStringWidth(Text);

            // check default event flags
            CheckDefaultEvents(window);

            // update buttons
            BtnDown.SetPosition(X + Width - 18, Y);
            BtnUp.SetPosition(X + Width - 9, Y);
            BtnDown.Update(window);
            BtnUp.Update(window);

            // handle value button clicks
            HandleButtonClicks();

            // update text input
            TxtInput.SetPosition(X, Y);
            TxtInput.SetSize(Width - 18, Height);
            TxtInput.Update(window);
            TxtInput.Active = Active;

            if (Active)
            {
                TxtInput.ReadOnly = false;
                if (KeyboardPS2.IsKeyDown(Cosmos.System.ConsoleKeyEx.Enter)) { SetValue(TxtInput.Text); Active = false; }
            }
            else { TxtInput.ReadOnly = true; }
        }

        // draw
        public override void Draw(Window window)
        {
            // draw base
            base.Draw(window);

            // draw text box
            TxtInput.Draw(window);

            // draw buttons
            BtnDown.Draw(window);
            BtnUp.Draw(window);
        }
        
        // safely set value based on text
        public bool SetValue(string text)
        {
            double val = 0.00;
            if (!double.TryParse(text, out val)) { val = Value; }
            if (val < Minimum) { val = Minimum; }
            if (val > Maximum) { val = Maximum; }

            SetValue(val);
            return true;
        }

        // set value
        public void SetValue(double val) { this.Value = val; TxtInput.SetText(val.ToString()); }

        // handle value button clicks
        private void HandleButtonClicks()
        {
            // value down
            if (BtnDown.MouseFlags.Down && !BtnDown.MouseFlags.Clicked)
            {
                // decrement
                Value -= Interval;

                // clamp and set
                if (Value < Minimum) { Value = Minimum; }
                TxtInput.SetText(Value.ToString());

                // mouse flag
                BtnDown.MouseFlags.Clicked = true;
            }

            // value up
            if (BtnUp.MouseFlags.Down && !BtnUp.MouseFlags.Clicked)
            {
                // increment
                Value += Interval;

                // clamp and set
                if (Value > Maximum) { Value = Maximum; }
                TxtInput.SetText(Value.ToString());

                // mouse flag
                BtnUp.MouseFlags.Clicked = true;
            }
        }
    }
}
