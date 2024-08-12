using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ZZZTOJ.Botris
{
    public static class ZZZTOJCore
    {
        [DllImport("zzz_botris"
            //, EntryPoint = "TetrisAI"
            )]
        public static extern nint BotrisAI(int[] overfield, int[] field, int field_w, int field_h, int b2b, int combo,
            char[] next, char hold, [MarshalAs(UnmanagedType.I1)] bool curCanHold, char active, int x, int y, int spin,
            [MarshalAs(UnmanagedType.I1)] bool canhold, [MarshalAs(UnmanagedType.I1)] bool can180spin, int upcomeAtt, int[] comboTable, int maxDepth, int level);
        [DllImport("zzz_botris"
           //, EntryPoint = "TetrisAI"
           )]
        public static extern nint BotrisAI3(int[] field, int field_w, int field_h, int b2b, int combo,
           char[] next, char hold, [MarshalAs(UnmanagedType.I1)] bool curCanHold, char active, int x, int y, int spin,
           [MarshalAs(UnmanagedType.I1)] bool canhold, [MarshalAs(UnmanagedType.I1)] bool can180spin, int upcomeAtt, int[] comboTable, int maxDepth, int duration);

        [DllImport("zzz_botris"
           //, EntryPoint = "TetrisAI"
           )]
        public static extern nint AIName(int level);
    }
}
