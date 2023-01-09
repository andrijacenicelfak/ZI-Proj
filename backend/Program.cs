using Algorithms;
using KnapsackCipher = Algorithms.KnapsackCypher;
using Algorithms.DataModels;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

int[] privateKey = { 1, 2, 4, 9, 17, 34, 69, 140 };
int M = 313;
int N = 101;

KnapsackCipher knapsackCipher = new KnapsackCipher(N, M, privateKey);
int[] publicKey = knapsackCipher.publicKey;

string bifidKey = "kljuc za kodiranje bifid";
Bifid bifidCipher = new Bifid(bifidKey);

string rc6Key = "KLJUCzaRC6";
RC6 rc = new RC6(rc6Key);

string rc6crtKey = "KLJUCzaRC6CRT";
RC6CRT rc6crt = new RC6CRT(rc6crtKey);

TigerHash hash = new TigerHash();

app.MapGet("/KnapsackKey", () => new KnapsackKey { key = publicKey });
app.MapGet("/BifidKey", () => new BifidKey { key = bifidKey });
app.MapGet("/RC6Key", () => new RC6Key { key = rc6Key });
app.MapGet("/RC6CRTKey", () => new RC6Key { key = rc6crtKey });

app.MapPost("/tigerHash", (TigerHashData data) =>
{
    TigerHashData rsp = new TigerHashData();
    rsp.data = hash.ComputeHash(data.data!);

    return rsp;
});

app.MapPost("/knapsackSendEncrypted", (KnapsackDataEncrypted kdata) =>
{
    byte[] dec = knapsackCipher.Decrypt(kdata.data!);
    char[] carr = new char[dec.Length / 2];
    Buffer.BlockCopy(dec, 0, carr, 0, dec.Length);
    string s = new String(carr);
    Console.WriteLine("Klijent kaze :" + s);
    KnapsackDataEncrypted kde = new KnapsackDataEncrypted();
    string msg = "Primljeno! Pozdrav";
    byte[] msgByte = new byte[msg.Length * sizeof(char)];
    Buffer.BlockCopy(msg.ToCharArray(), 0, msgByte, 0, msgByte.Length);
    kde.data = KnapsackCypher.EncryptWithKey(msgByte, kdata.senderPublicKey!);
    return kde;

});

app.MapPost("/bifidSendEncrypted", (BifidData bdata) =>
{
    string s = bifidCipher.Decrypt(bdata.data!);
    Console.WriteLine("Klijent kaze :" + s);
    Bifid clientBifid = new Bifid(bdata.senderKey!);
    string msg = clientBifid.Encrypt("Primljeno! Pozdrav!");
    BifidData msgData = new BifidData();
    msgData.data = msg;
    msgData.senderKey = bifidKey;
    return msgData;
});

app.MapPost("/RC6SendEncrypted", (RC6Data data) =>
{
    string s = rc.DecodeStringFaster(data.data!);
    Console.WriteLine("Klijent kaze : " + s);

    RC6 clientRC = new RC6(data.senderKey!);
    RC6Data ndata = new RC6Data();
    ndata.data = clientRC.EncryptStringFaster("Primljeno! Pozdrav!");
    ndata.senderKey = rc6Key;
    return ndata;
});
app.MapPost("/RC6CRTSendEncrypted", (RC6CRTData data) =>
{
    byte[] dec = rc6crt.DecryptByteArrayCRT(data.data!);
    char[] charDec = new char[dec.Length / 2];
    Buffer.BlockCopy(dec, 0, charDec, 0, dec.Length);
    Console.WriteLine("Klijent kaze : " + (new String(charDec)));

    RC6CRT clientRc = new RC6CRT(data.senderKey!);
    string msg = "Primljeno! Pozdrav!";
    byte[] zaKodiranje = new byte[msg.Length / 2];
    Buffer.BlockCopy(msg.ToCharArray(), 0, zaKodiranje, 0, zaKodiranje.Length);

    RC6CRTData ndata = new RC6CRTData();
    ndata.senderKey = rc6crtKey;
    ndata.data = clientRc.EncryptByteArrayCRT(zaKodiranje);

    return ndata;
});
app.Run();
