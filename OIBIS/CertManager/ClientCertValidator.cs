using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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
