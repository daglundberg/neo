# Neo

### *A light and scalable UI framework for Monogame.*

![alt text](https://github.com/daglundberg/neo/blob/master/images/constraints.png?raw=true)

### Problem

While using Texture2Ds and SpriteFonts with a SpriteBatch to create a simple UI or HUD for your game is easy to implement, it quickly becomes a pain to accommodate different screen sizes and DPIs.

### Solution

- Lets use math and shaders to draw crisp, lightweight, performant and infinitely scalable controls from basic shapes (no more loading textures).
- Lets use [multi-spectrum signed distance field fonts](https://github.com/Chlumsky/msdfgen) instead of SpriteFonts. MSDFs are lightweight and highly scalable.
- Lets implement a simple constraint system to help accommodate different screens.
- Lets keep it simple, clear and speedy. Neo does not include a bathroom sink, and that's a good thing (arguably).

### Alternatives

[Myra](https://github.com/rds1983/Myra) 

[Empty Keys](https://www.emptykeys.com/ui_library/)

[GeonBit.UI](https://github.com/RonenNess/GeonBit.UI)