using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Stats
{
    public class MD5Hash
    {
        private string salt;

        public MD5Hash(string salt) 
        {
            this.salt = salt;
        }

        public string Hash(string value) 
        {
            byte[] data = Encoding.ASCII.GetBytes(salt + value);
            byte[] hash = MD5.HashData(data);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
