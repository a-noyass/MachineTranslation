// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.AI.Translator.Http
{
    internal class CustomHeadersPolicy : HttpPipelineSynchronousPolicy
    {
        private readonly List<HttpHeader> _headers;

        public CustomHeadersPolicy(string key, string region)
        {
            _headers = new List<HttpHeader>
            {
                new HttpHeader("Ocp-Apim-Subscription-Key", key),
                new HttpHeader("Ocp-Apim-Subscription-Region", region)
            };
        }

        public override void OnSendingRequest(HttpMessage message)
        {
            foreach (var header in _headers)
            {
                message.Request.Headers.Add(header);
            }
        }
    }
}