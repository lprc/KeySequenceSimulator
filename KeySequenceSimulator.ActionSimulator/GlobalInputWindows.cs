using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace KeySequenceSimulator.ActionSimulator
{
    public class GlobalInputWindows : IGlobalInput
    {
        private IKeyboardMouseEvents m_GlobalHook;
        private Dictionary<char, Action> hotkeyActionDict = new Dictionary<char, Action>();

        public GlobalInputWindows()
        {
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyPress += HandleInputs;
        }

        // dispose hook
        public void Dispose()
        {
            m_GlobalHook.Dispose();
        }

        // register callback for a given hotkey
        public void RegisterHook(char hotkey, Action func)
        {
            hotkeyActionDict.Add(hotkey, func);
        }

        public void RemoveHook(char hotkey)
        {
            hotkeyActionDict.Remove(hotkey);
        }

        private void HandleInputs(object sender, KeyPressEventArgs e)
        {
            Action a;
            if (hotkeyActionDict.TryGetValue(e.KeyChar, out a))
                a.Invoke();
        }
    }
}
