// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.AI.Translator.Models;
using Azure.AI.Translator.Models.V1;
using Azure.Core;
using Azure.Core.Pipeline;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.AI.Translator.Http
{
    internal class TranslatorRestClient
    {
        private readonly string _endpoint;
        private readonly HttpPipeline _pipeline;
        private readonly ClientDiagnostics _clientDiagnostics;

        /// <summary> Initializes a new instance of TranslatorRestClient. </summary>
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
            _endpoint = endpoint;
            _pipeline = pipeline;
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
            // initialize request
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Post;

            // construct URI
            var uri = new RequestUriBuilder();
            uri.Reset(new Uri(_endpoint));
            uri.AppendPath("batches");
            request.Uri = uri;

            // add headers
            request.Headers.Add(HttpHeader.Common.JsonContentType);

            var content = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(body);
            request.Content = RequestContent.Create(content);

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
            var uri = new RequestUriBuilder();
            uri.Reset(new Uri(_endpoint));
            uri.AppendPath("batches");
            uri.AppendPath(jobId);

            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, text/json");
            return message;
        }

        internal HttpMessage CreateTranslateRequest(string text, TranslateOptions options)
        {
            // initialize request
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Post;

            // construct URI
            var uri = new RequestUriBuilder();
            uri.Reset(new Uri(_endpoint));
            uri.AppendPath("translate");
            Dictionary<string, string> parameters = CreateParametersDictionary(options);
            foreach (var p in parameters)
            {
                uri.AppendQuery(p.Key, p.Value);
            }
            request.Uri = uri;

            // add headers
            request.Headers.Add(HttpHeader.Common.JsonContentType);

            var content = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new object[] { new { Text = text } });
            request.Content = RequestContent.Create(content);

            return message;
        }

        public async Task<Response<IReadOnlyList<TranslateResult>>> TranslateAsync(string text, TranslateOptions options, CancellationToken cancellationToken)
        {
            using var message = CreateTranslateRequest(text, options);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        IReadOnlyList<TranslateResult> value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = JsonConvert.DeserializeObject<List<TranslateResult>>(document.RootElement.GetRawText());

                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        private Dictionary<string, string> CreateParametersDictionary(TranslateOptions options)
        {
            var dictionary = new Dictionary<string, string>
            {
                ["to"] = options.ToLanguage.ToString()
            };

            if (options.FromLanguage != null)
            {
                dictionary["from"] = options.FromLanguage.ToString();
            }
            if (options.AllowFallback != null)
            {
                dictionary["allowFallback"] = options.AllowFallback.ToString();
            }
            if (options.Category != null)
            {
                dictionary["category"] = options.Category;
            }
            if (options.FromScript != null)
            {
                dictionary["fromScript"] = options.FromScript;
            }
            if (options.IncludeAlignment != null)
            {
                dictionary["includeAlignment"] = options.IncludeAlignment.ToString();
            }
            if (options.IncludeSentenceLength != null)
            {
                dictionary["includeSentenceLength"] = options.IncludeSentenceLength.ToString();
            }
            if (options.ProfanityAction != null)
            {
                dictionary["profanityAction"] = options.ProfanityAction.ToString();
            }
            if (options.ProfanityMarker != null)
            {
                dictionary["profanityMarker"] = options.ProfanityMarker;
            }
            if (options.SuggestedFrom != null)
            {
                dictionary["suggestedFrom"] = options.SuggestedFrom;
            }
            if (options.TextType != null)
            {
                dictionary["textType"] = options.TextType.ToString();
            }
            if (options.ToScript != null)
            {
                dictionary["toScript"] = options.ToScript;
            }

            return dictionary;
        }
    }
}
