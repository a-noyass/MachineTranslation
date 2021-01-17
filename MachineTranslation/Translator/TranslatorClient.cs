// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.AI.Translator.Http;
using Azure.AI.Translator.Models;
using Azure.Core;
using Azure.Core.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.AI.Translator
{
    /// <summary>
    /// The client to use for interacting with the Azure Translator Service.
    /// </summary>
    public class TranslatorClient
    {

        TranslatorRestClient _restClient;
        // attributes
        private readonly string _subscriptionKey;
        private readonly string _endpoint = "https://api.cognitive.microsofttranslator.com/";
        private readonly string _location;
        private readonly string _version;

        /// <summary>
        /// Protected constructor to allow mocking.
        /// </summary>
        protected TranslatorClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TranslatorClient"/>
        /// </summary>
        /// <param name="subscriptionKey">The key for the Azure Translator resource.</param>
        /// <param name="location">The location of the Azure translator resource.</param>
        /// <param name="options">Options that allow configuration of requests sent to the Translator Service.</param>
        public TranslatorClient(string subscriptionKey, string location, TranslatorClientOptions options)
        {
            _subscriptionKey = subscriptionKey;
            _location = location;
            _version = options.GetVersionString();
            var pipeline = CreatePipeline(options, new CustomHeadersPolicy(subscriptionKey, location));
            _restClient = new TranslatorRestClient(pipeline, _endpoint);
        }

        private static HttpPipeline CreatePipeline(TranslatorClientOptions options, HttpPipelinePolicy authenticationPolicy)
            => HttpPipelineBuilder.Build(options,
                new HttpPipelinePolicy[] { authenticationPolicy, new ApiVersionPolicy(options.GetVersionString()) },
                new HttpPipelinePolicy[] { },
                new ResponseClassifier());

        /// <summary>
        /// Initializes a new instance of <see cref="TranslatorClient"/>
        /// </summary>
        /// <param name="subscriptionKey">The key for the Azure Translator resource.</param>
        /// <param name="location">The location of the Azure translator resource.</param>
        public TranslatorClient(string subscriptionKey, string location)
            : this(subscriptionKey, location, new TranslatorClientOptions())
        {
        }

        public async Task<Response<TranslateResult[]>> TranslateAsync(string sentence, LanguageCodes toLanguage)
        {
            return await TranslateAsync(sentence, new TranslateOptions { To = toLanguage });
        }

        public async Task<Response<TranslateResult[]>> TranslateAsync(string sentence, TranslateOptions options, CancellationToken cancellationToken = default)
        {
            var result = await _restClient.TranslateAsync(sentence, options, cancellationToken);
            return result;
        }




    }
}
