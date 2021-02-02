// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure;
using Azure.AI.Translator;
using Azure.AI.Translator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MachineTranslation.Tests
{
    public class UnitTest1
    {
        private readonly TranslatorClient _translator;
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
            string subscriptionKey = "7d232f685fc340b2ba463bda62a063a5";
            var endpoint = new Uri("https://Snyder-Test.cognitiveservices.azure.com");
            _translator = new TranslatorClient(endpoint, new AzureKeyCredential(subscriptionKey));
        }
        [Fact]
        public async Task Test1Async()
        {
            BatchSubmissionRequest requestBody = new BatchSubmissionRequest()
            {
                Inputs = new List<BatchRequest>()
                {
                    new BatchRequest()
                    {
                        StorageType = StorageType.Folder,

                        Source = new SourceInput()
                        {
                            SourceUrl = "https://nourdocuments.blob.core.windows.net/translator?sv=2019-12-12&ss=bfqt&srt=sco&sp=rwdlacupx&se=2021-02-02T19:59:59Z&st=2021-02-02T11:59:59Z&spr=https&sig=F9JFFU1Yejrj%2BMXb%2FPzZk88Mg0awWPcSw%2FD4qIFe9Uo%3D",
                            Language = "en",
                            StorageSource = StorageSource.AzureBlob
                        },

                        Targets = new List<TargetInput>()
                        {
                            new TargetInput()
                            {
                                Language = "it",
                                TargetUrl = "https://nourdocuments.blob.core.windows.net/translatortarget?sv=2019-12-12&ss=bfqt&srt=sco&sp=rwdlacupx&se=2021-02-02T19:59:59Z&st=2021-02-02T11:59:59Z&spr=https&sig=F9JFFU1Yejrj%2BMXb%2FPzZk88Mg0awWPcSw%2FD4qIFe9Uo%3D",
                                StorageSource = StorageSource.AzureBlob,
                            }
                        }
                    }
                }
            };

            var operation = await _translator.TranslateBatchesAsync(requestBody);

            await operation.WaitForCompletionAsync();

            output.WriteLine(JsonConvert.SerializeObject(operation.Value, Formatting.Indented));
        }

        [Fact]
        public async Task Test2Async()
        {
            var documents = _translator.GetBatchDocuments("19e0688c-25ac-48e3-89ad-cb6f3b8e7046");
            IAsyncEnumerator<DocumentStatusDetail> docsEnumerator = documents.GetAsyncEnumerator();
            while (await docsEnumerator.MoveNextAsync())
            {
                output.WriteLine(JsonConvert.SerializeObject(docsEnumerator.Current, Formatting.Indented));
            }
        }

        [Fact]
        public async Task Test3Async()
        {
            var requests = _translator.GetBatchRequests();
            IAsyncEnumerator<BatchStatusDetail> requestsEnumerator = requests.GetAsyncEnumerator();
            while (await requestsEnumerator.MoveNextAsync())
            {
                output.WriteLine(JsonConvert.SerializeObject(requestsEnumerator.Current, Formatting.Indented));
            }
        }
    }
}
