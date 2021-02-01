﻿namespace Azure.AI.Translator.Models.V1
{
    class TranslatorBatchesHeaders
    {
        private readonly Response _response;
        public TranslatorBatchesHeaders(Response response)
        {
            _response = response;
        }
        public string OperationLocation => _response.Headers.TryGetValue("Operation-Location", out string value) ? value : null;
    }
}
