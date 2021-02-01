using Newtonsoft.Json;
using System;

namespace Azure.AI.Translator.Models
{
    public class BatchesJobState
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("createdDateTimeUtc")]
        public DateTimeOffset CreatedDateTimeUtc { get; set; }

        [JsonProperty("lastActionDateTimeUtc")]
        public DateTimeOffset LastActionDateTimeUtc { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("summary")]
        public Summary Summary { get; set; }
    }

    public class Summary
    {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("failed")]
        public long Failed { get; set; }

        [JsonProperty("success")]
        public long Success { get; set; }

        [JsonProperty("inProgress")]
        public long InProgress { get; set; }

        [JsonProperty("notYetStarted")]
        public long NotYetStarted { get; set; }

        [JsonProperty("cancelled")]
        public long Cancelled { get; set; }
    }
}
