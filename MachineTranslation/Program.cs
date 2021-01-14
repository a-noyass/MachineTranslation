using Azure.AI.Translator;
using Azure.AI.Translator.Models;
using System;

namespace MachineTranslation.Translator
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            string subscriptionKey = "60c3b611ad4947509e8189419f5ff2a6";
            string location = "westus";
            var translator = new TranslatorClient(subscriptionKey, location);

            string sentence = "hello world";
            var response = await translator.TranslateAsync(LanguageCodes.EN, LanguageCodes.AR, sentence);

            Console.WriteLine(response);
        }
    }
}
