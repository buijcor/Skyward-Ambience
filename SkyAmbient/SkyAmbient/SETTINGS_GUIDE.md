    # Skyward Ambience - Settings Guide

## How to Open the Settings UI

### Step-by-Step Instructions

1. **Launch RimWorld** and go to the **Main Menu**

2. **Open Mod Settings**
   - Click on **"Options"** in the main menu
   - Select **"Mod Settings"** from the options menu
   - Alternatively, you can access Mod Settings from the **Mods** menu by clicking the **"Settings"** button next to a mod

3. **Find Skyward Ambience**
   - In the Mod Settings window, look for **"Skyward Ambience"** in the list of mods on the left side
   - Click on **"Skyward Ambience"** to open its settings panel

4. **Adjust Settings**
   - The settings panel will appear on the right side
   - Use the sliders to adjust the cloud properties
   - Changes take effect immediately for new cloud shadows

### Quick Access Path

```
Main Menu → Options → Mod Settings → Skyward Ambience
```

Or

```
Main Menu → Mods → [Select Skyward Ambience] → Settings
```

## Available Settings

### Cloud Noise Scale
- **Range**: 0.5 to 5.0
- **Default**: 2.0
- **Description**: Controls the scale of the noise pattern used for cloud shadows
  - **Lower values** (0.5-1.5): Create larger, smoother noise patterns
  - **Higher values** (3.0-5.0): Create smaller, more detailed noise patterns
  - **Default** (2.0): Balanced cloud appearance

### Cloud Color - Red
- **Range**: 0% to 100%
- **Default**: 20%
- **Description**: Adjusts the red component of the cloud shadow color
  - **0%**: No red tint
  - **100%**: Maximum red tint

### Cloud Color - Green
- **Range**: 0% to 100%
- **Default**: 20%
- **Description**: Adjusts the green component of the cloud shadow color
  - **0%**: No green tint
  - **100%**: Maximum green tint

### Cloud Color - Blue
- **Range**: 0% to 100%
- **Default**: 25%
- **Description**: Adjusts the blue component of the cloud shadow color
  - **0%**: No blue tint
  - **100%**: Maximum blue tint

### Cloud Transparency
- **Range**: 10% to 100%
- **Default**: 30%
- **Description**: Controls how transparent or opaque the cloud shadows appear
  - **Lower values** (10-20%): Very transparent, subtle shadows
  - **Medium values** (30-50%): Balanced visibility
  - **Higher values** (70-100%): Very opaque, prominent shadows

### Reset to Defaults Button
- Click this button to restore all cloud settings to their default values
- This will reset:
  - Cloud Noise Scale: 2.0
  - Cloud Color Red: 20%
  - Cloud Color Green: 20%
  - Cloud Color Blue: 25%
  - Cloud Transparency: 30%

## Tips

- **Color Combinations**: Experiment with different RGB values to create various cloud shadow colors
  - Dark gray/black: Low values (10-30%) for all RGB
  - Blue-tinted: Higher blue (40-60%), lower red/green
  - Warm shadows: Higher red (40-60%), lower blue

- **Performance**: All settings are applied in real-time. Changes affect new cloud shadows that spawn after you adjust the settings

- **Visual Testing**: Watch the sky in-game to see how your settings affect the cloud shadows as they drift across the map

## Troubleshooting

**Can't find the settings?**
- Make sure the mod is enabled in the Mods menu
- Check that you're looking in "Mod Settings" (not "Options" directly)
- The mod must be loaded successfully (check the log for any errors)

**Settings not saving?**
- RimWorld automatically saves mod settings
- If settings don't persist, check that you have write permissions in the RimWorld folder
- Settings are saved in: `[RimWorld Folder]/Config/ModSettings.xml`

**Changes not visible?**
- Settings changes should apply immediately to all cloud shadows
- If you don't see changes:
  1. Make sure you're in-game on a map (not in the main menu)
  2. Wait a few seconds for the clouds to update (they update every frame)
  3. Try adjusting the transparency slider to a very high value (80-100%) to make clouds more visible
  4. Try adjusting the color sliders to extreme values (0% or 100%) to see clear differences
  5. If still not working, save and reload the map
  6. Check the RimWorld log for any errors related to Skyward Ambience

