
namespace Algorithms.Interfaces
{
    class RC6CRTInterface : AlgorithmInterface
    {
        private RC6CRT algorithm;
        public RC6CRTInterface(string key, byte[]? iv = null)
        {
            algorithm = new RC6CRT(key, iv);
        }
        public byte[] Encrypt(byte[] input)
        {
            return algorithm.EncryptByteArrayCRT(input);
        }
        public byte[] Decrypt(byte[] input)
        {
            return algorithm.DecryptByteArrayCRT(input);
        }
        public byte[] EncryptParallel(byte[] input)
        {
            return new byte[16];
        }
        public byte[] DecryptParallel(byte[] input)
        {
            return new byte[16];
        }
    }
}