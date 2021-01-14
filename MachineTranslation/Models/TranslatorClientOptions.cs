// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core;
using System;

namespace Azure.AI.Translator.Models
{
    public class TranslatorClientOptions : ClientOptions
    {
        /// <summary>
        /// The latest service version supported by this client library.
        /// </summary>
        internal const ServiceVersion LatestVersion = ServiceVersion.V3_0;

        /// <summary>
        /// Gets the <see cref="ServiceVersion"/> of the service API used when making requests
        /// </summary>
        internal ServiceVersion Version { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorClientOptions"/> class.
        /// </summary>
        /// <param name="version">
        /// The <see cref="ServiceVersion"/> of the service API used when making requests.
        /// </param>
        public TranslatorClientOptions(ServiceVersion version = LatestVersion)
        {
            Version = version;
        }

        internal string GetVersionString()
        {
            switch (Version)
            {
                case ServiceVersion.V3_0:
                    return "3.0";

                default:
                    throw new ArgumentException(Version.ToString());
            }
        }
    }

    public enum ServiceVersion
    {
        V3_0 = 1
    }
}
