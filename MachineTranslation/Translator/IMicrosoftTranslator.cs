using MachineTranslation.Models;
using System.Threading.Tasks;

namespace MachineTranslation.Translator
{
    public interface IMicrosoftTranslator
    {
        public Task<string> TranslateAsync(LanguageCodes fromLanguage, LanguageCodes toLanguage, string sentence);
    }
}
