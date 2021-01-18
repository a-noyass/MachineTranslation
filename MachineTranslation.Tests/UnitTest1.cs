// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure;
using Azure.AI.Translator;
using Azure.AI.Translator.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MachineTranslation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1Async()
        {
            string subscriptionKey = "60c3b611ad4947509e8189419f5ff2a6";
            string location = "westus";
            var translator = new TranslatorClient(location, new AzureKeyCredential(subscriptionKey));

            string sentence = "hello world";
            var response = await translator.TranslateAsync(sentence, LanguageCode.AR);

            Console.WriteLine(response);
        }
    }
}
