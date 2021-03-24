using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.GUI;
using NapalmOS.Graphics;
using NapalmOS.Programs;

namespace NapalmOS.Commands
{
    public class Help : Command
    {
        public Help() : base("HELP")
        {
            this.Help = "SHOW HELPFUL INFORMATION";
            this.Usage = "";
        }

        public override void Execute(string line, string[] args)
        {
            if (args.Length == 1)
            {
                // show help menu
                for (int i = 0; i < WinTerm.Commands.Count; i++)
                {
                    ((WinTerm)WindowManager.GetWindow("term.app")).Write("-- " + WinTerm.Commands[i].Name.ToUpper());
                    ((WinTerm)WindowManager.GetWindow("term.app")).SetCursorPos(16, WinTerm.CursorY);
                    ((WinTerm)WindowManager.GetWindow("term.app")).WriteLine(WinTerm.Commands[i].Help, Color.Silver);
                }
            }
            else
            {
                // show help for specific command
                string command = args[1];
                for (int i = 0; i < WinTerm.Commands.Count; i++)
                {
                    if (command.ToUpper() == WinTerm.Commands[i].Name.ToUpper())
                    {
                        ((WinTerm)WindowManager.GetWindow("term.app")).WriteLine(WinTerm.Commands[i].Help, Color.Silver);
                        ((WinTerm)WindowManager.GetWindow("term.app")).WriteLine(WinTerm.Commands[i].Usage, Color.Cyan);
                        return;
                    }
                }
                ((WinTerm)WindowManager.GetWindow("term.app")).WriteLine("Unknown argument");
            }
        }
    }
}
