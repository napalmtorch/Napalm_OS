using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;

namespace NapalmOS.Hardware
{
    public class KeyboardStream
    {
        // properties
        public string Output;
        public bool AcceptNewLine = false;
        private bool upperCase;

        public void Read()
        {
            // check letter casing
            if (KeyboardPS2.CapsLockPressed)
            {
                if (KeyboardPS2.ShiftPressed) { upperCase = false; }
                else { upperCase = true; }
            }
            else
            {
                if (KeyboardPS2.ShiftPressed) { upperCase = true; }
                else { upperCase = false; }
            }

            // letters
            if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.A)) { if (upperCase) { Output += "A"; } else { Output += "a"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.B)) { if (upperCase) { Output += "B"; } else { Output += "b"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.C)) { if (upperCase) { Output += "C"; } else { Output += "c"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.D)) { if (upperCase) { Output += "D"; } else { Output += "d"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.E)) { if (upperCase) { Output += "E"; } else { Output += "e"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.F)) { if (upperCase) { Output += "F"; } else { Output += "f"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.G)) { if (upperCase) { Output += "G"; } else { Output += "g"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.H)) { if (upperCase) { Output += "H"; } else { Output += "h"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.I)) { if (upperCase) { Output += "I"; } else { Output += "i"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.J)) { if (upperCase) { Output += "J"; } else { Output += "j"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.K)) { if (upperCase) { Output += "K"; } else { Output += "k"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.L)) { if (upperCase) { Output += "L"; } else { Output += "l"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.M)) { if (upperCase) { Output += "M"; } else { Output += "m"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.N)) { if (upperCase) { Output += "N"; } else { Output += "n"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.O)) { if (upperCase) { Output += "O"; } else { Output += "o"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.P)) { if (upperCase) { Output += "P"; } else { Output += "p"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.Q)) { if (upperCase) { Output += "Q"; } else { Output += "q"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.R)) { if (upperCase) { Output += "R"; } else { Output += "r"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.S)) { if (upperCase) { Output += "S"; } else { Output += "s"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.T)) { if (upperCase) { Output += "T"; } else { Output += "t"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.U)) { if (upperCase) { Output += "U"; } else { Output += "u"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.V)) { if (upperCase) { Output += "V"; } else { Output += "v"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.W)) { if (upperCase) { Output += "W"; } else { Output += "w"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.X)) { if (upperCase) { Output += "X"; } else { Output += "x"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.Y)) { if (upperCase) { Output += "Y"; } else { Output += "y"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.Z)) { if (upperCase) { Output += "Z"; } else { Output += "z"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.D1)) { if (upperCase) { Output += "!"; } else { Output += "1"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.D2)) { if (upperCase) { Output += "@"; } else { Output += "2"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.D3)) { if (upperCase) { Output += "#"; } else { Output += "3"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.D4)) { if (upperCase) { Output += "$"; } else { Output += "4"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.D5)) { if (upperCase) { Output += "%"; } else { Output += "5"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.D6)) { if (upperCase) { Output += "^"; } else { Output += "6"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.D7)) { if (upperCase) { Output += "&"; } else { Output += "7"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.D8)) { if (upperCase) { Output += "*"; } else { Output += "8"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.D9)) { if (upperCase) { Output += "("; } else { Output += "9"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.D0)) { if (upperCase) { Output += ")"; } else { Output += "0"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.Minus)) { if (upperCase) { Output += "_"; } else { Output += "-"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.Equal)) { if (upperCase) { Output += "+"; } else { Output += "="; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.LBracket)) { if (upperCase) { Output += "{"; } else { Output += "["; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.RBracket)) { if (upperCase) { Output += "}"; } else { Output += "]"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.Backslash)) { if (upperCase) { Output += "|"; } else { Output += "\\"; } }
            else if (KeyboardPS2.IsKeyDown(':')) { Output += ":"; }
            else if (KeyboardPS2.IsKeyDown(';')) { Output += ";"; }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.Apostrophe)) { if (upperCase) { Output += "\""; } else { Output += "'"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.Comma)) { if (upperCase) { Output += "<"; } else { Output += ","; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.Period)) { if (upperCase) { Output += ">"; } else { Output += "."; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.Slash)) { if (upperCase) { Output += "?"; } else { Output += "/"; } }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.Backquote)) { if (upperCase) { Output += "~"; } else { Output += "`"; } }


            // functions
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.Spacebar)) { Output += " "; }
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.Enter) && AcceptNewLine) { Output += "\n"; }

            // backspace
            else if (KeyboardPS2.IsKeyDown(Sys.ConsoleKeyEx.Backspace))
            {
                if (Output.Length > 1) { Output = Output.Remove(Output.Length - 1, 1); }
                else if (Output.Length == 1) { Output = string.Empty; }
            }
        }
    }
}
