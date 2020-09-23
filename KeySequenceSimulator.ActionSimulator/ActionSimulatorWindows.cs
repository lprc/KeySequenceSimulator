using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            inputSimulator.Mouse.MoveMouseTo(NormalizePixelX(x), NormalizePixelY(y));

            if (key == MouseKey.LEFT)
                inputSimulator.Mouse.LeftButtonClick();
            else if (key == MouseKey.RIGHT)
                inputSimulator.Mouse.RightButtonClick();
            else if (key == MouseKey.MIDDLE)
                inputSimulator.Mouse.MiddleButtonClick();
        }

        public void SimulateMouseDoubleClick(MouseKey key, int x, int y)
        {
            inputSimulator.Mouse.MoveMouseTo(NormalizePixelX(x), NormalizePixelY(y));

            if (key == MouseKey.LEFT)
                inputSimulator.Mouse.LeftButtonDoubleClick();
            else if (key == MouseKey.RIGHT)
                inputSimulator.Mouse.RightButtonDoubleClick();
            else if (key == MouseKey.MIDDLE)
                inputSimulator.Mouse.MiddleButtonDoubleClick();
        }

        public void SimulateMouseDown(MouseKey key, int x, int y)
        {
            inputSimulator.Mouse.MoveMouseTo(NormalizePixelX(x), NormalizePixelY(y));

            if (key == MouseKey.LEFT)
                inputSimulator.Mouse.LeftButtonDown();
            else if (key == MouseKey.RIGHT)
                inputSimulator.Mouse.RightButtonDown();
            else if (key == MouseKey.MIDDLE)
                inputSimulator.Mouse.MiddleButtonDown();
        }

        public void SimulateMouseUp(MouseKey key, int x, int y)
        {
            inputSimulator.Mouse.MoveMouseTo(NormalizePixelX(x), NormalizePixelY(y));

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

        private int NormalizePixelX(int xPixel)
        {
            // get both desktops combined maximum screen resolution
            int screenMaxWidth = GetSystemMetrics((int)SYSTEM_METRICS_ARGS.SM_CXVIRTUALSCREEN) - 1;
            return (int)(xPixel * (65535.0f / screenMaxWidth));
        }

        private int NormalizePixelY(int yPixel)
        {
            // get both desktops combined maximum screen resolution
            int screenMaxHeight = GetSystemMetrics((int)SYSTEM_METRICS_ARGS.SM_CYVIRTUALSCREEN) - 1;
            return (int)(yPixel * (65535.0f / screenMaxHeight));
        }

        [DllImport("User32.dll")] static extern int GetSystemMetrics(int nIndex);

        private enum SYSTEM_METRICS_ARGS
        {
            SM_CXSCREEN = 0, SM_CYSCREEN = 1,
            SM_CXVIRTUALSCREEN = 78, SM_CYVIRTUALSCREEN = 79
        }
    }
}
