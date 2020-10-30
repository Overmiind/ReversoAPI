# ReversoAPI
Reverso context API for .NET

## Quick start

```c#
var service = new ReversoService();
TranslatedResponse translatedWord = await service.TranslateWord(new TranslateWordRequest(from: Language.En, to: Language.Ru)
{
    Word = "Influence",
});

TranslatedResponse translatedSegment = await service.TranslateSegment(new TranslateSegmentRequest(Language.En, Language.Ru)
{
    Source = "Working directory"
});

TranslateTextResponse translatedText = await service.TranslateSentence(new TranslateTextRequest(Language.En, Language.Ru)
{
    Source = "We’re happy because we laugh"
});
```

## API 

### TranslateWord
Translates single word with or without context

**Params**: <br>
+ Word: word to translate <br>
+ Source: context with your word <br>

```c#
var service = new ReversoService();
TranslatedResponse result = await service.TranslateWord(new TranslateWordRequest(from: Language.En, to: Language.Ru)
{
    Word = "Influence",
});

foreach (var resultSource in result.Sources)
{
    // Gets all variants of translation
    foreach (var translations in resultSource.Translations)
    {
        Console.WriteLine($"Translated word: {translations.Translation}");
        Console.WriteLine("Examples:");
        
        // Gets all examples for this word translation
        foreach (var translationsContext in translations.Contexts)
        {
            Console.WriteLine($"Original example: {translationsContext.Source}");
            Console.WriteLine($"Translated example: {translationsContext.Target}");
        }
    }
}
```

### TranslateSentence
Translates single sentence

**Params:**
+ Source: text to translate

```c#
TranslateTextResponse translatedText = await service.TranslateSentence(new TranslateTextRequest(Language.En, Language.Ru)
{
    Source = "We’re happy because we laugh"
});

Console.WriteLine(translatedText.Translation);
```

### TranslateSegment
Translates simple segment of sentence 

**Params:**
+ Source: text to translate

```c#
var service = new ReversoService();
TranslatedResponse result = await service.TranslateSegment(new TranslateSegmentRequest(from: Language.En, to: Language.Ru)
{
    Source = "Working directory",
})
foreach (var resultSource in result.Sources)
{
    // Gets all variants of translation
    foreach (var translations in resultSource.Translations)
    {
        Console.WriteLine($"Translated word: {translations.Translation}");
        Console.WriteLine("Examples:");

        // Gets all examples for this word translation
        foreach (var translationsContext in translations.Contexts)
        {
            Console.WriteLine($"Original example: {translationsContext.Source}");
            Console.WriteLine($"Translated example: {translationsContext.Target}");
        }
    }
}

```
