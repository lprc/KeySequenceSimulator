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
            return (KeyboardKey)Enum.Parse(typeof(KeyboardKey), key.ToString());
        }
    }
}
