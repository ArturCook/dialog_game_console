using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogGameConsole.Util
{
    public static class Ids
    {
        private static Dictionary<Type, int> _idsCount = new ();

        public static int GetId(Type type)
        {
            if (!_idsCount.ContainsKey(type))
                _idsCount[type] = 0;
            _idsCount[type]++;
            return _idsCount[type];
        }

        public static string FormatId(int id, Type type)
        {
            return id.ToString().PadLeft((int)Math.Ceiling(Math.Log10(_idsCount[type])), '0');
        }
    }
}
