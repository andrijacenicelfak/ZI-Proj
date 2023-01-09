using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Algorithms;
using Algorithms.Interfaces;
using Algorithms.DataModels;


public class Program
{
    private HttpClient client;

    private int[]? ServiceKnapsackKey { get; set; }

    private KnapsackCypher Knapsack;

    private Bifid bifid;
    private string KeyBifid { get; set; }
    private string? ServiceBifidKey { get; set; }

    private string? ServiceRC6Key { get; set; }
    private string KeyRC6 { get; set; }

    private RC6 RC6Cipher;


    private string? ServiceRC6CRTKey { get; set; }
    private string KeyRC6CRT { get; set; }

    private RC6CRT RC6CRTCipher;
    public Program()
    {
        client = new()
        {
            BaseAddress = new Uri("http://localhost:5184")
        };
        int[] privateKey = { 1, 3, 5, 10, 20, 41, 81, 162 };
        Knapsack = new KnapsackCypher(101, 337, privateKey);
        KeyBifid = "ovojekljuczabifid";
        bifid = new Bifid(KeyBifid);
        KeyRC6 = "kljuczarc6annanana";
        RC6Cipher = new RC6(KeyRC6);


        KeyRC6CRT = "kljuczarc6annanana";
        RC6CRTCipher = new RC6CRT(KeyRC6CRT);
    }

    public async Task<bool> Command(string txt)
    {
        string[] command = txt.Split(' ');
        switch (command[0])
        {
            case "knapsack":
                await SendKnapsackCodedMessage();
                break;
            case "bifid":
                await SendBifidCodedData();
                break;
            case "rc6":
                await SendRC6CodedData();
                break;
            case "rc6crt":
                await SendRC6CodedData();
                break;
            case "hash":
                await GetHash();
                break;
            case "file":
                try
                {
                    switch (command[1])
                    {
                        case "rc6":
                            EncryptFileRC6(command[2], command[3], command[4]);
                            break;
                        case "rc6crt":
                            EncryptFileRC6CRT(command[2], command[3], command[4]);
                            break;
                        case "knapsack":
                            EncryptFileKnapsack(command[2], command[3], command[4]);
                            break;
                        case "bifid":
                            EncryptFileBifid(command[2], command[3], command[4]);
                            break;
                        default:
                            Console.WriteLine("Nije prepoznat algoritam : " + command[1]);
                            break;
                    }
                    Console.WriteLine("Uspesno!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Nevalidna komanda! " + e.Message);
                }
                break;
            case "bmp":
                try
                {
                    switch (command[1])
                    {
                        case "rc6":
                            EncryptFileRC6BMP(command[2], command[3], command[4]);
                            break;
                        case "rc6crt":
                            EncryptFileRC6CRTBMP(command[2], command[3], command[4]);
                            break;
                        case "knapsack":
                            EncryptFileKnapsackBMP(command[2], command[3], command[4]);
                            break;
                        default:
                            Console.WriteLine("Nije prepoznat algoritam : " + command[1]);
                            break;
                    }
                    Console.WriteLine("Uspesno!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Nevalidna komanda! " + e.Message);
                }
                break;
            case "filep":
                EncryptFileRC6Parallel(command[1], command[2], command[3]);
                break;
            default:
                Console.WriteLine("Komanda : " + command[0] + " nije prepoznata!");
                break;
        }
        return true;
    }

    public void EncryptFileRC6(string inputFile, string encFile = "enc.bin", string decFile = "dec.bin")
    {
        Encryption e = new Encryption(new RC6Interface(KeyRC6));
        e.EncryptFile(inputFile, encFile);
        e.DecryptFile(encFile, decFile);
    }
    public void EncryptFileRC6CRT(string inputFile, string encFile = "enc.bin", string decFile = "dec.bin")
    {
        Encryption e = new Encryption(new RC6CRTInterface(KeyRC6));
        e.EncryptFile(inputFile, encFile);
        e.DecryptFile(encFile, decFile);
    }

    public void EncryptFileKnapsack(string inputFile, string encFile = "enc.bin", string decFile = "dec.bin")
    {
        int[] privateKey = { 1, 3, 5, 10, 20, 41, 81, 162 };
        Encryption e = new Encryption(new KnapsackInterface(101, 337, privateKey));
        e.EncryptFile(inputFile, encFile);
        e.DecryptFile(encFile, decFile);
    }

    public void EncryptFileRC6BMP(string inputFile, string encFile = "enc.bin", string decFile = "dec.bin")
    {
        Encryption e = new Encryption(new RC6Interface(KeyRC6));
        e.EncryptBMPFile(inputFile, encFile);
        e.DecryptBMPFile(encFile, decFile);
    }
    public void EncryptFileRC6CRTBMP(string inputFile, string encFile = "enc.bin", string decFile = "dec.bin")
    {
        Encryption e = new Encryption(new RC6CRTInterface(KeyRC6));
        e.EncryptBMPFile(inputFile, encFile);
        e.DecryptBMPFile(encFile, decFile);
    }

