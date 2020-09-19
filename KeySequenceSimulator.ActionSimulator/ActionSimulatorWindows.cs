using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySequenceSimulator.ActionSimulator
{
    public class ActionSimulatorWindows : IActionSimulator
    {
        public void SimulateKey(KeyAction keyAction, int key)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void SimulateText(string text)
        {
            throw new NotImplementedException();
        }
    }
}
