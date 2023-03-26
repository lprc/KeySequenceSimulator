using System;
using System.Collections.Generic;
using System.Text;

namespace KeySequenceSimulator.ActionSimulator
{
    public interface IGlobalInput
    {
        void RegisterHook(KeyboardKey hotkey, Action func);
        void RemoveHook(KeyboardKey hotkey);
        void Dispose();
        List<KeyboardKey> GetRegisteredHotkeys();
        Point GetCursorPos();
    }

    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
