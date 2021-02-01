// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure;
using Azure.AI.Translator;
using Azure.AI.Translator.Models;
using System;
using System.Collections.Generic;
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
            var endpoint = new Uri("https://Snyder-Test.cognitiveservices.azure.com");
            var translator = new TranslatorClient(endpoint, new AzureKeyCredential(subscriptionKey));

            var requestBody = new BatchTranslationRequest()
            {
                Inputs = new List<Input>()
                {
                    new Input()
                    {
                        StorageType = "File",

                        Source = new Source()
                        {
                            SourceUrl = "https://nourdocuments.blob.core.windows.net/translator/2005_Book_.txt?sp=r&st=2021-01-31T14:02:18Z&se=2021-01-31T22:02:18Z&spr=https&sv=2019-12-12&sr=b&sig=fWtFkBvSh0srLhRLe83a2Yc1olkFJGl8%2B1sVy00POec%3D" + "sp=r&st=2021-01-31T14:02:18Z&se=2021-01-31T22:02:18Z&spr=https&sv=2019-12-12&sr=b&sig=fWtFkBvSh0srLhRLe83a2Yc1olkFJGl8%2B1sVy00POec%3D",
                            Language = "en",
                            StorageSource = "AzureBlob"
                        },

                        Targets = new List<Target>()
                        {
                            new Target()
                            {
                                Language = "it",
                                TargetUrl = "https://nourdocuments.blob.core.windows.net/translator/2005_Book_it.txt?sp=r&st=2021-01-31T14:04:48Z&se=2021-01-31T22:04:48Z&spr=https&sv=2019-12-12&sr=b&sig=BgC6S2iJmKxCb426PFZ7PaV3B22esnjNvJ5wUszHw3k%3D",
                                //Category = "[category]",
                                StorageSource = "AzureBlob",

                                // Glossaries are only needed if you want to provide your own custom glossaries
                                //Glossaries = new List<Glossary>()
                                //{
                                //    new Glossary()
                                //    {
                                //        Format = "[glossary format]",
                                //        GlossaryUrl = "[glossary url]",

                                //        // Version is optional in case of some extenstions (ex: tsv)
                                //        Version = "[glossary format version]",
                                //        StorageSource = "AzureBlob"
                                //    }
                                //}
                            }
                        }
                    }
                }
            };
            var response = await translator.TranslateBatchesAsync(requestBody);

            Console.WriteLine(response);
        }
    }
}
