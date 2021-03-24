using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.GUI;
using NapalmOS.Hardware;
using NapalmOS.Graphics;
using NapalmOS.Programs;

namespace NapalmOS.Commands
{
    public class ListDir : Command
    {
        string[] files, folders;

        public ListDir() : base("DIR")
        {
            this.Help = "SHOW DIRECTORY CONTENTS";
            this.Usage = "dir [path]";
        }

        public override void Execute(string line, string[] args)
        {
            string path = FileSystem.ConvertArgumentToPath(args);
            if (args.Length == 1) { path = WinTerm.CurrentPath; }
            if (FileSystem.DirectoryExists(path))
            {
                folders = FileSystem.GetDirectories(path);
                files = FileSystem.GetFiles(path);

                for (int i = 0; i < folders.Length; i++)
                {
                    ((WinTerm)WindowManager.GetWindow("term.app")).WriteLine(folders[i], Color.Yellow);
                }

                for (int i = 0; i < files.Length; i++)
                {
                    ((WinTerm)WindowManager.GetWindow("term.app")).WriteLine(files[i]);
                }
            }
            else
            {
                ((WinTerm)WindowManager.GetWindow("term.app")).WriteLine("Invalid directory");
            }
        }
    }
}
