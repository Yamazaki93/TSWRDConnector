using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW
{
    public static class MemExtensions
    {
        public static UIntPtr GetPtr(this Mem m, string address)
        {
            return (UIntPtr)(m.ReadLong(address));
        }

        public static string GetCodeRepresentation(this Mem m, UIntPtr ptr)
        {
            return "0x" + ptr.ToUInt64().ToString("x16");
        }

        public static string ReadUTF16(this Mem m, string code)
        {
            var currentIndex = 0;
            var totalBytes = new byte[256];
            var bytes = m.ReadBytes(code, 2);
            if (bytes == null)
                return "";
            var value = BitConverter.ToUInt16(bytes, 0);
            while (value != 0 && currentIndex < 254)
            {
                totalBytes[currentIndex] = bytes[0];
                totalBytes[currentIndex + 1] = bytes[1];
                currentIndex += 2;
                var newCode = m.GetCodeRepresentation(m.Get64BitCode(code) + 2);
                bytes = m.ReadBytes(newCode, 2);
                code = newCode;

                value = BitConverter.ToUInt16(bytes, 0);
            }

            return Encoding.Unicode.GetString(totalBytes.Take(currentIndex).ToArray());
        }
    }
}
