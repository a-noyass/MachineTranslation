using Newtonsoft.Json;
using System.Collections.Generic;

namespace Azure.AI.Translator.Models
{
    public class TranslatorResponse
    {
        [JsonProperty("detectedLanguage")]
        public DetectedLanguage DetectedLanguage { get; set; }

        [JsonProperty("translations")]
        public IEnumerable<TranslatorResult> Translations { get; set; }
    }

    public class TranslatorResult
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
        public long Score { get; set; }
    }
}
