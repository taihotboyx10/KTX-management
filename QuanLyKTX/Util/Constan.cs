using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKTX.Util
{
    class Constan
    {
        public static readonly Dictionary<int, string> ROOM_STATUS = new Dictionary<int, string>()
        {
            {0, "Không thể cho thuê"},
            {1, "Có thể cho thuê"},
        };
    }
}
