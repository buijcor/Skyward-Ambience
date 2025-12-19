# Testing Skyward Ambience in RimWorld

## Prerequisites

1. **Build the mod first** (see BUILD_INSTRUCTIONS.md)
2. Verify `Assemblies\SkywardAmbience.dll` exists
3. Make sure Harmony mod is enabled (required dependency)

## Step 1: Enable the Mod

1. **Launch RimWorld**
2. **Go to Mods Menu**
   - Click "Mods" button on the main menu
   - Or press `M` key

3. **Enable Required Mods** (in order):
   - ✅ **Core** (should already be enabled)
   - ✅ **Harmony** (brrainz.harmony) - REQUIRED
   - ✅ **Skyward Ambience** (Buijcor.SkywardAmbience)

4. **Check Load Order**
   - Harmony should load BEFORE Skyward Ambience
   - RimWorld usually handles this automatically, but verify if issues occur

5. **Click "Apply"** and restart RimWorld if prompted

## Step 2: Load or Create a Game

1. **Load an existing save** or **Start a new colony**
2. **Wait for the game to fully load** (let the map generate/load completely)

## Step 3: What to Look For

### Bird Flocks
- **Visual**: Small dark silhouettes flying across the sky
- **Shadows**: Dark shadows moving on the ground as birds pass overhead
- **Timing**: Birds spawn randomly, may take 1-5 minutes to appear
- **Behavior**: 
  - Birds enter from map edges
  - Fly across the map in straight or slightly curved paths
  - Cast shadows that move on the ground below them
  - Disappear after crossing the map

### Cloud Shadows
- **Visual**: Large, semi-transparent dark patches on the ground
- **Movement**: Slowly drift across the map
- **Timing**: Clouds spawn less frequently than birds, may take 2-10 minutes
- **Behavior**:
  - Enter from map edges
  - Move slowly across the terrain
  - Vary in size (2-6 tiles across)
  - Fade in/out at edges

## Step 4: Verify It's Working

### Quick Test (5-10 minutes)
1. Load a game and wait 5-10 minutes
2. Watch the sky and ground for:
   - Bird shadows moving across the ground
   - Cloud shadows drifting overhead
   - Dark silhouettes in the sky (birds)

### Extended Test (30+ minutes)
- Play normally and observe over time
- Effects should appear periodically throughout gameplay
- Maximum 3 bird flocks and 5 cloud shadows can be active at once

## Step 5: Check for Errors

### In-Game Log
1. Press `Ctrl+F12` to open the Developer Console
2. Look for any red error messages
3. Check for messages starting with `[Skyward Ambience]`

### Log Files
1. Navigate to: `%USERPROFILE%\AppData\LocalLow\Ludeon Studios\RimWorld by Ludeon Studios\Player.log`
2. Open with Notepad
3. Search for:
   - `SkywardAmbience` - Should see "Mod loaded successfully"
   - `Exception` or `Error` - Check for any related errors
   - `MapComponent_SkyAmbience` - Should see initialization messages

### Common Error Messages

#### "Could not find type 'SkywardAmbience.MapComponent_SkyAmbience'"
- **Cause**: DLL not found or not compiled correctly
- **Fix**: Rebuild the mod and verify DLL is in `Assemblies\` folder

#### "Harmony not found"
- **Cause**: Harmony mod not enabled
- **Fix**: Enable Harmony mod in mod list

#### "NullReferenceException" or crashes
- **Cause**: Code error or missing references
- **Fix**: Check log file for specific error, rebuild mod

## Step 6: Debugging Tips

### If Nothing Appears

1. **Check Mod is Active**
   - Verify mod is enabled in mod list
   - Check mod order (Harmony before Skyward Ambience)

2. **Check Console for Messages**
   - Press `Ctrl+F12` in-game
   - Look for: `[Skyward Ambience] Mod loaded successfully.`
   - If missing, mod didn't load properly

3. **Verify MapComponent is Added**
   - Open Developer Console (`Ctrl+F12`)
   - Type: `Find.Map.GetComponent<MapComponent_SkyAmbience>()`
   - Should return the component instance (not null)

4. **Check Spawn Rates**
   - Bird spawn chance: 0.0002 per tick (very low, be patient)
   - Cloud spawn chance: 0.0001 per tick (even lower)
   - At 60 ticks/second, expect birds every ~8 minutes on average
   - Effects are subtle and infrequent by design

5. **Enable Developer Mode for Debug Messages**
   - Enable Developer Mode: Options → Development Mode
   - When enabled, you'll see log messages when birds/clouds spawn:
     - `[Skyward Ambience] Bird flock spawned. Total active: X`
     - `[Skyward Ambience] Cloud shadow spawned. Total active: X`
   - This helps verify the mod is working even if visuals are hard to see

### Performance Check

- The mod should have **zero performance impact**
- If you notice lag, check the log for errors
- Effects only draw when in camera view (optimized)

## Step 7: Visual Verification Checklist

- [ ] Mod appears in mod list
- [ ] No errors in console on game start
- [ ] Log shows "Mod loaded successfully"
- [ ] MapComponent is present (check via console)
- [ ] Bird shadows appear on ground (after waiting)
- [ ] Bird silhouettes visible in sky (after waiting)
- [ ] Cloud shadows drift across map (after waiting)
- [ ] No crashes or errors during gameplay

## Troubleshooting

### Mod Not Loading
- Verify DLL is in correct location: `Mods\SkyAmbient\Assemblies\SkywardAmbience.dll`
- Check About.xml is valid
- Ensure Harmony is enabled

### Effects Not Visible
- **Be Patient**: Spawn rates are intentionally low for subtlety
- Wait 10-15 minutes of gameplay
- Check camera is zoomed out enough to see sky
- Verify you're looking at the map (not in menus)

### Performance Issues
- Check log for errors
- Verify you're using Release build (not Debug)
- Effects should be minimal impact

## Testing Different Scenarios

1. **Different Map Sizes**: Test on small, medium, and large maps
2. **Different Biomes**: Test in various biomes (desert, forest, tundra, etc.)
3. **Different Times**: Test during day, dusk, and night
4. **Different Weather**: Test in clear, foggy, and rainy weather
5. **Long Play Sessions**: Play for 1+ hours to see multiple spawns

## Expected Behavior

- **Subtle**: Effects are designed to be background ambience, not prominent
- **Infrequent**: Birds and clouds spawn rarely (every 5-15 minutes)
- **Non-Intrusive**: Should not interfere with gameplay
- **Performance**: Zero noticeable impact on FPS

## Success Criteria

✅ Mod loads without errors  
✅ No crashes during gameplay  
✅ Bird shadows appear on ground periodically  
✅ Cloud shadows drift across map occasionally  
✅ No performance degradation  

If all criteria are met, the mod is working correctly!

