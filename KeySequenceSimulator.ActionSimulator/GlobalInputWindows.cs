using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeySequenceSimulator.ActionSimulator
{
    public class GlobalInputWindows : IGlobalInput
    {
        private IKeyboardMouseEvents m_GlobalHook;
        private Dictionary<KeyboardKey, Action> hotkeyActionDict = new Dictionary<KeyboardKey, Action>();

        public GlobalInputWindows()
        {
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyPress += HandleInputs;

            // special hook for F1-F12 keys
            m_GlobalHook.OnCombination(new Dictionary<Combination, Action>() {
                { Combination.TriggeredBy(Keys.F1), () => HandleCombinatedInputs("F1") },
                { Combination.TriggeredBy(Keys.F2), () => HandleCombinatedInputs("F2") },
                { Combination.TriggeredBy(Keys.F3), () => HandleCombinatedInputs("F3") },
                { Combination.TriggeredBy(Keys.F4), () => HandleCombinatedInputs("F4") },
                { Combination.TriggeredBy(Keys.F5), () => HandleCombinatedInputs("F5") },
                { Combination.TriggeredBy(Keys.F6), () => HandleCombinatedInputs("F6") },
                { Combination.TriggeredBy(Keys.F7), () => HandleCombinatedInputs("F7") },
                { Combination.TriggeredBy(Keys.F8), () => HandleCombinatedInputs("F8") },
                { Combination.TriggeredBy(Keys.F9), () => HandleCombinatedInputs("F9") },
                { Combination.TriggeredBy(Keys.F10), () => HandleCombinatedInputs("F10") },
                { Combination.TriggeredBy(Keys.F11), () => HandleCombinatedInputs("F11") },
                { Combination.TriggeredBy(Keys.F12), () => HandleCombinatedInputs("F12") },
            });
        }

        // dispose hook
        public void Dispose()
        {
            m_GlobalHook.Dispose();
        }

        // register callback for a given hotkey
        public void RegisterHook(KeyboardKey hotkey, Action func)
        {
            hotkeyActionDict.Add(hotkey, func);
        }

        public void RemoveHook(KeyboardKey hotkey)
        {
            hotkeyActionDict.Remove(hotkey);
        }

        private void HandleInputs(object sender, KeyPressEventArgs e)
        {
            Action a;
            if (hotkeyActionDict.TryGetValue(Util.CharToKeyboardKey(e.KeyChar), out a))
                a.Invoke();
        }

        private void HandleCombinatedInputs(string key)
        {
            Action a;
            if (hotkeyActionDict.TryGetValue(Util.StringToKeyboardKey(key), out a))
                a.Invoke();
        }

        public List<KeyboardKey> GetRegisteredHotkeys()
        {
            return hotkeyActionDict.Keys.ToList();
        }
    }
}
