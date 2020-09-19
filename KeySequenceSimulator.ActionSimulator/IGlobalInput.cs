using System;
using System.Collections.Generic;
using System.Text;

namespace KeySequenceSimulator.ActionSimulator
{
    public interface IGlobalInput
    {
        void RegisterHook(int hotkeyCode, Action func);
        void RemoveHooks(int hotkeyCode);
    }
}
