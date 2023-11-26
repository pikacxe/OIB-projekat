using System;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;

namespace CertificationManager
{
    public class ClientCertValidator: X509CertificateValidator
    {
        // Check if certificate is self-issued
        public override void Validate(X509Certificate2 certificate)
        {
            if (certificate.Subject.Equals(certificate.Issuer))
            {
                throw new Exception("Certificate is self-issued.");
            }
        }
    }
}
