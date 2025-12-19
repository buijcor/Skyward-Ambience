# Skyward Ambience

A zero-impact visual mod for RimWorld 1.6 that adds life to the "upper layer" of the map by adding:

- **Bird Flocks**: Flocks of birds that occasionally pass by overhead, casting shadows as they fly across the map
- **Cloud Shadows**: Drifting cloud shadows that move across the map, adding atmospheric depth

## Features

- **Zero Gameplay Impact**: Purely visual effects that don't affect gameplay mechanics
- **Performance Optimized**: Only draws effects when they're in the camera view
- **Dynamic Spawning**: Effects spawn naturally from map edges with varied timing and behaviors
- **Subtle Ambience**: Adds life and movement to the sky without being distracting
- **Customizable Cloud Shadows**: Adjust cloud noise, color (RGB), and transparency via mod settings

## Installation

1. Place this mod folder in your RimWorld `Mods` directory
2. Enable the mod in the RimWorld mod menu
3. Requires Harmony (dependency automatically handled)

## Configuration

The mod includes customizable settings for cloud shadows. To access the settings:

1. Go to **Options → Mod Settings** (or **Mods → [Select Skyward Ambience] → Settings**)
2. Click on **"Skyward Ambience"** in the mod list
3. Adjust the sliders for:
   - Cloud Noise Scale (controls noise pattern size)
   - Cloud Color (Red, Green, Blue components)
   - Cloud Transparency (alpha/opacity)

See [SETTINGS_GUIDE.md](SETTINGS_GUIDE.md) for detailed instructions and explanations.

## Building

This mod requires:
- RimWorld 1.6
- Harmony mod
- Visual Studio or compatible IDE with .NET Framework 4.7.2

Build the project and ensure the compiled DLL is placed in the `Assemblies` folder.

## Technical Details

- Uses `MapComponent` to manage visual effects per map
- Effects are drawn using Unity's `Graphics.DrawMesh` in the update cycle
- Materials are created programmatically (no external texture files required)
- Effects automatically clean up when they leave the map area or expire

## Mod Structure

```
SkyAmbient/
├── About/
│   └── About.xml
├── Source/
│   ├── SkywardAmbienceMod.cs
│   ├── MapComponent_SkyAmbience.cs
│   ├── SkyEffects/
│   │   ├── BirdFlock.cs
│   │   └── CloudShadow.cs
│   ├── Patches/
│   │   └── Map_FinalizeInit_Patch.cs
│   └── Properties/
│       └── AssemblyInfo.cs
└── Assemblies/
    └── SkywardAmbience.dll (build output)
```

## License

Free to use and modify.
