using System.Resources;
using RacketStringManager.Resources;

namespace RacketStringManager.Services;

public class TranslationService : ITranslationService
{
    private readonly ResourceManager _manager;

    public TranslationService() => _manager = new ResourceManager(typeof(AppRes));

    public string GetTranslatedText(string key) => _manager.GetString(key);
}