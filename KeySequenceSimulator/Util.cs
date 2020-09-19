using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySequenceSimulator
{
    static class Util
    {
        // get key char from Avalonia.Input.Key
        public static char KeyToChar(Key key, KeyModifiers keymod)
        {
            if ((keymod & (KeyModifiers.Alt | KeyModifiers.Control | KeyModifiers.Meta)) != 0)
            {
                return '\x00';
            }

            bool capslock = Console.CapsLock;
            bool shift = keymod == KeyModifiers.Shift;
            bool iscap = (capslock && !shift) || (!capslock && shift);

            switch (key)
            {
                case Key.Enter: return '\n';
                case Key.A: return (iscap ? 'A' : 'a');
                case Key.B: return (iscap ? 'B' : 'b');
                case Key.C: return (iscap ? 'C' : 'c');
                case Key.D: return (iscap ? 'D' : 'd');
                case Key.E: return (iscap ? 'E' : 'e');
                case Key.F: return (iscap ? 'F' : 'f');
                case Key.G: return (iscap ? 'G' : 'g');
                case Key.H: return (iscap ? 'H' : 'h');
                case Key.I: return (iscap ? 'I' : 'i');
                case Key.J: return (iscap ? 'J' : 'j');
                case Key.K: return (iscap ? 'K' : 'k');
                case Key.L: return (iscap ? 'L' : 'l');
                case Key.M: return (iscap ? 'M' : 'm');
                case Key.N: return (iscap ? 'N' : 'n');
                case Key.O: return (iscap ? 'O' : 'o');
                case Key.P: return (iscap ? 'P' : 'p');
                case Key.Q: return (iscap ? 'Q' : 'q');
                case Key.R: return (iscap ? 'R' : 'r');
                case Key.S: return (iscap ? 'S' : 's');
                case Key.T: return (iscap ? 'T' : 't');
                case Key.U: return (iscap ? 'U' : 'u');
                case Key.V: return (iscap ? 'V' : 'v');
                case Key.W: return (iscap ? 'W' : 'w');
                case Key.X: return (iscap ? 'X' : 'x');
                case Key.Y: return (iscap ? 'Y' : 'y');
                case Key.Z: return (iscap ? 'Z' : 'z');
                case Key.D0: return '0';
                case Key.D1: return '1';
                case Key.D2: return '2';
                case Key.D3: return '3';
                case Key.D4: return '4';
                case Key.D5: return '5';
                case Key.D6: return '6';
                case Key.D7: return '7';
                case Key.D8: return '8';
                case Key.D9: return '9';
                case Key.OemMinus: return '-';
                case Key.OemComma: return ',';
                case Key.OemPeriod: return '.';
                case Key.Tab: return '\t';
                case Key.Space: return ' ';

                // Number Pad
                case Key.NumPad0: return '0';
                case Key.NumPad1: return '1';
                case Key.NumPad2: return '2';
                case Key.NumPad3: return '3';
                case Key.NumPad4: return '4';
                case Key.NumPad5: return '5';
                case Key.NumPad6: return '6';
                case Key.NumPad7: return '7';
                case Key.NumPad8: return '8';
                case Key.NumPad9: return '9';
                case Key.Subtract: return '-';
                case Key.Add: return '+';
                case Key.Decimal: return '.';
                case Key.Divide: return '/';
                case Key.Multiply: return '*';

                default: return '\x00';
            }
        }
    }
}
