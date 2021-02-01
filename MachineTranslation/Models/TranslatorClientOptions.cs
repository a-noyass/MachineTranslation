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
        internal const ServiceVersion LatestVersion = ServiceVersion.V1_0_preview_1;

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
            return Version switch
            {
                ServiceVersion.V1_0_preview_1 => "1.0-preview.1",
                _ => throw new ArgumentException(Version.ToString()),
            };
        }
    }

    public enum ServiceVersion
    {
        V1_0_preview_1 = 1
    }
}
