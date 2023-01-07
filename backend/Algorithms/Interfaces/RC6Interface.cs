
namespace Algorithms.Interfaces
{
    class RC6Interface : AlgorithmInterface
    {
        private RC6 algorithm;
        public RC6Interface(string key)
        {
            algorithm = new RC6(key);
        }
        public byte[] Encrypt(byte[] input)
        {
            return algorithm.EncryptByteArray(input);
        }
        public byte[] Decrypt(byte[] input)
        {
            return algorithm.DecryptByteArray(input);
        }
    }
}