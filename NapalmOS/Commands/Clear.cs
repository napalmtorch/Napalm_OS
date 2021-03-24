using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.GUI;

namespace NapalmOS.Commands
{
    public class Clear : Command
    {
        public Clear() : base("CLS")
        {
            this.Help = "CLEAR THE SCREEN";
            this.Usage = "";
        }

        public override void Execute(string line, string[] args)
        {
            ((Programs.WinTerm)WindowManager.GetWindow("term.app")).Clear(Programs.WinTerm.TextColor, Programs.WinTerm.BackColor);
        }
    }
}
