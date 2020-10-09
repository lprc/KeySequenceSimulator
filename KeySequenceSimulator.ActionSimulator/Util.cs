using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace KeySequenceSimulator.ActionSimulator
{
    static class Util
    {
        // tries to get KeyboardKey from char. Defaults to KeyboardKey.None
        public static KeyboardKey CharToKeyboardKey(char key)
        {
            switch (key)
            {
                // special signs
                case '\n':
                case '\r':
                    return KeyboardKey.Return;
                case ',':
                    return KeyboardKey.OemComma;
                case '\t':
                    return KeyboardKey.Tab;
                case ' ':
                    return KeyboardKey.Space;
                case '\b':
                    return KeyboardKey.Back;

                // alphabet
                case 'A':
                case 'a':
                    return KeyboardKey.A;
                case 'B':
                case 'b':
                    return KeyboardKey.B;
                case 'C':
                case 'c':
                    return KeyboardKey.C;
                case 'D':
                case 'd':
                    return KeyboardKey.D;
                case 'E':
                case 'e':
                    return KeyboardKey.E;
                case 'F':
                case 'f':
                    return KeyboardKey.F;
                case 'G':
                case 'g':
                    return KeyboardKey.G;
                case 'H':
                case 'h':
                    return KeyboardKey.H;
                case 'I':
                case 'i':
                    return KeyboardKey.I;
                case 'J':
                case 'j':
                    return KeyboardKey.J;
                case 'K':
                case 'k':
                    return KeyboardKey.K;
                case 'L':
                case 'l':
                    return KeyboardKey.L;
                case 'M':
                case 'm':
                    return KeyboardKey.M;
                case 'N':
                case 'n':
                    return KeyboardKey.N;
                case 'O':
                case 'o':
                    return KeyboardKey.O;
                case 'P':
                case 'p':
                    return KeyboardKey.P;
                case 'Q':
                case 'q':
                    return KeyboardKey.Q;
                case 'R':
                case 'r':
                    return KeyboardKey.R;
                case 'S':
                case 's':
                    return KeyboardKey.S;
                case 'T':
                case 't':
                    return KeyboardKey.T;
                case 'U':
                case 'u':
                    return KeyboardKey.U;
                case 'V':
                case 'v':
                    return KeyboardKey.V;
                case 'W':
                case 'w':
                    return KeyboardKey.W;
                case 'X':
                case 'x':
                    return KeyboardKey.X;
                case 'Y':
                case 'y':
                    return KeyboardKey.Y;
                case 'Z':
                case 'z':
                    return KeyboardKey.Z;

                // Number Pad
                case '0': return KeyboardKey.D0;
                case '1': return KeyboardKey.D1;
                case '2': return KeyboardKey.D2;
                case '3': return KeyboardKey.D3;
                case '4': return KeyboardKey.D4;
                case '5': return KeyboardKey.D5;
                case '6': return KeyboardKey.D6;
                case '7': return KeyboardKey.D7;
                case '8': return KeyboardKey.D8;
                case '9': return KeyboardKey.D9;
                case '-': return KeyboardKey.OemMinus;
                case '+': return KeyboardKey.OemPlus;
                case '.': return KeyboardKey.OemPeriod;
                case '/': return KeyboardKey.Divide;
                case '*': return KeyboardKey.Multiply;

                default: return KeyboardKey.None;
            }
        }

        // tries to get special KeyboardKey from string, like F1-F12 keys
        public static KeyboardKey StringToKeyboardKey(string key)
        {
            if (key.Length == 1)
                return CharToKeyboardKey(key[0]);

            switch (key)
            {
                case "F1":
                    return KeyboardKey.F1;
                case "F2":
                    return KeyboardKey.F2;
                case "F3":
                    return KeyboardKey.F3;
                case "F4":
                    return KeyboardKey.F4;
                case "F5":
                    return KeyboardKey.F5;
                case "F6":
                    return KeyboardKey.F6;
                case "F7":
                    return KeyboardKey.F7;
                case "F8":
                    return KeyboardKey.F8;
                case "F9":
                    return KeyboardKey.F9;
                case "F10":
                    return KeyboardKey.F10;
                case "F11":
                    return KeyboardKey.F11;
                case "F12":
                    return KeyboardKey.F12;

                default: 
                    return KeyboardKey.None;
            }
        }
    }
}
