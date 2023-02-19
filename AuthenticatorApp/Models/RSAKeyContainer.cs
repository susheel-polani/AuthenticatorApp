﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AuthenticatorApp.Models
{
    internal class RSAKeyContainer
    {
        public static RSA fetchContainer(string containerName)
        {
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };
            var rsa = new RSACryptoServiceProvider(parameters);

            return rsa;
        }
        public static RSA deleteContainer(string containerName)
        {
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };
            var rsa = new RSACryptoServiceProvider(parameters)
            {
                PersistKeyInCsp = false
            };
            return rsa;
        }
    }
}