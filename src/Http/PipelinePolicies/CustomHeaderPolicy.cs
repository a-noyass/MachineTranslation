// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.AI.Translator.Http.PipelinePolicies
{
    class CustomHeaderPolicy : HttpPipelineSynchronousPolicy
    {
        private readonly string _value;
        private readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomHeaderPolicy"/> class.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="value">The value of the header.</param>
        public CustomHeaderPolicy(string name, string value)
        {
            _value = value;
            _name = name;
        }

        /// <inheritdoc/>
        public override void OnSendingRequest(HttpMessage message)
        {
            message.Request.Headers.SetValue(_name, _value);
        }
    }
}
