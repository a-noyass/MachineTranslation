// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.AI.Translator.Models;
using System.Threading.Tasks;

namespace MachineTranslation.Translator
{
    public interface ITranslatorClient
    {
        Task<string> TranslateAsync(string sentence, LanguageCodes toLanguage);
        Task<string> TranslateAsync(string sentence, TranslateOptions options);
    }
}
