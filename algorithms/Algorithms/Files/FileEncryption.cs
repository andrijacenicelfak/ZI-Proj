
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

        public void EncryptBMPFile(string inputName, string outputName)
        {
            FileStream input = new FileStream(inputName, FileMode.Open, FileAccess.Read);
            byte[] header = new byte[54];
            int count = input.Read(header, 0, 54);
            FileStream output = new FileStream(outputName, FileMode.Create, FileAccess.Write);
            output.Write(header, 0, 54);

            byte[] data = new byte[512];
            while (input.Read(data, 0, 512) > 0)
            {
                byte[] enc = Algorithm.Encrypt(data);
                output.Write(enc);
            }

            output.Close();
            input.Close();
        }

        public void DecryptBMPFile(string inputName, string outputName)
        {
            FileStream input = new FileStream(inputName, FileMode.Open, FileAccess.Read);
            byte[] header = new byte[54];
            int count = input.Read(header, 0, 54);
            FileStream output = new FileStream(outputName, FileMode.Create, FileAccess.Write);
            output.Write(header, 0, 54);

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