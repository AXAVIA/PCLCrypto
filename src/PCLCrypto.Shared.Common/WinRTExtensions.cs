﻿//-----------------------------------------------------------------------
// <copyright file="WinRTExtensions.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if !SILVERLIGHT || WINDOWS_PHONE // exclude SL5
namespace PCLCrypto
{
    using System;
    using System.Collections.Generic;
    using System.Text;
#if !PCL
    using PCLCrypto.Formatters;
#endif
    using Validation;

    /// <summary>
    /// Extension methods that add functionality to the WinRT crypto API.
    /// </summary>
    public static class WinRTExtensions
    {
        /// <summary>
        /// Creates a cryptographic key based on the specified RSA parameters.
        /// </summary>
        /// <param name="provider">The asymmetric algorithm provider.</param>
        /// <param name="parameters">The RSA parameters from which to initialize the key.</param>
        /// <returns>The cryptographic key.</returns>
        public static ICryptographicKey ImportParameters(this IAsymmetricKeyAlgorithmProvider provider, RSAParameters parameters)
        {
#if PCL
            throw new NotImplementedException("Not implemented in reference assembly.");
#else
            Requires.NotNull(provider, "provider");

            byte[] keyBlob = KeyFormatter.Pkcs1.Write(parameters);
            return KeyFormatter.HasPrivateKey(parameters)
                ? provider.ImportKeyPair(keyBlob, CryptographicPrivateKeyBlobType.Pkcs1RsaPrivateKey)
                : provider.ImportPublicKey(keyBlob, CryptographicPublicKeyBlobType.Pkcs1RsaPublicKey);
#endif
        }

        /// <summary>
        /// Exports the RSA parameters of a cryptographic key.
        /// </summary>
        /// <param name="key">The cryptographic key.</param>
        /// <param name="includePrivateParameters"><c>true</c> to include the private key in the exported parameters; <c>false</c> to only include the public key.</param>
        /// <returns>The RSA parameters for the key.</returns>
        public static RSAParameters ExportParameters(this ICryptographicKey key, bool includePrivateParameters)
        {
#if PCL
            throw new NotImplementedException("Not implemented in reference assembly.");
#else
            Requires.NotNull(key, "key");

            byte[] keyBlob = includePrivateParameters
                ? key.Export(CryptographicPrivateKeyBlobType.Pkcs1RsaPrivateKey)
                : key.ExportPublicKey(CryptographicPublicKeyBlobType.Pkcs1RsaPublicKey);
            RSAParameters parameters = KeyFormatter.Pkcs1.Read(keyBlob);
            return parameters;
#endif
        }
    }
}

#endif
