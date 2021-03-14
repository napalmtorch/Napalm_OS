using System;
using System.Collections.Generic;
using System.Text;

namespace Napalm_OS.Graphics
{
    public abstract class Font
    {
        // properties
        public List<uint[]> Characters;
        public uint CharWidth;
        public uint CharHeight;
        public uint CharSpacingX;
        public uint CharSpacingY;

        // constructor
        public Font(uint cw, uint ch)
        {
            Characters = new List<uint[]>();
            this.CharWidth = cw;
            this.CharHeight = ch;
        }

        public int StringWidth(string txt)
        {
            if (txt.Length > 0) { return (int)((CharWidth + CharSpacingX) * txt.Length); }
            else { return 0; }
        }

        // convert character to index in char array
        public static FontCharacter CharToFontChar(char c)
        {
            FontCharacter output = FontCharacter.Space;
            if (c == '!') { output = FontCharacter.Exclamation; }
            else if (c == '\"') { output = FontCharacter.Quotation; }
            else if (c == '#') { output = FontCharacter.NumberSign; }
            else if (c == '$') { output = FontCharacter.DollarSign; }
            else if (c == '%') { output = FontCharacter.Percent; }
            else if (c == '&') { output = FontCharacter.Ampersand; }
            else if (c == '\'') { output = FontCharacter.Apostrophe; }
            else if (c == '(') { output = FontCharacter.LeftBracket; }
            else if (c == ')') { output = FontCharacter.RightBracket; }
            else if (c == '*') { output = FontCharacter.Asterisk; }
            else if (c == '+') { output = FontCharacter.Plus; }
            else if (c == ',') { output = FontCharacter.Comma; }
            else if (c == '-') { output = FontCharacter.Minus; }
            else if (c == '.') { output = FontCharacter.Period; }
            else if (c == '/') { output = FontCharacter.Slash; }
            else if (c == '0') { output = FontCharacter.D0; }
            else if (c == '1') { output = FontCharacter.D1; }
            else if (c == '2') { output = FontCharacter.D2; }
            else if (c == '3') { output = FontCharacter.D3; }
            else if (c == '4') { output = FontCharacter.D4; }
            else if (c == '5') { output = FontCharacter.D5; }
            else if (c == '6') { output = FontCharacter.D6; }
            else if (c == '7') { output = FontCharacter.D7; }
            else if (c == '8') { output = FontCharacter.D8; }
            else if (c == '9') { output = FontCharacter.D9; }
            else if (c == ':') { output = FontCharacter.Colon; }
            else if (c == ';') { output = FontCharacter.SemiColon; }
            else if (c == '<') { output = FontCharacter.LessThan; }
            else if (c == '=') { output = FontCharacter.Equals; }
            else if (c == '>') { output = FontCharacter.GreaterThan; }
            else if (c == '?') { output = FontCharacter.Question; }
            else if (c == '@') { output = FontCharacter.At; }
            else if (c == 'A') { output = FontCharacter.CapitalA; }
            else if (c == 'B') { output = FontCharacter.CapitalB; }
            else if (c == 'C') { output = FontCharacter.CapitalC; }
            else if (c == 'D') { output = FontCharacter.CapitalD; }
            else if (c == 'E') { output = FontCharacter.CapitalE; }
            else if (c == 'F') { output = FontCharacter.CapitalF; }
            else if (c == 'G') { output = FontCharacter.CapitalG; }
            else if (c == 'H') { output = FontCharacter.CapitalH; }
            else if (c == 'I') { output = FontCharacter.CapitalI; }
            else if (c == 'J') { output = FontCharacter.CapitalJ; }
            else if (c == 'K') { output = FontCharacter.CapitalK; }
            else if (c == 'L') { output = FontCharacter.CapitalL; }
            else if (c == 'M') { output = FontCharacter.CapitalM; }
            else if (c == 'N') { output = FontCharacter.CapitalN; }
            else if (c == 'O') { output = FontCharacter.CapitalO; }
            else if (c == 'P') { output = FontCharacter.CapitalP; }
            else if (c == 'Q') { output = FontCharacter.CapitalQ; }
            else if (c == 'R') { output = FontCharacter.CapitalR; }
            else if (c == 'S') { output = FontCharacter.CapitalS; }
            else if (c == 'T') { output = FontCharacter.CapitalT; }
            else if (c == 'U') { output = FontCharacter.CapitalU; }
            else if (c == 'V') { output = FontCharacter.CapitalV; }
            else if (c == 'W') { output = FontCharacter.CapitalW; }
            else if (c == 'X') { output = FontCharacter.CapitalX; }
            else if (c == 'Y') { output = FontCharacter.CapitalY; }
            else if (c == 'Z') { output = FontCharacter.CapitalZ; }
            else if (c == '[') { output = FontCharacter.LeftSquareBracket; }
            else if (c == '\\') { output = FontCharacter.Backslash; }
            else if (c == ']') { output = FontCharacter.RightSquareBracket; }
            else if (c == '^') { output = FontCharacter.Circumflex; }
            else if (c == '_') { output = FontCharacter.Underscore; }
            else if (c == '`') { output = FontCharacter.Backtick; }
            else if (c == 'a') { output = FontCharacter.A; }
            else if (c == 'b') { output = FontCharacter.B; }
            else if (c == 'c') { output = FontCharacter.C; }
            else if (c == 'd') { output = FontCharacter.D; }
            else if (c == 'e') { output = FontCharacter.E; }
            else if (c == 'f') { output = FontCharacter.F; }
            else if (c == 'g') { output = FontCharacter.G; }
            else if (c == 'h') { output = FontCharacter.H; }
            else if (c == 'i') { output = FontCharacter.I; }
            else if (c == 'j') { output = FontCharacter.J; }
            else if (c == 'k') { output = FontCharacter.K; }
            else if (c == 'l') { output = FontCharacter.L; }
            else if (c == 'm') { output = FontCharacter.M; }
            else if (c == 'n') { output = FontCharacter.N; }
            else if (c == 'o') { output = FontCharacter.O; }
            else if (c == 'p') { output = FontCharacter.P; }
            else if (c == 'q') { output = FontCharacter.Q; }
            else if (c == 'r') { output = FontCharacter.R; }
            else if (c == 's') { output = FontCharacter.S; }
            else if (c == 't') { output = FontCharacter.T; }
            else if (c == 'u') { output = FontCharacter.U; }
            else if (c == 'v') { output = FontCharacter.V; }
            else if (c == 'w') { output = FontCharacter.W; }
            else if (c == 'x') { output = FontCharacter.X; }
            else if (c == 'y') { output = FontCharacter.Y; }
            else if (c == 'z') { output = FontCharacter.Z; }
            else if (c == '{') { output = FontCharacter.LeftCurlyBracket; }
            else if (c == '|') { output = FontCharacter.Divider; }
            else if (c == '}') { output = FontCharacter.RightCurlyBracket; }
            else if (c == '~') { output = FontCharacter.Tilde; }
            else if (c == ' ') { output = FontCharacter.Space; }
            return output;
        }
    }

