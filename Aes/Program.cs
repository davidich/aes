using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using RealAes = System.Security.Cryptography.Aes;

namespace Aes
{
    internal class Program
    {
        private const string CbcCipherText1 = "28a226d160dad07883d04e008a7897ee2e4b7465d5290d0c0e6c6822236e1daafb94ffe0c5da05d9476be028ad7c1d81";


        


        public static void Main(string[] args)
        {
            byte[] iv = ParseByteArray("4ca00ff4c898d61e1edbf1800618fb28");
            byte[] key = ParseByteArray("140b41b22a29beb4061bda66b6747e14");
            string message = "Basic CBC mode encryption needs padding.";

            
            var properResult = ProperEncoder(iv, key, message);
            var myResult = MyEncoder(iv, key, message); 
            Debug.Assert(myResult == properResult);

            ProperDecryptor();
        }

        private static string MyEncoder(byte[] iv, byte[] key, string message)
        {
            var cbcEncoder = new CbcAes(iv, key);
            var messageBytes = Encoding.ASCII.GetBytes(message);
            byte[] result = cbcEncoder.Encode(messageBytes);
            return string.Join("", result.Select(b => b.ToString("x2")));
        }

        private static string ProperEncoder(byte[] iv, byte[] key, string message)
        {
            using (RealAes aesAlg = RealAes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;          
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Mode = CipherMode.CBC;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(message);
                        }
                        var encrypted = msEncrypt.ToArray();
                        return string.Join("", encrypted.Select(b => b.ToString("x2")));
                    }
                }
            }
        }

      
        private static void ProperDecryptor()
        {
            using (RealAes aes = RealAes.Create())
            {
                aes.Key = ParseByteArray("140b41b22a29beb4061bda66b6747e14");
                aes.IV = ParseByteArray(CbcCipherText1.Substring(0, 32));
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                // Create the streams used for encryption.
                var encBytes = ParseByteArray(CbcCipherText1.Substring(32, CbcCipherText1.Length - 32));
                using (MemoryStream msDecrypt = new MemoryStream(encBytes))
                {
                    using (CryptoStream cryptoStream =
                        new CryptoStream(msDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        var decBytes = new byte[encBytes.Length];
                        cryptoStream.Read(decBytes, 0, decBytes.Length);

                        var decString = string.Join(" ", decBytes.Select(b => b.ToString("X")));
                        Console.WriteLine(decString);

                        using (var tempMemStream = new MemoryStream(decBytes))
                        {
                            using (StreamReader streamReader = new StreamReader(tempMemStream))
                            {
                                Console.WriteLine(streamReader.ReadToEnd());
                            }
                        }
                    }
                }
            }
        }

        private static byte[] ParseByteArray(string value)
        {
            return Enumerable.Range(0, value.Length)
                .Where(i => i % 2 == 0)
                .Select(i => value.Substring(i, 2))
                .Select(i => byte.Parse(i, NumberStyles.HexNumber))
                .ToArray();
        }
    }
}