using Azure.AI.Translator.Models;
using Azure.Core;
using System.Threading.Tasks;

namespace MachineTranslation.Translator
{
    public interface ITranslatorClient
    {
        Task<string> TranslateAsync(LanguageCodes fromLanguage, LanguageCodes toLanguage, string sentence);
    }
}
