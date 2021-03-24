using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.GUI;
using NapalmOS.Graphics;
using NapalmOS.Hardware;
using NapalmOS.Programs;

namespace NapalmOS.Commands
{
    public class ShellScript : Command
    {
        public ShellScript() : base("SH")
        {
            this.Help = "EXECUTE SHELL SCRIPT";
            this.Usage = "SH [file]";
        }

        public override void Execute(string line, string[] args)
        {
            if (args.Length > 1)
            {
                string file = FileSystem.ConvertArgumentToPath(args);
                file = file.Remove(file.Length - 1, 1);
                if (FileSystem.FileExists(file))
                {
                    WinTerm.ParseScript(file);
                }
                else { ((WinTerm)WindowManager.GetWindow("term.app")).WriteLine("Invalid filename", Color.Red); }
            }
            else
            {
                ((WinTerm)WindowManager.GetWindow("term.app")).WriteLine("Invalid arguments", Color.Red);
                ((WinTerm)WindowManager.GetWindow("term.app")).WriteLine(Usage, Color.Cyan);
            }
        }
    }
}
