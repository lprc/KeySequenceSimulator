using System;
using System.Collections.Generic;
using System.Text;

namespace KeySequenceSimulator.ActionSimulator
{
    public interface IGlobalInput
    {
        void RegisterHook(char hotkey, Action func);
        void RemoveHook(char hotkey);
        void Dispose();
    }
}
