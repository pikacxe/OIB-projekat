using CertificationManager;
using Common;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace IntrusionPreventionSystemProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Print username of a user who is running a service
            Formatter.PrintCurrentUser();

            /// srvCertCN.SubjectName should be set to the service's username. .NET WindowsIdentity class provides information about Windows user running the given process
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            string address = "net.tcp://localhost:6002/IIntrusionPreventionSystem";
            ServiceHost host = new ServiceHost(typeof(IntrusionPreventionService));
            host.AddServiceEndpoint(typeof(IIntrusionPreventionSystem), binding, address);
            ServiceSecurityAuditBehavior auditBehavior = new ServiceSecurityAuditBehavior();

            auditBehavior.AuditLogLocation = AuditLogLocation.Application;
            auditBehavior.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            host.Description.Behaviors.Add(auditBehavior);
            

           
            ///Custom validation mode enables creation of a custom validator - CustomCertificateValidator
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();

            ///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            ///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
            host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
            
            try
            {
                host.Open();
               
                CustomConsole.WriteLine("Intrusion prevention service started. Press Esc to exit...", MessageType.Info);
                while (Console.ReadKey(intercept: true).Key != ConsoleKey.Escape) ;
            }
            catch (Exception e)
            {
                CustomConsole.WriteLine(e.Message, MessageType.Error);
            }
            finally
            {
                host.Close();
            }
        }
    }
}
