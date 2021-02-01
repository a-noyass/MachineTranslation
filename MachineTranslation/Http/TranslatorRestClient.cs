// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.AI.Translator.Models;
using Azure.Core;
using Azure.Core.Pipeline;
using Newtonsoft.Json;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.AI.Translator.Http
{
    internal class TranslatorRestClient
    {
        private readonly string endpoint;
        private readonly HttpPipeline _pipeline;
        private readonly ClientDiagnostics _clientDiagnostics;

        /// <summary> Initializes a new instance of <see cref="TranslatorRestClient"/> </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> Supported Cognitive Services endpoints (protocol and hostname, for example: https://westus.api.cognitive.microsoft.com). </param>
        /// <exception cref="ArgumentNullException"> <paramref name="endpoint"/> is null. </exception>
        public TranslatorRestClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, string endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }
            _clientDiagnostics = clientDiagnostics;
            this.endpoint = endpoint;
            _pipeline = pipeline;
        }

        public ResponseWithHeaders<TranslatorBatchesHeaders> TranslateBatch(BatchTranslationRequest body, CancellationToken cancellationToken)
        {
            using var message = CreateBatchTranslateRequest(body);
            _pipeline.Send(message, cancellationToken);
            var headers = new TranslatorBatchesHeaders(message.Response);
            switch (message.Response.Status)
            {
                case 202:
                    return ResponseWithHeaders.FromValue(headers, message.Response);
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        public async Task<ResponseWithHeaders<TranslatorBatchesHeaders>> TranslateBatchAsync(BatchTranslationRequest body, CancellationToken cancellationToken)
        {
            using var message = CreateBatchTranslateRequest(body);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            var headers = new TranslatorBatchesHeaders(message.Response);
            switch (message.Response.Status)
            {
                case 202:
                    return ResponseWithHeaders.FromValue(headers, message.Response);
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        internal HttpMessage CreateBatchTranslateRequest(BatchTranslationRequest body)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Post;

            var uri = new RawRequestUriBuilder();
            uri.AppendRaw(endpoint, false);
            uri.AppendRaw("/translator/text/batch/v1.0-preview.1", false);
            uri.AppendPath("/batches", false);
            request.Uri = uri;

            request.Headers.Add(HttpHeader.Common.JsonContentType);

            var content = new Utf8JsonRequestContent();
            content.JsonWriter.WriteObjectValue(body);
            request.Content = content;

            return message;
        }

        public async Task<Response<BatchesJobState>> BatchesStatusAsync(string jobId, CancellationToken cancellationToken = default)
        {
            if (jobId == null)
            {
                throw new ArgumentNullException(nameof(jobId));
            }

            using var message = CreateBatchesStatusRequest(jobId);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        BatchesJobState value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = JsonConvert.DeserializeObject<BatchesJobState>(document.RootElement.GetRawText());
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        public Response<BatchesJobState> BatchesStatus(string jobId, CancellationToken cancellationToken = default)
        {
            if (jobId == null)
            {
                throw new ArgumentNullException(nameof(jobId));
            }

            using var message = CreateBatchesStatusRequest(jobId);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        BatchesJobState value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = JsonConvert.DeserializeObject<BatchesJobState>(document.RootElement.GetRawText());
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateBatchesStatusRequest(string jobId)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;

            var uri = new RawRequestUriBuilder();
            uri.AppendRaw(endpoint, false);
            uri.AppendRaw("/translator/text/batch/v1.0-preview.1", false);
            uri.AppendPath("/batches", false);
            uri.AppendPath(jobId);
            request.Uri = uri;

            request.Headers.Add("Accept", "application/json, text/json");
            return message;
        }
    }
}
