using Newtonsoft.Json;
using System;

namespace Azure.AI.Translator.Models.V1
{
    public class BatchTranslationRequest
    {
        [JsonProperty("inputs")]
        public Input[] Inputs { get; set; }
    }

    public partial class Input
    {
        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("targets")]
        public Target[] Targets { get; set; }

        [JsonProperty("storageType")]
        public string StorageType { get; set; }
    }

    public partial class Source
    {
        [JsonProperty("sourceUrl")]
        public Uri SourceUrl { get; set; }

        [JsonProperty("filter")]
        public Filter Filter { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("storageSource")]
        public string StorageSource { get; set; }
    }

    public partial class Filter
    {
        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("suffix")]
        public string Suffix { get; set; }
    }

    public partial class Target
    {
        [JsonProperty("targetUrl")]
        public Uri TargetUrl { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("glossaries")]
        public Glossary[] Glossaries { get; set; }

        [JsonProperty("storageSource")]
        public string StorageSource { get; set; }
    }

    public partial class Glossary
    {
        [JsonProperty("glossaryUrl")]
        public Uri GlossaryUrl { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("storageSource")]
        public string StorageSource { get; set; }
    }
}
