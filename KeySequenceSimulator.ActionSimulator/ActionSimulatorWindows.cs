using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace KeySequenceSimulator.ActionSimulator
{
    public class ActionSimulatorWindows : IActionSimulator
    {
        private InputSimulator inputSimulator = new InputSimulator();

        public void SimulateKey(KeyAction keyAction, char key)
        {
            VirtualKeyCode vk = Util.CharToKeyCode(key);

            if(vk != VirtualKeyCode.NONAME)
                switch (keyAction)
                {
                    case KeyAction.DOWN:
                        inputSimulator.Keyboard.KeyDown(vk);
                        break;
                    case KeyAction.UP:
                        inputSimulator.Keyboard.KeyUp(vk);
                        break;
                    case KeyAction.PRESS:
                        inputSimulator.Keyboard.KeyPress(vk);
                        break;
                }
            
        }

        public void SimulateMouseClick(MouseKey key, int x, int y)
        {
            inputSimulator.Mouse.MoveMouseTo(x, y);

            if (key == MouseKey.LEFT)
                inputSimulator.Mouse.LeftButtonClick();
            else if (key == MouseKey.RIGHT)
                inputSimulator.Mouse.RightButtonClick();
            else if (key == MouseKey.MIDDLE)
                inputSimulator.Mouse.MiddleButtonClick();
        }

        public void SimulateMouseDoubleClick(MouseKey key, int x, int y)
        {
            inputSimulator.Mouse.MoveMouseTo(x, y);

            if (key == MouseKey.LEFT)
                inputSimulator.Mouse.LeftButtonDoubleClick();
            else if (key == MouseKey.RIGHT)
                inputSimulator.Mouse.RightButtonDoubleClick();
            else if (key == MouseKey.MIDDLE)
                inputSimulator.Mouse.MiddleButtonDoubleClick();
        }

        public void SimulateMouseDown(MouseKey key, int x, int y)
        {
            inputSimulator.Mouse.MoveMouseTo(x, y);

            if (key == MouseKey.LEFT)
                inputSimulator.Mouse.LeftButtonDown();
            else if (key == MouseKey.RIGHT)
                inputSimulator.Mouse.RightButtonDown();
            else if (key == MouseKey.MIDDLE)
                inputSimulator.Mouse.MiddleButtonDown();
        }

        public void SimulateMouseUp(MouseKey key, int x, int y)
        {
            inputSimulator.Mouse.MoveMouseTo(x, y);

            if (key == MouseKey.LEFT)
                inputSimulator.Mouse.LeftButtonUp();
            else if (key == MouseKey.RIGHT)
                inputSimulator.Mouse.RightButtonUp();
            else if (key == MouseKey.MIDDLE)
                inputSimulator.Mouse.MiddleButtonUp();
        }

        public void SimulateSleep(int duration)
        {
            Thread.Sleep(duration);
        }

        public void SimulateText(string text)
        {
            foreach (var c in text)
                SimulateKey(KeyAction.PRESS, c);
        }
    }
}
