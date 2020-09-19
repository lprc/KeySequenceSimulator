using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace KeySequenceSimulator.ActionSimulator
{
    static class Util
    {
        public static VirtualKeyCode CharToKeyCode(char key)
        {
            switch (key)
            {
                case '\n':
                case 'A':
                case 'a':
                    return VirtualKeyCode.VK_A;
                case 'B':
                case 'b':
                    return VirtualKeyCode.VK_B;
                case 'C':
                case 'c':
                    return VirtualKeyCode.VK_C;
                case 'D':
                case 'd':
                    return VirtualKeyCode.VK_D;
                case 'E':
                case 'e':
                    return VirtualKeyCode.VK_E;
                case 'F':
                case 'f':
                    return VirtualKeyCode.VK_F;
                case 'G':
                case 'g':
                    return VirtualKeyCode.VK_G;
                case 'H':
                case 'h':
                    return VirtualKeyCode.VK_H;
                case 'I':
                case 'i':
                    return VirtualKeyCode.VK_I;
                case 'J':
                case 'j':
                    return VirtualKeyCode.VK_J;
                case 'K':
                case 'k':
                    return VirtualKeyCode.VK_K;
                case 'L':
                case 'l':
                    return VirtualKeyCode.VK_L;
                case 'M':
                case 'm':
                    return VirtualKeyCode.VK_M;
                case 'N':
                case 'n':
                    return VirtualKeyCode.VK_N;
                case 'O':
                case 'o':
                    return VirtualKeyCode.VK_O;
                case 'P':
                case 'p':
                    return VirtualKeyCode.VK_P;
                case 'Q':
                case 'q':
                    return VirtualKeyCode.VK_Q;
                case 'R':
                case 'r':
                    return VirtualKeyCode.VK_R;
                case 'S':
                case 's':
                    return VirtualKeyCode.VK_S;
                case 'T':
                case 't':
                    return VirtualKeyCode.VK_T;
                case 'U':
                case 'u':
                    return VirtualKeyCode.VK_U;
                case 'V':
                case 'v':
                    return VirtualKeyCode.VK_V;
                case 'W':
                case 'w':
                    return VirtualKeyCode.VK_W;
                case 'X':
                case 'x':
                    return VirtualKeyCode.VK_X;
                case 'Y':
                case 'y':
                    return VirtualKeyCode.VK_Y;
                case 'Z':
                case 'z':
                    return VirtualKeyCode.VK_Z;
                case ',': 
                    return VirtualKeyCode.OEM_COMMA;
                case '\t':
                    return VirtualKeyCode.TAB;
                case ' ':
                    return VirtualKeyCode.SPACE;

                // Number Pad
                case '0': return VirtualKeyCode.VK_0;
                case '1': return VirtualKeyCode.VK_1;
                case '2': return VirtualKeyCode.VK_2;
                case '3': return VirtualKeyCode.VK_3;
                case '4': return VirtualKeyCode.VK_4;
                case '5': return VirtualKeyCode.VK_5;
                case '6': return VirtualKeyCode.VK_6;
                case '7': return VirtualKeyCode.VK_7;
                case '8': return VirtualKeyCode.VK_8;
                case '9': return VirtualKeyCode.VK_9;
                case '-': return VirtualKeyCode.OEM_MINUS;
                case '+': return VirtualKeyCode.OEM_PLUS;
                case '.': return VirtualKeyCode.OEM_PERIOD;
                case '/': return VirtualKeyCode.DIVIDE;
                case '*': return VirtualKeyCode.MULTIPLY;

                default: return VirtualKeyCode.NONAME;
            }
        }
    }
}
