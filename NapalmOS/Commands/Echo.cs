using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.GUI;

namespace NapalmOS.Commands
{
    public class Echo : Command
    {
        public Echo() : base("ECHO")
        {
            this.Help = "REPEAT LINE OF TEXT";
            this.Usage = "ECHO [text]";
        }

        public override void Execute(string line, string[] args)
        {
            if (args.Length > 1)
            {
                string text = "";
                for (int i = 1; i < args.Length; i++) { text += args[i] + " "; }
                ((Programs.WinTerm)WindowManager.GetWindow("term.app")).WriteLine(text);
            }
            else { ((Programs.WinTerm)WindowManager.GetWindow("term.app")).WriteLine(""); }
        }
    }
}
