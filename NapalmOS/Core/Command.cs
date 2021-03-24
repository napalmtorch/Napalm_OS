using System;
using System.Collections.Generic;
using System.Text;

namespace NapalmOS.Core
{
    public abstract class Command
    {
        // properties
        public string Name { get; protected set; }
        public string Help { get; protected set; }
        public string Usage { get; protected set; }

        // constructor
        public Command(string name)
        {
            this.Name = name;
            this.Help = "";
            this.Usage = "";
        }

        // execute
        public abstract void Execute(string line, string[] args);
    }
}
