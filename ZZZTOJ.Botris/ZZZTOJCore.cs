using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ZZZTOJ.Botris
{
    public static class ZZZTOJCore
    {
        [DllImport("zzz_botris.dll"
            //, EntryPoint = "TetrisAI"
            )]
        public static extern nint BotrisAI(int[] overfield, int[] field, int field_w, int field_h, int b2b, int combo,
            char[] next, char hold, bool curCanHold, char active, int x, int y, int spin,
            bool canhold, bool can180spin, int upcomeAtt, int[] comboTable, int maxDepth, int level, int player);
        [DllImport("zzz_botris.dll"
           //, EntryPoint = "TetrisAI"
           )]
        public static extern nint BotrisAI2(int[] field, int field_w, int field_h, int b2b, int combo,
           char[] next, char hold, bool curCanHold, char active, int x, int y, int spin,
           bool canhold, bool can180spin, int upcomeAtt, int[] comboTable, int maxDepth, int level, int player);
    }
}
