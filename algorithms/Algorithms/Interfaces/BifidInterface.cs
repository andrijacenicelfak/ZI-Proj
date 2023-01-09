using System;

namespace Algorithms.Interfaces
{
    class BifidInterface : AlgorithmInterface
    {
        private Bifid algorithm;
        public BifidInterface(string key)
        {
            algorithm = new Bifid(key);
        }
        public byte[] Encrypt(byte[] input)
        {
            string sinput = System.Text.Encoding.Default.GetString(input);
            string output = algorithm.Encrypt(sinput);
            char[] coutput = output.ToCharArray();
            byte[] boutput = new byte[coutput.Length * sizeof(char)];
            Buffer.BlockCopy(coutput, 0, boutput, 0, coutput.Length * sizeof(char));
            return boutput;
        }
        public byte[] Decrypt(byte[] input)
        {
            string sinput = System.Text.Encoding.Default.GetString(input);
            string output = algorithm.Decrypt(sinput);
            char[] coutput = output.ToCharArray();
            byte[] boutput = new byte[coutput.Length * sizeof(char)];
            Buffer.BlockCopy(coutput, 0, boutput, 0, coutput.Length * sizeof(char));
            return boutput;
        }


        public byte[] EncryptParallel(byte[] input)
        {
            string sinput = System.Text.Encoding.Default.GetString(input);
            string output = algorithm.Encrypt(sinput);
            char[] coutput = output.ToCharArray();
            byte[] boutput = new byte[coutput.Length * sizeof(char)];
            Buffer.BlockCopy(coutput, 0, boutput, 0, coutput.Length * sizeof(char));
            return boutput;
        }
        public byte[] DecryptParallel(byte[] input)
        {
            string sinput = System.Text.Encoding.Default.GetString(input);
            string output = algorithm.Decrypt(sinput);
            char[] coutput = output.ToCharArray();
            byte[] boutput = new byte[coutput.Length * sizeof(char)];
            Buffer.BlockCopy(coutput, 0, boutput, 0, coutput.Length * sizeof(char));
            return boutput;
        }
    }
}