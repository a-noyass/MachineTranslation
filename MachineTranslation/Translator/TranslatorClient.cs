// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.AI.Translator.Http;
using Azure.AI.Translator.Http.PipelinePolicies;
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

        private readonly TranslatorRestClient _restClient;
        internal readonly ClientDiagnostics _clientDiagnostics;

        // attributes
        private const string Endpoint = "https://api.cognitive.microsofttranslator.com/";
        private const string AuthorizationHeader = "Ocp-Apim-Subscription-Key";
        private const string LocationHeader = "Ocp-Apim-Subscription-Region";

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
        public TranslatorClient(string location, AzureKeyCredential subscriptionKey, TranslatorClientOptions options)
        {
            _clientDiagnostics = new ClientDiagnostics(options);
            var pipeline = HttpPipelineBuilder.Build(
                options,
                new AzureKeyCredentialPolicy(subscriptionKey, AuthorizationHeader),
                new CustomHeaderPolicy(LocationHeader, location),
                new ApiVersionPolicy(options.GetVersionString()));
            _restClient = new TranslatorRestClient(_clientDiagnostics, pipeline, Endpoint);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TranslatorClient"/>
        /// </summary>
        /// <param name="subscriptionKey">The key for the Azure Translator resource.</param>
        /// <param name="location">The location of the Azure translator resource.</param>
        public TranslatorClient(string location, AzureKeyCredential subscriptionKey)
            : this(location, subscriptionKey, new TranslatorClientOptions())
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