    public void EncryptFileKnapsackBMP(string inputFile, string encFile = "enc.bin", string decFile = "dec.bin")
    {
        int[] privateKey = { 1, 3, 5, 10, 20, 41, 81, 162 };
        Encryption e = new Encryption(new KnapsackInterface(101, 337, privateKey));
        e.EncryptBMPFile(inputFile, encFile);
        e.DecryptBMPFile(encFile, decFile);
    }

    public void EncryptFileBifid(string inputFile, string encFile = "enc.txt", string decFile = "dec.txt")
    {
        Encryption e = new Encryption(new BifidInterface(KeyBifid));
        e.EncryptFile(inputFile, encFile);
        e.DecryptFile(encFile, decFile);
    }

    public void EncryptFileRC6Parallel(string inputFile, string encFile = "enc.bin", string decFile = "dec.bin")
    {
        Encryption e = new Encryption(new RC6Interface(KeyRC6));
        e.EncryptFileParallelNew(inputFile, encFile, 4);
        e.DecryptFileParallelNew(encFile, decFile, 4);
    }

    public async Task<bool> GetKnapsackKey()
    {
        KnapsackKey? kkey = await client.GetFromJsonAsync<KnapsackKey>("KnapsackKey");
        if (kkey != null)
            ServiceKnapsackKey = kkey!.key!;
        else
        {
            Console.WriteLine("Neuspesno preuzimanje kljuca!");
            return false;
        }
        return true;
    }
    public async Task<bool> SendKnapsackCodedMessage()
    {
        if (ServiceKnapsackKey == null)
            if (!(await GetKnapsackKey()))
                return false;

        Console.WriteLine("Poruka za servis : ");
        string? msg = "";
        msg = Console.ReadLine();
        byte[] msgData = new byte[msg!.Length * sizeof(char)];
        Buffer.BlockCopy(msg.ToCharArray(), 0, msgData, 0, msgData.Length);
        KnapsackDataEncrypted kde = new KnapsackDataEncrypted();
        kde.data = KnapsackCypher.EncryptWithKey(msgData, ServiceKnapsackKey!);
        kde.senderPublicKey = Knapsack.publicKey;
        Console.Write("Saljem porku : " + msg + "\nKriptovana : ");
        foreach (int i in kde.data)
            Console.Write(i + " ");
        Console.WriteLine();

        var rsp = await client.PostAsJsonAsync<KnapsackDataEncrypted>("knapsackSendEncrypted", kde);
        KnapsackDataEncrypted? rspData = await rsp.Content.ReadFromJsonAsync<KnapsackDataEncrypted>();
        if (rspData == null)
        {
            Console.WriteLine("Servis nije odgovorio!");
            return true;
        }
        Console.WriteLine("Servise odgovorio sa : ");
        foreach (int i in rspData!.data!)
            Console.Write(i + " ");
        Console.WriteLine();
        byte[] rspDataBytes = Knapsack.Decrypt(rspData.data);
        char[] rspDataChar = new char[rspDataBytes.Length / 2];
        Buffer.BlockCopy(rspDataBytes, 0, rspDataChar, 0, rspDataBytes.Length);

        Console.WriteLine("Dekodirano : " + (new String(rspDataChar)));
        return true;
    }

    public async Task<bool> GetBifidKey()
    {
        BifidKey? kkey = await client.GetFromJsonAsync<BifidKey>("BifidKey");
        if (kkey != null)
        {
            ServiceBifidKey = kkey!.key!;
        }
        else
        {
            Console.WriteLine("Neuspesno preuzimanje kljuca!");
            return false;
        }
        return true;
    }
    public async Task<bool> SendBifidCodedData()
    {
        if (ServiceBifidKey == null)
            if (!(await GetBifidKey()))
                return false;

        Console.WriteLine("Poruka za servis : ");
        string? msg = "";
        msg = Console.ReadLine();

        Bifid serviceBifid = new Bifid(ServiceBifidKey!);

        string msgSend = serviceBifid.Encrypt(msg!);
        Console.WriteLine("Saljem poruku : " + msg + "\nKodirano : " + msgSend);
        BifidData sdata = new BifidData();
        sdata.data = msgSend;
        sdata.senderKey = KeyBifid;

        var rsp = await client.PostAsJsonAsync<BifidData>("bifidSendEncrypted", sdata);
        BifidData? rspData = await rsp.Content.ReadFromJsonAsync<BifidData>();
        if (rspData == null)
        {
            Console.WriteLine("Servis nije odgovorio!");
            return true;
        }
        Console.WriteLine("Servis odgovorio sa : " + rspData.data!);
        Console.WriteLine("Dekriptovano : " + bifid.Decrypt(rspData.data!));

        return true;
    }


