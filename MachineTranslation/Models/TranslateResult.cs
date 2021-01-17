// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Azure.AI.Translator.Models
{
    public class TranslateResult
    {
        [JsonProperty("detectedLanguage")]
        public DetectedLanguage DetectedLanguage { get; set; }

        [JsonProperty("translations")]
        public IEnumerable<Translation> Translations { get; set; }
    }

    public class Translation
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
    }

    public class DetectedLanguage
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }
    }
}
