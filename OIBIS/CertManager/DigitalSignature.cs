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
        public static byte[] Create(Stream file, X509Certificate2 certificate)
        {
            /// Looks for the certificate's private key to sign a message
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PrivateKey;

            if (csp == null)
            {
                Console.WriteLine("Valid certificate was not found.");
            }

            byte[] hash = null;

            SHA256Managed sha256 = new SHA256Managed();
            hash = sha256.ComputeHash(file);

            byte[] signature = csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA256"));
            return signature;
        }


        public static bool Verify(Stream file, byte[] signature, X509Certificate2 certificate)
        {
            /// Looks for the certificate's public key to verify a message
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PublicKey.Key;

            byte[] hash = null;

            SHA256Managed sha256 = new SHA256Managed();
            hash = sha256.ComputeHash(file);

            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), signature);
        }
    }
}
