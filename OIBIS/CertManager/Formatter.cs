using System;
using System.Security.Principal;

namespace CertificationManager
{
    public class Formatter
    {
        /// <summary>
		/// Returns username based on the Windows Logon Name. 
		/// </summary>
		/// <param name="winLogonName"> Windows logon name can be formatted either as a UPN (<username>@<domain_name>) or a SPN (<domain_name>\<username>) </param>
		/// <returns> username </returns>
		public static string ParseName(string winLogonName)
        {
            string[] parts = new string[] { };

            if (winLogonName.Contains("@"))
            {
                ///UPN format
                parts = winLogonName.Split('@');
                return parts[0];
            }
            else if (winLogonName.Contains("\\"))
            {
                /// SPN format
                parts = winLogonName.Split('\\');
                return parts[1];
            }
            else
            {
                return winLogonName;
            }
        }

        public static void PrintCurrentUser()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nCurrently running as '{ParseName(WindowsIdentity.GetCurrent().Name)}'\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
