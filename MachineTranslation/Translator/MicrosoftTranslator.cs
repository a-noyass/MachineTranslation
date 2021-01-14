// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MachineTranslation.Http;
using MachineTranslation.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MachineTranslation.Translator
{
    public class MicrosoftTranslator : IMicrosoftTranslator
    {
        // client
        private static IHttpHandler _client = new HttpHandler();

        // attributes
        private string _subscriptionKey;
        private string _endpoint = "https://api.cognitive.microsofttranslator.com/";
        private string _location;


        public MicrosoftTranslator(string subscriptionKey, string location)
        {
            _subscriptionKey = subscriptionKey;
            _location = location;
        }

        public async Task<string> TranslateAsync(LanguageCodes fromLanguage, LanguageCodes toLanguage, string sentence)
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
                ["from"] = fromLanguage.ToString(),
                ["to"] = toLanguage.ToString()
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
