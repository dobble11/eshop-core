using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Utils
{
    public class Base64Helper
    {
        public static string Encode(string t)
        {
            var bytes = Encoding.UTF8.GetBytes(t);
            return Convert.ToBase64String(bytes);
        }

        public static string Decode(string t)
        {
            var bytes = Convert.FromBase64String(t);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
