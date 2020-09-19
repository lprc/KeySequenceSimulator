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
            throw new NotImplementedException();
        }

        public void SimulateMouseDoubleClick(MouseKey key, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void SimulateMouseDown(MouseKey key, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void SimulateMouseUp(MouseKey key, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void SimulateSleep(int duration)
        {
            Thread.Sleep(duration);
        }

        public void SimulateText(string text)
        {
            throw new NotImplementedException();
        }
    }
}
