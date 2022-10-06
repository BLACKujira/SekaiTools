using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BilibiliUtilities.Utils.CentralUtils
{
    public class VideoUtil
    {
        
        private static readonly string _tb = "fZodR9XQDSUm21yCkr6zBqiveYah8bt4xsWpHnJE7jL5VG3guMTKNPAwcF";
        private static readonly Dictionary<char, ulong> _tr = new Dictionary<char, ulong>();
        private static readonly List<int> _s = new List<int> { 11, 10, 3, 8, 4, 6 };
        private static readonly List<char> _r = new List<char>{ 'B', 'V', '1', 'n', 'n', '4', 'n', '1', 'n', '7', 'n', 'n' };
        private static readonly ulong _xor = 177451812;
        private const ulong Add = 8728348608;


        public static async Task<ulong> BvToAv(string bv)
        {

            for (ulong i = 0; i < 58; ++i)
            {
                _tr[_tb[(int)i]] = i;
            }
            ulong r = 0;
            for (int i = 0; i < 6; ++i)
            {
                r += _tr[bv[_s[i]]] * (ulong)Math.Pow(58, i);
            }
            return (r - Add) ^ _xor;
        }
        
        public static async Task<string> AvToBv(ulong av)
        {
            for (ulong i = 0; i < 58; ++i)
            {
                _tr[_tb[(int)i]] = i;
            }
            av = (av ^ _xor) + Add;
            for (int i = 0; i < 6; ++i)
            {
                _r[_s[i]] = _tb[(int)(av / (ulong)Math.Pow(58, i) % 58)];
            }
            return string.Join("", _r);
        }
    }
}