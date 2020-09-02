using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VerifyAuthenticodeSignature
{
    class Program
    {
        static void Main(string[] args)
        {
            string validSignPath = @"C:\";
            //bool isTrusted = AuthenticodeTools.IsTrusted();

            string invalidSignPath = @"C:\temp\AppSecInc.Update.AsapUpdater.Invalid.Signature.exe";
            invalidSignPath = @"c:\Installers\Luminar3_Setup.exe";
            invalidSignPath = @"C:\temp\Setup KB 5.54_IT_Signed.exe";
            bool isTrusted = AuthenticodeTools.IsTrusted(invalidSignPath);

            //var updaterAssembly = Assembly.LoadFile(invalidSignPath);
            //Console.WriteLine("Invalid signature: ");
            //foreach (var evidence in updaterAssembly.Evidence)
            //{
            //    Console.WriteLine(evidence);
            //}
            //foreach (var perm in updaterAssembly.PermissionSet)
            //{
            //    Console.WriteLine(perm);
            //}

            var assembly = Assembly.LoadFile(validSignPath);
            foreach (var evidence in assembly.Evidence)
            {
                if (evidence is System.Security.Policy.Publisher)
                {
                    Console.WriteLine("has valid Authenticode signature...");
                    break;
                }
            }
            //foreach (var perm in updaterAssembly.PermissionSet)
            //{
            //    Console.WriteLine(perm);
            //}

            //var manifestSignatureInformationCollection = ManifestSignatureInformation.VerifySignature(AppDomain.CurrentDomain.ActivationContext);
        }
    }
}
