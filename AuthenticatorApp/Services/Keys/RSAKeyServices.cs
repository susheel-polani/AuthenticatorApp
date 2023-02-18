using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticatorApp.Services.Keys
{
    internal class RSAKeyServices
    {
        /*public static string ToXmlString(RSA rsa, bool includePrivateParameters)
        {
            RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                  parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
                  parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
                  parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
                  parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
                  parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
                  parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
                  parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
                  parameters.D != null ? Convert.ToBase64String(parameters.D) : null);
        }*/
        public static List<string> getPubKeyParameters(RSA rsa)
        {
            RSAParameters pubkeyParameters = rsa.ExportParameters(false);
            List<string> pubkey = new List<string>();

            pubkey.Add(pubkeyParameters.Modulus != null ? Convert.ToBase64String(pubkeyParameters.Modulus) : null);
            pubkey.Add(pubkeyParameters.Exponent != null ? Convert.ToBase64String(pubkeyParameters.Exponent) : null);
            return pubkey;
        }
        public static void GenerateKeyInContainer(string containerName)
        {
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };
            var rsa = new RSACryptoServiceProvider(parameters);
            // TODO: return public key
        }

        public static List<string> GetPublicKeyFromContainer(string containerName)
        {
            // TODO : Write a utility function for rsa object.
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };
            var rsa = new RSACryptoServiceProvider(parameters);

            return getPubKeyParameters(rsa);
        }

        public static void DeleteKeyFromContainer(string containerName)
        {
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };
            var rsa = new RSACryptoServiceProvider(parameters)
            {
                PersistKeyInCsp = false
            };

            rsa.Clear();
        }
    }
}
