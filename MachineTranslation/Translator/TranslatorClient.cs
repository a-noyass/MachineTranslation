// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.AI.Translator.Models;
using MachineTranslation.Http;
using MachineTranslation.Translator;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Azure.AI.Translator
{
    public class TranslatorClient : ITranslatorClient
    {
        // client
        private static IHttpHandler _client = new HttpHandler();

        // attributes
        private readonly string _subscriptionKey;
        private readonly string _endpoint = "https://api.cognitive.microsofttranslator.com/";
        private readonly string _location;
        private readonly ServiceVersion _version;


        public TranslatorClient(string subscriptionKey, string location, TranslatorClientOptions options)
        {
            _subscriptionKey = subscriptionKey;
            _location = location;
            _version = options.Version;
        }

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
            var parameters = new Dictionary<string, string>
            {
                ["api-version"] = "3.0",
                ["from"] = options.From.ToString(),
                ["to"] = options.To.ToString()
            };

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
    }
}
