﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Azure.AI.Translator.Models
{
    public class TranslateOptions
    {
        public LanguageCode? ToLanguage { get; set; }
        public LanguageCode? FromLanguage { get; set; }
        public TextType? TextType { get; set; }
        public string Category { get; set; }
        public ProfanityAction? ProfanityAction { get; set; }
        public string ProfanityMarker { get; set; }
        public bool? IncludeAlignment { get; set; }
        public bool? IncludeSentenceLength { get; set; }
        public string SuggestedFrom { get; set; }
        public string FromScript { get; set; }
        public string ToScript { get; set; }
        public bool? AllowFallback { get; set; }
    }

    public enum ProfanityAction
    {
        NoAction,
        Marked,
        Deleted
    }
    public enum TextType
    {
        plain,
        html
    }
}
