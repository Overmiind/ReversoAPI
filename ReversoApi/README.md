# ReversoAPI
Unofficial Reverso Context client for .NET.

## Quick Start

```csharp
using ReversoApi;
using ReversoApi.Models;
using ReversoApi.Models.Requests;

using var client = new ReversoClient();

var result = await client.TranslateAsync(new TranslateRequest
{
    Word = "influence",
    Sentence = "They all wanted to influence the decision.",
    From = Language.En,
    To = Language.Ru
});
```

## Public API

### `TranslateAsync(TranslateRequest)`
This method automatically chooses which Reverso API to call:
- If input is 1 word: uses word translation API.
- If input is a short segment (2-3 words): uses segment translation API.
- If input is more than 3 words: uses text translation API.

Input selection:
- If `Word` is provided, decision is based on `Word`.
- If `Word` is empty, decision is based on `Sentence`.
- In word mode, `Sentence` is used as context.

Response shape depends on translation type:
- Word/segment translations return data in `Sources`.
- Text translations return data in `Translation`.

### `TranslateSegmentAsync(TranslateRequest)`
Experimental feature.

This method uses another Reverso API variant and is usually better for words and short segments.

Requirements:
- `Word` is required
- `Sentence` is required
- `WordPos` defaults to `"0"`

Example:

```csharp
using var client = new ReversoClient();

var segment = await client.TranslateSegmentAsync(new TranslateRequest
{
    Word = "translation",
    Sentence = "Enjoy cutting-edge AI-powered translation from Reverso in 25+ languages",
    WordPos = "30",
    From = Language.En,
    To = Language.Ru
});
```

## Request Model

`TranslateRequest`:
- `Word` (optional)
- `Sentence` (optional)
- `WordPos` (optional, default `"0"`)
- `From` (required)
- `To` (required)

Validation:
- at least one of `Word` or `Sentence` must be provided
- `WordPos` cannot be empty

## Response Model

`TranslateResponse`:
- `Kind` (`Word`, `Segment`, `Sentence`)
- `Input`
- `Translation` (text mode)
- `Sources` (word/segment modes)
- `DirectionFrom`, `DirectionTo`, `IsDirectionChanged` (text mode)
- `Success`, `Error`, `Message`

## Tests

Run all tests:

```bash
dotnet test
```

Current tests include:
- unit tests for request validation and direction formatting
- integration tests for:
  - `TranslateAsync` word mode
  - `TranslateAsync` segment mode
  - `TranslateAsync` sentence/text mode
  - `TranslateSegmentAsync`

## Disclaimer
This code is completely free to use. Its operation is provided as-is and is not guaranteed.

