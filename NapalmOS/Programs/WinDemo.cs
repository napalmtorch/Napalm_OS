using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.Graphics;
using NapalmOS.GUI;
using NapalmOS.Hardware;

namespace NapalmOS.Programs
{
    public class WinDemo : Window
    {
        Button btn;
        CheckBox chk;
        TextBox txt;
        NumberPicker num;

        // constructor
        public WinDemo() : base(32, 32, 140, 80, "GUI DEMO", "demo.app")
        {
            UpdateClientBounds();

            btn = new Button(4, 4, "BUTTON");
            AddControl(btn);

            chk = new CheckBox(4, 14, "CHECK BOX");
            AddControl(chk);

            txt = new TextBox(4, 24, "TEXT BOX");
            AddControl(txt);

            num = new NumberPicker(4, 34, 128);
            AddControl(num);
        }

        // update
        public override void Update()
        {
            base.Update();
        }

        // draw
        public override void Draw()
        {
            base.Draw();
        }
    }
}
