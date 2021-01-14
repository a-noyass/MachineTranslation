// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core;

namespace Azure.AI.Translator.Models
{
    public class TranslatorClientOptions : ClientOptions
    {
        public ServiceVersion Version { get; set; }

        public TranslatorClientOptions(ServiceVersion version = ServiceVersion.V3_0)
        {
            Version = version;
        }
    }

    public enum ServiceVersion
    {
        V3_0 = 1
    }
}
