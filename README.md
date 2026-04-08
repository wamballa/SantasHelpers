# SantasHelpers

Unity project for a modern mobile reinterpretation of a classic Game & Watch-style two-character conveyor game.

## Project Focus

The current direction is:

- landscape-first mobile play
- preserve discrete Game & Watch movement rules
- modernize presentation, HUD, and touch controls for phones

## Current Scene Flow

Active scenes live in [Assets/Scenes/Active](/D:/Documents/Unity/SantasHelpers/Assets/Scenes/Active):

- [Menu.unity](/D:/Documents/Unity/SantasHelpers/Assets/Scenes/Active/Menu.unity)
- [Level01.unity](/D:/Documents/Unity/SantasHelpers/Assets/Scenes/Active/Level01.unity)
- [Level02.unity](/D:/Documents/Unity/SantasHelpers/Assets/Scenes/Active/Level02.unity)
- [GameOver.unity](/D:/Documents/Unity/SantasHelpers/Assets/Scenes/Active/GameOver.unity)

Legacy and experimental scenes have been separated out to reduce confusion.

## Key Structure

- gameplay systems live under `Assets/Scripts`
- active gameplay tuning uses [DefaultGameplayConfig.asset](/D:/Documents/Unity/SantasHelpers/Assets/Resources/DefaultGameplayConfig.asset)
- backlog tracking lives in [Assets/backlog.md](/D:/Documents/Unity/SantasHelpers/Assets/backlog.md)

## Working Notes

- Use [Assets/backlog.md](/D:/Documents/Unity/SantasHelpers/Assets/backlog.md) as the commit-friendly project tracker.
- Keep gameplay world layout stable and adapt mobile support through camera framing, HUD layout, and input.
- Prefer scene wiring and ScriptableObject config over hardcoded runtime lookup where practical.
