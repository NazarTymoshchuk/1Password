using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1Password
{
    public class XORCipher
    {
        private string GetRepeatKey(string s, int n)
        {
            var r = s;
            while (r.Length < n)
            {
                r += r;
            }
            return r.Substring(0, n);
        }
        private string Cipher(string text, string secretKey)
        {
            var currentKey = GetRepeatKey(secretKey, text.Length);
            var res = string.Empty;
            for (var i = 0; i < text.Length; i++)
            {
                res += ((char)(text[i] ^ currentKey[i])).ToString();
            }
            return res;
        }
        public string Encrypt(string text, string key) => Cipher(text, key);
        public string Decrypt(string text, string key) => Cipher(text, key);
        public string GetRandomKey(int len)
        {
            var gamma = string.Empty;
            var rnd = new Random();
            for (var i = 0; i < len; i++)
            {
                gamma += ((char)rnd.Next(35, 126)).ToString();
            }
            return gamma;
        }
    }
}