    public async Task<bool> GetRC6Key()
    {
        RC6Key? kkey = await client.GetFromJsonAsync<RC6Key>("RC6Key");
        if (kkey != null)
        {
            ServiceRC6Key = kkey!.key!;
        }
        else
        {
            Console.WriteLine("Neuspesno preuzimanje kljuca!");
            return false;
        }
        return true;
    }
    public async Task<bool> SendRC6CodedData()
    {
        if (ServiceRC6Key == null)
            if (!(await GetRC6Key()))
                return false;

        Console.WriteLine("Poruka za servis : ");
        string? msg = "";
        msg = Console.ReadLine();
        RC6 rc6Service = new RC6(ServiceRC6Key!);
        RC6Data kde = new RC6Data();
        kde.data = rc6Service.EncryptStringFaster(msg!);
        kde.senderKey = KeyRC6;
        Console.WriteLine("Saljem porku : " + msg + "\nKriptovana : " + kde.data);

        var rsp = await client.PostAsJsonAsync<RC6Data>("RC6SendEncrypted", kde);
        RC6Data? rspData = await rsp.Content.ReadFromJsonAsync<RC6Data>();
        if (rspData == null)
        {
            Console.WriteLine("Servis nije odgovorio!");
            return true;
        }
        Console.WriteLine("Servise odgovorio sa : " + rspData.data);

        Console.WriteLine("Dekodirano : " + RC6Cipher.DecodeStringFaster(rspData.data!));

        return true;
    }


    public async Task<bool> GetRC6CRTKey()
    {
        RC6CRTKey? kkey = await client.GetFromJsonAsync<RC6CRTKey>("RC6Key");
        if (kkey != null)
        {
            ServiceRC6CRTKey = kkey!.key!;
        }
        else
        {
            Console.WriteLine("Neuspesno preuzimanje kljuca!");
            return false;
        }
        return true;
    }
    public async Task<bool> SendRC6CRTCodedData()
    {
        if (ServiceRC6Key == null)
            if (!(await GetRC6Key()))
                return false;

        Console.WriteLine("Poruka za servis : ");
        string? msg = "";
        msg = Console.ReadLine();
        RC6CRT rc6Service = new RC6CRT(ServiceRC6CRTKey!);
        RC6CRTData kde = new RC6CRTData();

        byte[] sendData = new byte[msg!.Length * sizeof(char)];
        Buffer.BlockCopy(msg.ToCharArray(), 0, sendData, 0, sendData.Length);

        kde.data = rc6Service.EncryptByteArrayCRT(sendData);
        kde.senderKey = KeyRC6CRT;

        char[] kriptData = new char[kde.data.Length / 2];
        Buffer.BlockCopy(kde.data, 0, kriptData, 0, kde.data.Length);

        Console.WriteLine("Saljem porku : " + msg + "\nKriptovana : " + (new String(kriptData)));

        var rsp = await client.PostAsJsonAsync<RC6CRTData>("RC6SendEncrypted", kde);
        RC6CRTData? rspData = await rsp.Content.ReadFromJsonAsync<RC6CRTData>();
        if (rspData == null)
        {
            Console.WriteLine("Servis nije odgovorio!");
            return true;
        }
        char[] odgkript = new char[rspData.data!.Length / 2];
        Buffer.BlockCopy(rspData.data, 0, odgkript, 0, rspData.data.Length);

        Console.WriteLine("Servise odgovorio sa : " + (new String(odgkript)));

        byte[] dekodirano = RC6CRTCipher.DecryptByteArrayCRT(rspData.data);
        char[] dekChar = new char[dekodirano.Length / 2];
        Buffer.BlockCopy(dekodirano, 0, dekChar, 0, dekodirano.Length);
        Console.WriteLine("Dekodirano : " + (new String(dekChar)));

        return true;
    }

    public async Task<bool> GetHash()
    {
        Console.WriteLine("Poruka koja ce da se hesira : ");
        string? msg = "";
        msg = Console.ReadLine();

        TigerHashData tdata = new TigerHashData();
        tdata.data = new byte[msg!.Length / 2];
        Buffer.BlockCopy(msg.ToCharArray(), 0, tdata.data, 0, tdata.data.Length);
        Console.Write("Bajtovi poruke : ");
        foreach (byte b in tdata.data)
            Console.Write(b + " ");
        Console.WriteLine();


        var rsp = await client.PostAsJsonAsync<TigerHashData>("tigerHash", tdata);
        TigerHashData? rspData = await rsp.Content.ReadFromJsonAsync<TigerHashData>();
        if (rspData == null)
        {
            Console.WriteLine("Servis nije odgovorio!");
            return true;
        }
        Console.Write("Hash za tu poruku je : ");
        foreach (byte b in rspData.data!)
            Console.Write(b + " ");
        Console.WriteLine();

        return true;
    }

    public static async Task Main()
    {
        Program p = new Program();
        string? command = "";
        while (command != "end")
        {
            Console.WriteLine("Ukucaj komandu : ");
            command = Console.ReadLine();
            if (command != "" && command != "end")
                await p.Command(command!);
        }
    }
}