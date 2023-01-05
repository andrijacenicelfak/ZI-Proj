namespace Algorithms.Files
{
    class FileEncription
    {
        private AlgorithmInterface Algorithm { get; set; }

        public FileEncription(AlgorithmInterface algorithm)
        {
            Algorithm = algorithm;
        }

        public void EncryptFile(string inputName, string outputName)
        {
            FileStream input = new FileStream(inputName, FileMode.Open, FileAccess.Read);
            FileStream output = new FileStream(outputName, FileMode.Create, FileAccess.Write);
            byte[] data = new byte[512];
            while (input.Read(data, 0, 512) > 0)
            {
                byte[] enc = Algorithm.Encrypt(data);
                output.Write(enc);
            }
            output.Close();
            input.Close();
        }

        public void DecryptFile(string inputName, string outputName)
        {
            FileStream input = new FileStream(inputName, FileMode.Open, FileAccess.Read);
            FileStream output = new FileStream(outputName, FileMode.Create, FileAccess.Write);
            byte[] data = new byte[512];
            while (input.Read(data, 0, 512) > 0)
            {
                byte[] enc = Algorithm.Decrypt(data);
                output.Write(enc);
            }
            output.Close();
            input.Close();
        }
    }
}