using System;
using System.Collections.Generic;
using System.Text;

namespace KeySequenceSimulator.ActionSimulator
{
    public enum KeyAction
    {
        DOWN, UP, PRESS
    }
    public enum MouseKey
    {
        LEFT, RIGHT, MIDDLE
    }
    public interface IActionSimulator
    {
        void SimulateKey(KeyAction keyAction, int key);
        void SimulateMouseClick(MouseKey key, int x, int y);
        void SimulateMouseDown(MouseKey key, int x, int y);
        void SimulateMouseUp(MouseKey key, int x, int y);
        void SimulateMouseDoubleClick(MouseKey key, int x, int y);
        void SimulateSleep(int duration);
        void SimulateText(string text);
    }
}
