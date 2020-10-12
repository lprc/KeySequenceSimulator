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

        public void SimulateKey(KeyAction keyAction, KeyboardKey key, KeyMod mod = KeyMod.NONE)
        {
            VirtualKeyCode vk = KeyboardKeyToKeyCode(key);

            if (vk != VirtualKeyCode.NONAME)
                switch (keyAction)
                {
                    case KeyAction.DOWN:
                        inputSimulator.Keyboard.KeyDown(vk);
                        break;
                    case KeyAction.UP:
                        inputSimulator.Keyboard.KeyUp(vk);
                        break;
                    case KeyAction.PRESS:
                        inputSimulator.Keyboard.ModifiedKeyStroke(ParseKeyMods(mod), vk);
                        //inputSimulator.Keyboard.KeyPress(vk);
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
            inputSimulator.Keyboard.TextEntry(text);
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

        private VirtualKeyCode[] ParseKeyMods(KeyMod mod)
        {
            var mods = new List<VirtualKeyCode>();
            if ((mod & KeyMod.SHIFT) != KeyMod.NONE)
                mods.Add(VirtualKeyCode.SHIFT);
            if ((mod & KeyMod.CTRL) != KeyMod.NONE)
                mods.Add(VirtualKeyCode.CONTROL);
            if ((mod & KeyMod.ALT) != KeyMod.NONE)
                mods.Add(VirtualKeyCode.MENU);
            if ((mod & KeyMod.META) != KeyMod.NONE)
                mods.Add(VirtualKeyCode.LWIN);

            return mods.ToArray();
        }
        private VirtualKeyCode KeyboardKeyToKeyCode(KeyboardKey key)
        {
            switch (key)
            {
                // alphabet
                case KeyboardKey.A:
                    return VirtualKeyCode.VK_A;
                case KeyboardKey.B:
                    return VirtualKeyCode.VK_B;
                case KeyboardKey.C:
                    return VirtualKeyCode.VK_C;
                case KeyboardKey.D:
                    return VirtualKeyCode.VK_D;
                case KeyboardKey.E:
                    return VirtualKeyCode.VK_E;
                case KeyboardKey.F:
                    return VirtualKeyCode.VK_F;
                case KeyboardKey.G:
                    return VirtualKeyCode.VK_G;
                case KeyboardKey.H:
                    return VirtualKeyCode.VK_H;
                case KeyboardKey.I:
                    return VirtualKeyCode.VK_I;
                case KeyboardKey.J:
                    return VirtualKeyCode.VK_J;
                case KeyboardKey.K:
                    return VirtualKeyCode.VK_K;
                case KeyboardKey.L:
                    return VirtualKeyCode.VK_L;
                case KeyboardKey.M:
                    return VirtualKeyCode.VK_M;
                case KeyboardKey.N:
                    return VirtualKeyCode.VK_N;
                case KeyboardKey.O:
                    return VirtualKeyCode.VK_O;
                case KeyboardKey.P:
                    return VirtualKeyCode.VK_P;
                case KeyboardKey.Q:
                    return VirtualKeyCode.VK_Q;
                case KeyboardKey.R:
                    return VirtualKeyCode.VK_R;
                case KeyboardKey.S:
                    return VirtualKeyCode.VK_S;
                case KeyboardKey.T:
                    return VirtualKeyCode.VK_T;
                case KeyboardKey.U:
                    return VirtualKeyCode.VK_U;
                case KeyboardKey.V:
                    return VirtualKeyCode.VK_V;
                case KeyboardKey.W:
                    return VirtualKeyCode.VK_W;
                case KeyboardKey.X:
                    return VirtualKeyCode.VK_X;
                case KeyboardKey.Y:
                    return VirtualKeyCode.VK_Y;
                case KeyboardKey.Z:
                    return VirtualKeyCode.VK_Z;

                // special signs
                case KeyboardKey.Return:
                    return VirtualKeyCode.RETURN;
                case KeyboardKey.OemComma:
                    return VirtualKeyCode.OEM_COMMA;
                case KeyboardKey.Tab:
                    return VirtualKeyCode.TAB;
                case KeyboardKey.Space:
                    return VirtualKeyCode.SPACE;
                case KeyboardKey.LeftCtrl:
                case KeyboardKey.RightCtrl:
                    return VirtualKeyCode.CONTROL;
                case KeyboardKey.LeftShift:
                case KeyboardKey.RightShift:
                    return VirtualKeyCode.SHIFT;
                case KeyboardKey.CapsLock:
                    return VirtualKeyCode.CAPITAL;
                case KeyboardKey.Escape:
                    return VirtualKeyCode.ESCAPE;

                // Number Pad
                case KeyboardKey.D0:
                case KeyboardKey.NumPad0:
                    return VirtualKeyCode.VK_0;
                case KeyboardKey.D1:
                case KeyboardKey.NumPad1:
                    return VirtualKeyCode.VK_1;
                case KeyboardKey.D2:
                case KeyboardKey.NumPad2:
                    return VirtualKeyCode.VK_2;
                case KeyboardKey.D3:
                case KeyboardKey.NumPad3:
                    return VirtualKeyCode.VK_3;
                case KeyboardKey.D4:
                case KeyboardKey.NumPad4:
                    return VirtualKeyCode.VK_4;
                case KeyboardKey.D5:
                case KeyboardKey.NumPad5:
                    return VirtualKeyCode.VK_5;
                case KeyboardKey.D6:
                case KeyboardKey.NumPad6:
                    return VirtualKeyCode.VK_6;
                case KeyboardKey.D7:
                case KeyboardKey.NumPad7:
                    return VirtualKeyCode.VK_7;
                case KeyboardKey.D8:
                case KeyboardKey.NumPad8:
                    return VirtualKeyCode.VK_8;
                case KeyboardKey.D9:
                case KeyboardKey.NumPad9:
                    return VirtualKeyCode.VK_9;
                case KeyboardKey.OemMinus:
                    return VirtualKeyCode.OEM_MINUS;
                case KeyboardKey.OemPlus: 
                    return VirtualKeyCode.OEM_PLUS;
                case KeyboardKey.OemPeriod: 
                    return VirtualKeyCode.OEM_PERIOD;
                case KeyboardKey.Divide: 
                    return VirtualKeyCode.DIVIDE;
                case KeyboardKey.Multiply:
                    return VirtualKeyCode.MULTIPLY;

                // F keys
                case KeyboardKey.F1:
                    return VirtualKeyCode.F1;
                case KeyboardKey.F2:
                    return VirtualKeyCode.F2;
                case KeyboardKey.F3:
                    return VirtualKeyCode.F3;
                case KeyboardKey.F4:
                    return VirtualKeyCode.F4;
                case KeyboardKey.F5:
                    return VirtualKeyCode.F5;
                case KeyboardKey.F6:
                    return VirtualKeyCode.F6;
                case KeyboardKey.F7:
                    return VirtualKeyCode.F7;
                case KeyboardKey.F8:
                    return VirtualKeyCode.F8;
                case KeyboardKey.F9:
                    return VirtualKeyCode.F9;
                case KeyboardKey.F10:
                    return VirtualKeyCode.F10;
                case KeyboardKey.F11:
                    return VirtualKeyCode.F11;
                case KeyboardKey.F12:
                    return VirtualKeyCode.F12;

                default: return VirtualKeyCode.NONAME;
            }
        }
    }
}
