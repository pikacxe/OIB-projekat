using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.IO;

namespace CertificationManager
{
    public class DigitalSignature
    {
        //create digital signature
        public static byte[] Create(byte[] data, X509Certificate2 certificate)
        {
            /// Looks for the certificate's private key to sign a message
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PrivateKey;

            if (csp == null)
            {
                Console.WriteLine("Valid certificate was not found.");
            }

            byte[] hash = null;

            SHA1Managed sha256 = new SHA1Managed();
            hash = sha256.ComputeHash(data);
            

            byte[] signature = csp.SignHash(hash, HashAlgorithmName.SHA1.ToString());
            return signature;
        }


        public static bool Verify(byte[] data, byte[] signature, X509Certificate2 certificate)
        {
            /// Looks for the certificate's public key to verify a message
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PublicKey.Key;

            byte[] hash = null;

            SHA1Managed sha256 = new SHA1Managed();
            hash = sha256.ComputeHash(data);

            return csp.VerifyHash(hash, HashAlgorithmName.SHA1.ToString(), signature);
        }
    }
}
