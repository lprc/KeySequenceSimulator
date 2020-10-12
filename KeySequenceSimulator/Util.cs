using Avalonia.Input;
using KeySequenceSimulator.ActionSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySequenceSimulator
{
    static class Util
    {
        public static KeyboardKey AvaloniaKeyToKeyboardKey(Key key)
        {
            // maybe hardcode a switch statement in the future
            var ret = (KeyboardKey)Enum.Parse(typeof(KeyboardKey), key.ToString());

            switch (ret)
            {
                case KeyboardKey.NumPad0:
                    ret = KeyboardKey.D0;
                    break;
                case KeyboardKey.NumPad1:
                    ret = KeyboardKey.D1;
                    break;
                case KeyboardKey.NumPad2:
                    ret = KeyboardKey.D2;
                    break;
                case KeyboardKey.NumPad3:
                    ret = KeyboardKey.D3;
                    break;
                case KeyboardKey.NumPad4:
                    ret = KeyboardKey.D4;
                    break;
                case KeyboardKey.NumPad5:
                    ret = KeyboardKey.D5;
                    break;
                case KeyboardKey.NumPad6:
                    ret = KeyboardKey.D6;
                    break;
                case KeyboardKey.NumPad7:
                    ret = KeyboardKey.D7;
                    break;
                case KeyboardKey.NumPad8:
                    ret = KeyboardKey.D8;
                    break;
                case KeyboardKey.NumPad9:
                    ret = KeyboardKey.D9;
                    break;

            }

            return ret;
        }
    }
}