    public enum FontCharacter
    {
        Exclamation,
        Quotation,
        NumberSign,
        DollarSign,
        Percent,
        Ampersand,
        Apostrophe,
        LeftBracket,
        RightBracket,
        Asterisk,
        Plus,
        Comma,
        Minus,
        Period,
        Slash,
        D0,
        D1,
        D2,
        D3,
        D4,
        D5,
        D6,
        D7,
        D8,
        D9,
        Colon,
        SemiColon,
        LessThan,
        Equals,
        GreaterThan,
        Question,
        At,
        CapitalA,
        CapitalB,
        CapitalC,
        CapitalD,
        CapitalE,
        CapitalF,
        CapitalG,
        CapitalH,
        CapitalI,
        CapitalJ,
        CapitalK,
        CapitalL,
        CapitalM,
        CapitalN,
        CapitalO,
        CapitalP,
        CapitalQ,
        CapitalR,
        CapitalS,
        CapitalT,
        CapitalU,
        CapitalV,
        CapitalW,
        CapitalX,
        CapitalY,
        CapitalZ,
        LeftSquareBracket,
        Backslash,
        RightSquareBracket,
        Circumflex,
        Underscore,
        Backtick,
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,
        LeftCurlyBracket,
        Divider,
        RightCurlyBracket,
        Tilde,
        Space,
        ExclamationFlipped,
        Pixel = 116,
    }
}
