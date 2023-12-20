using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class TripleDesAlgorithm
    {
        public static string Encrypt(Intrusion intrusion, string EncryptionKey)
        {
            using (TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider())
            {
                tripleDes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                tripleDes.Mode = CipherMode.ECB; // Electronic Codebook mode
                tripleDes.Padding = PaddingMode.PKCS7;

                using (MemoryStream memoryStream = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                {
                    // Convert the Intrusion object to a JSON string for encryption
                    string jsonString = JsonConvert.SerializeObject(intrusion);
                    streamWriter.Write(jsonString);
                    streamWriter.Close();
                    cryptoStream.Close();
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public static Intrusion Decrypt(string encryptedData, string EncryptionKey)
        {
            using (TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider())
            {
                tripleDes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                tripleDes.Mode = CipherMode.ECB;
                tripleDes.Padding = PaddingMode.PKCS7;

                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(encryptedData)))
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDes.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader streamReader = new StreamReader(cryptoStream))
                {
                    // Read the decrypted JSON string and convert it back to an Intrusion object
                    string jsonString = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<Intrusion>(jsonString);
                }
            }
        }
    }
}
