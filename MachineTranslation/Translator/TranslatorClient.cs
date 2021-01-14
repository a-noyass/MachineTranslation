// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.AI.Translator.Http;
using Azure.AI.Translator.Models;
using Azure.Core;
using Azure.Core.Pipeline;
using MachineTranslation.Http;
using MachineTranslation.Translator;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Azure.AI.Translator
{
    /// <summary>
    /// The client to use for interacting with the Azure Translator Service.
    /// </summary>
    public class TranslatorClient : ITranslatorClient
    {
        HttpPipeline _httpPipeline;
        // client
        private static readonly IHttpHandler _client = new HttpHandler();

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
            _httpPipeline = CreatePipeline(options, new CustomHeadersPolicy(subscriptionKey, location));
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

        public async Task<string> TranslateAsync(string sentence, LanguageCodes toLanguage)
        {
            return await TranslateAsync(sentence, new TranslateOptions { To = toLanguage });
        }

        public async Task<string> TranslateAsync(string sentence, TranslateOptions options)
        {
            // define url
            var route = "/translate";
            var requestUrl = _endpoint + route;

            // define body
            object[] body = new object[] { new { Text = sentence } };

            // define headers
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _subscriptionKey,
                ["Ocp-Apim-Subscription-Region"] = _location,
            };

            // define parameters
            // string route = "/translate?api-version=3.0&from=en&to=de&to=it"; ------------------>>>>>> TODO (multiple to language!)
            Dictionary<string, string> parameters = CreateParametersDictionary(options);

            // get api response
            var response = await _client.SendJsonPostRequestAsync(requestUrl, body, headers, parameters);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                //var getClosedListResponse = JsonConvert.DeserializeObject<List<GetClosedListResponse>>(responseString);
                return responseString;
            }
            else
            {
                throw new Exception();
                //throw new FetchingExamplesFailedException(response.StatusCode.ToString());
            }
        }

        //private Request CreateTranslateRequest(string sentence, TranslateOptions options)
        //{
        //    Request request = _httpPipeline.CreateRequest();
        //    request.Method = RequestMethod.Post;

        //    request.Headers.Add(HttpHeader.Common.JsonContentType);

        //    request.Content = new RequestContent()

        //}

        private Dictionary<string, string> CreateParametersDictionary(TranslateOptions options)
        {
            var dictionary = new Dictionary<string, string>
            {
                ["api-version"] = _version,
                ["to"] = options.To.ToString()
            };

            if (options.From != null)
            {
                dictionary["from"] = options.From.ToString();
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
