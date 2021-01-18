// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Azure.AI.Translator.Models
{
    public class TranslateResult
    {
        [JsonProperty("detectedLanguage")]
        public DetectedLanguage DetectedLanguage { get; internal set; }

        [JsonProperty("translations")]
        public IReadOnlyList<Translation> Translations { get; internal set; }
    }

    public class Translation
    {
        [JsonProperty("text")]
        public string Text { get; internal set; }

        [JsonProperty("to")]
        public string To { get; internal set; }
    }

    public class DetectedLanguage
    {
        [JsonProperty("language")]
        public string Language { get; internal set; }

        [JsonProperty("score")]
        public double Score { get; internal set; }
    }
}
