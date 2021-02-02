﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.AI.Translator.Http;
using Azure.AI.Translator.Models;
using Azure.Core;
using Azure.Core.Pipeline;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.AI.Translator
{
    /// <summary>
    /// The client to use for interacting with the Azure Translator Service.
    /// </summary>
    public class TranslatorClient
    {

        internal readonly TranslatorRestClient _serviceRestClient;
        internal readonly ClientDiagnostics _clientDiagnostics;

        // attributes
        private const string AuthorizationHeader = "Ocp-Apim-Subscription-Key";
        private readonly string DefaultCognitiveScope = "https://cognitiveservices.azure.com/.default";

        /// <summary>
        /// Protected constructor to allow mocking.
        /// </summary>
        protected TranslatorClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TranslatorClient"/>
        /// </summary>
        /// <param name="endpoint">A <see cref="Uri"/> to the service the client
        /// sends requests to.  Endpoint can be found in the Azure portal.</param>
        /// <param name="credential">A <see cref="TokenCredential"/> used to
        /// authenticate requests to the service, such as DefaultAzureCredential.</param>
        /// <param name="options"><see cref="TranslatorClientOptions"/> that allow
        /// callers to configure how requests are sent to the service.</param>
        public TranslatorClient(Uri endpoint, TokenCredential credential, TranslatorClientOptions options)
        {
            Argument.AssertNotNull(endpoint, nameof(endpoint));
            Argument.AssertNotNull(credential, nameof(credential));
            Argument.AssertNotNull(options, nameof(options));

            _clientDiagnostics = new ClientDiagnostics(options);
            var pipeline = HttpPipelineBuilder.Build(options, new BearerTokenAuthenticationPolicy(credential, DefaultCognitiveScope));
            _serviceRestClient = new TranslatorRestClient(_clientDiagnostics, pipeline, endpoint.AbsoluteUri);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TranslatorClient"/>
        /// </summary>
        /// <param name="endpoint">A <see cref="Uri"/> to the service the client
        /// sends requests to.  Endpoint can be found in the Azure portal.</param>
        /// <param name="credential">A <see cref="TokenCredential"/> used to
        /// authenticate requests to the service, such as DefaultAzureCredential.</param>
        public TranslatorClient(Uri endpoint, TokenCredential credential)
            : this(endpoint, credential, new TranslatorClientOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TranslatorClient"/>
        /// </summary>
        /// <param name="endpoint">A <see cref="Uri"/> to the service the client
        /// sends requests to.  Endpoint can be found in the Azure portal.</param>
        /// <param name="credential">The API key used to access
        /// the service. This will allow you to update the API key
        /// without creating a new client.</param>
        /// <param name="options"><see cref="TranslatorClientOptions"/> that allow
        /// callers to configure how requests are sent to the service.</param>
        public TranslatorClient(Uri endpoint, AzureKeyCredential credential, TranslatorClientOptions options)
        {
            Argument.AssertNotNull(endpoint, nameof(endpoint));
            Argument.AssertNotNull(credential, nameof(credential));
            Argument.AssertNotNull(options, nameof(options));

            _clientDiagnostics = new ClientDiagnostics(options);
            var pipeline = HttpPipelineBuilder.Build(options, new AzureKeyCredentialPolicy(credential, AuthorizationHeader));
            _serviceRestClient = new TranslatorRestClient(_clientDiagnostics, pipeline, endpoint.AbsoluteUri);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TranslatorClient"/>
        /// </summary>
        /// <param name="endpoint">A <see cref="Uri"/> to the service the client
        /// sends requests to.  Endpoint can be found in the Azure portal.</param>
        /// <param name="credential">The API key used to access
        /// the service. This will allow you to update the API key
        /// without creating a new client.</param>
        public TranslatorClient(Uri endpoint, AzureKeyCredential credential)
            : this(endpoint, credential, new TranslatorClientOptions())
        {
        }

        public BatchesOperation TranslateBatches(BatchSubmissionRequest request, CancellationToken cancellationToken = default)
        {
            var job = _serviceRestClient.TranslateBatch(request, cancellationToken);
            return new BatchesOperation(job.Headers.OperationLocation, this);
        }

        public async Task<BatchesOperation> TranslateBatchesAsync(BatchSubmissionRequest request, CancellationToken cancellationToken = default)
        {
            var job = await _serviceRestClient.TranslateBatchAsync(request, cancellationToken).ConfigureAwait(false);
            return new BatchesOperation(job.Headers.OperationLocation, this);
        }

        public AsyncPageable<BatchStatusDetail> GetBatchRequests(CancellationToken cancellationToken = default)
        {
            async Task<Page<BatchStatusDetail>> FirstPageFunc(int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(TranslatorClient)}.{nameof(GetBatchRequests)}");
                scope.Start();

                try
                {
                    var response = await _serviceRestClient.GetBatchRequests(null, null, cancellationToken).ConfigureAwait(false);

                    var result = response.Value;
                    return Page.FromValues(result.Value, result.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }

            async Task<Page<BatchStatusDetail>> NextPageFunc(string nextLink, int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(TranslatorClient)}.{nameof(GetBatchRequests)}");
                scope.Start();

                try
                {
                    int top = default;
                    int skip = default;

                    // Extracting parameters from the URL.
                    // nextLink - https://westus.api.cognitive.microsoft.com/translator/text/batch/v1.0-preview.1/batches?$skip=20&$top=0

                    string @params = nextLink.Split('?').Last();
                    // params = '$skip=20&$top=0'

                    // Extracting Top and Skip parameter values
                    string[] parameters = @params.Split('&');
                    // '$skip=20', '$top=0'

                    foreach (string paramater in parameters)
                    {
                        if (paramater.Contains("top"))
                        {
                            _ = int.TryParse(paramater.Split('=')[1], out top);
                            // 0
                        }
                        if (paramater.Contains("skip"))
                        {
                            _ = int.TryParse(paramater.Split('=')[1], out skip);
                            // 20
                        }
                    }

                    Response<BatchStatusResponse> response = await _serviceRestClient.GetBatchRequests(skip, top, cancellationToken).ConfigureAwait(false);

                    var result = response.Value;
                    return Page.FromValues(result.Value, result.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }

            return PageableHelpers.CreateAsyncEnumerable(FirstPageFunc, NextPageFunc);
        }

        public AsyncPageable<DocumentStatusDetail> GetBatchDocuments(string jobId, CancellationToken cancellationToken = default)
        {
            async Task<Page<DocumentStatusDetail>> FirstPageFunc(int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(TranslatorClient)}.{nameof(GetBatchDocuments)}");
                scope.Start();

                try
                {
                    var response = await _serviceRestClient.GetBatchDocumentsAsync(jobId, null, null, cancellationToken).ConfigureAwait(false);

                    var result = response.Value;
                    return Page.FromValues(result.Value, result.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }

            async Task<Page<DocumentStatusDetail>> NextPageFunc(string nextLink, int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(TranslatorClient)}.{nameof(GetBatchRequests)}");
                scope.Start();

                try
                {
                    int top = default;
                    int skip = default;

                    // Extracting parameters from the URL.
                    // nextLink - https://westus.api.cognitive.microsoft.com/translator/text/batch/v1.0-preview.1/batches/8002878d-2e43-4675-ad20-455fe004641b/documents?$skip=20&$top=0

                    string @params = nextLink.Split('?').Last();
                    // params = '$skip=20&$top=0'

                    // Extracting Top and Skip parameter values
                    string[] parameters = @params.Split('&');
                    // '$skip=20', '$top=0'

                    foreach (string paramater in parameters)
                    {
                        if (paramater.Contains("top"))
                        {
                            _ = int.TryParse(paramater.Split('=')[1], out top);
                            // 0
                        }
                        if (paramater.Contains("skip"))
                        {
                            _ = int.TryParse(paramater.Split('=')[1], out skip);
                            // 20
                        }
                    }

                    Response<DocumentStatusResponse> response = await _serviceRestClient.GetBatchDocumentsAsync(jobId, skip, top, cancellationToken).ConfigureAwait(false);

                    var result = response.Value;
                    return Page.FromValues(result.Value, result.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }

            return PageableHelpers.CreateAsyncEnumerable(FirstPageFunc, NextPageFunc);
        }
    }
}
