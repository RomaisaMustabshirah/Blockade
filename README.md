# BLOCKADE 🧱

A falling-block puzzle game built in Unity 6 that mixes **Tetris-style stacking** with **colour matching**.
Line up 4 blocks of the same colour to clear them, shuffle colours mid-fall, and set off chain reactions.

▶ **Play in your browser:** https://romaisamustabshirah.github.io/Blockade/

## How to Play
- Pieces fall from the top. Each block in a piece has its own colour.
- Line up **4 or more blocks of the same colour**, horizontally or vertically, to clear them.
- **Click a falling piece** to shuffle its colours.
- After a clear, blocks above fall down and can trigger **chain reactions**.
- The game ends when the board fills up. Hit **Try Again** to restart.

## Scoring
| Blocks cleared | Points |
|---|---|
| 4 | 10 |
| 5 or more | 20 |

## Controls
| Action | Keyboard | Touch |
|---|---|---|
| Move | ← → / A D | Swipe left / right |
| Rotate | Rotate key | Tap |
| Soft drop | ↓ / S | — |
| Hard drop | Hard-drop key | Drag down |
| Shuffle colours | Click piece | Tap piece |

## Features
- Colour-matching mechanic (match 4+ to clear)
- Cascading gravity with chain-reaction clears
- Pop-and-shatter block clear effect
- Smooth grid-aligned movement with soft drop and hard drop
- Mobile touch controls with safe-area support
- Animated start menu, music toggle and sound effects
- Score and saved high score

## Built With
- Unity 6 (6000.5)
- C#
- WebGL + GitHub Pages

## Project Structure
| Script | Purpose |
|---|---|
| `Grids/GridManager.cs` | Board grid, match detection, gravity, chain reactions, clear effects |
| `Blocks/PieceSpawner.cs` | Spawns pieces with random shapes and colours |
| `Blocks/PlayerController.cs` | Movement, rotation, soft/hard drop, locking |
| `Blocks/PieceColorShuffle.cs` | Click-to-shuffle piece colours |
| `TouchControls.cs` | Swipe, tap and drag gestures |
| `Core/ScoreManager.cs` | Score and high score |
| `Managers/AudioManager.cs` | Music and sound effects |
| `StartMenu.cs` | Animated title screen |

## Run It Locally
1. Clone the repo:
   ```bash
   git clone https://github.com/RomaisaMustabshirah/Blockade.git
   ```
2. Open the folder in Unity Hub (**Add → Add project from disk**).
3. Open the `Main` scene and press **Play**.

## Author
Designed and developed by **Romaisa Mustabshirah**.
