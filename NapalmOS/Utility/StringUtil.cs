using System;
using System.Collections.Generic;
using System.Text;

namespace NapalmOS
{
    public static class StringUtil
    {
        // convert hex string to integer
        public static uint HexToInt(string hexVal)
        {
            uint len = (uint)hexVal.Length;
            uint base1 = 1;
            uint dec_val = 0;

            for (uint i = len - 1; i >= 0; i--)
            {
                if (hexVal[(int)i] >= '0' &&
                    hexVal[(int)i] <= '9')
                {
                    dec_val += (uint)((hexVal[(int)i] - 48) * base1);
                    base1 = base1 * 16;
                }
                else if (hexVal[(int)i] >= 'A' && hexVal[(int)i] <= 'F')
                {
                    dec_val += (uint)((hexVal[(int)i] - 55) * base1);
                    base1 = base1 * 16;
                }
            }
            return dec_val;
        }

        // convert integer to hex string
        public static string IntToHex(int n)
        {
            string result = "";

            while (n != 0)
            {
                if ((n % 16) < 10) { result = n % 16 + result; }
                else
                {
                    string temp = "";
                    switch (n % 16)
                    {
                        case 10: temp = "A"; break;
                        case 11: temp = "B"; break;
                        case 12: temp = "C"; break;
                        case 13: temp = "D"; break;
                        case 14: temp = "E"; break;
                        case 15: temp = "F"; break;
                    }
                    result = temp + result;
                }
                n /= 16;
            }
            return "0x" + result;
        }
    }
}
