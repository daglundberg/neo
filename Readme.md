# Neo

### *A light and scalable UI framework for Monogame.*

![alt text](https://github.com/daglundberg/neo/blob/master/images/constraints.png?raw=true)

### Problem

While using Texture2Ds and SpriteFonts with a SpriteBatch to create a simple UI or HUD for your game is easy to implement, it can become tedious to accommodate different screen sizes and DPIs.

There are a dozen UI frameworks out there for Monogame but many are on the clunky side and/or lack proper support for touch devices. Others are highly opinionated and come with a learning curve.

### Solution

- Lets use math and shaders to draw crisp, lightweight, performant and infinitely scalable controls from basic shapes (no more loading textures).
- Lets implement a simple constraint system to help accommodate different screens.
- Lets use [multi-spectrum signed distance field fonts](https://github.com/Chlumsky/msdfgen) instead of SpriteFonts. MSDFs are lightweight and highly scalable.
- Lets maintain 100% cross platform support.
- Lets keep it simple small. Neo does not include a bathroom sink, and that's a good thing.

### Alternatives

[Myra](https://github.com/rds1983/Myra) 

[Empty Keys](https://www.emptykeys.com/ui_library/)

[GeonBit.UI](https://github.com/RonenNess/GeonBit.UI)

[Apos.Gui](https://github.com/Apostolique/Apos.Gui)