
namespace Algorithms.Interfaces
{
    interface AlgorithmInterface
    {
        public byte[] Encrypt(byte[] input);
        public byte[] Decrypt(byte[] input);

        public byte[] EncryptParallel(byte[] input);
        public byte[] DecryptParallel(byte[] input);
    }
}